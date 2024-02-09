using System.Diagnostics;
using System.Threading.Tasks;

public class ServerControlService
{
    private Process _serverProcess;
    private string _serverExecutable;

    public ServerControlService(string serverPath)
    {
        _serverExecutable = serverPath;
    }

    public void StartServer()
    {
        Task.Run(() =>
        {
            _serverProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _serverExecutable,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };

            _serverProcess.Start();
            _serverProcess.WaitForExit();
        });
    }

    public void StopServer()
    {
        _serverProcess?.CloseMainWindow();
        _serverProcess = null;
    }

    public void RestartServer()
    {
        StopServer();
        StartServer();
    }
}