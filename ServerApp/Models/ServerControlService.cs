using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class ServerControlService
{
    private Process? _serverProcess;
    private string _serverExecutable;
    private readonly ILogger<ServerControlService> _logger;
    private RCONService _rconConnection;

    public ServerControlService([Service] RCONService rconConnection, ILogger<ServerControlService> logger)
    {
        _serverExecutable = "../InstallApp/bin/Debug/net8.0/steamcmd/steamapps/common/PalServer/PalServer.exe";
        _logger = logger;
        _rconConnection = rconConnection;
    }

    public void StartServer()
    {
        Task.Run(() =>
        {
            try
            {
                _logger.LogInformation("Initialising Palworld Server.");
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
                bool serverOnline = false;
                int attemptNumber = 1;
                while (!serverOnline)
                {
                    try
                    {   
                        Thread.Sleep(2000);
                        _logger.LogInformation($"Checking server is online...");
                        string? checkServerStartup;
                        checkServerStartup = _rconConnection.SendServerCommand("Info").Result;
                        if(checkServerStartup != null){
                            _logger.LogInformation(checkServerStartup);
                        serverOnline = true;
                        }
                        else
                        {
                            _logger.LogInformation("Server is not online yet.");
                            if (attemptNumber > 10)
                            {
                                _logger.LogError("Server failed to start.");
                                return;
                            }
                            attemptNumber++;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error while checking server status.");
                        Thread.Sleep(1000);
                    }
                }
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
            _logger.LogInformation("Shutdown request sent to server.");
            string shutdownResponse;
            shutdownResponse = _rconConnection.SendServerCommand("Shutdown").Result;
            _logger.LogInformation(shutdownResponse);
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