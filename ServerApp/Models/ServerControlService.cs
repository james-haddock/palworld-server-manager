using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class ServerControlService
{
    private Process? _serverProcess;
    private string _serverExecutable;
    private readonly ILogger<ServerControlService> _logger;

    public ServerControlService(ILogger<ServerControlService> logger)
    {
        _serverExecutable = "../InstallApp/bin/Debug/net8.0/steamcmd/steamapps/common/PalServer/PalServer.exe";
        _logger = logger;
    }

    public void StartServer()
    {
        Task.Run(() =>
        {
            try
            {
                _serverProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = _serverExecutable,
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = false,
                    }
                };

                _serverProcess.Start();
                _logger.LogInformation("Server started successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to start the server.");
            }
        });
    }

    public void StopServer()
    {
        try
        {
            _serverProcess?.CloseMainWindow();
            _logger.LogInformation("Server stopped successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to stop the server.");
        }
    }

    public void RestartServer()
    {
        try
        {
            StopServer();
            StartServer();
            _logger.LogInformation("Server restarted successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to restart the server.");
        }
    }
}