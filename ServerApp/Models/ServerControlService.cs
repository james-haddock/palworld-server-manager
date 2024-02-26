using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class ServerControlService
{
    private Process? _serverProcess;
    private string _serverExecutable;
    private readonly ILogger<ServerControlService> _logger;
    private RCONService _rconConnection;


    public ServerControlService(string serverExecutablePath, [Service] RCONService rconConnection, ILogger<ServerControlService> logger)
    {
        _serverExecutable = serverExecutablePath;
        _logger = logger;
        _rconConnection = rconConnection;
    }

    public bool StartServer()
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
                    if (checkServerStartup != null)
                    {
                        _logger.LogInformation(checkServerStartup);
                        serverOnline = true;
                    }
                    else
                    {
                        _logger.LogInformation("Server is not online yet.");
                        if (attemptNumber > 10)
                        {
                            _logger.LogError("Server failed to start.");
                            return false;
                        }
                        attemptNumber++;
                    }
                }
                catch
                // TEST AFTER REMOVING CATCHING THE EXCEPTION
                {
                    _logger.LogError("Error while checking server status.");
                    Thread.Sleep(1000);
                    return false;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to start the server.");
            return false;
        }

        return true;
    }

    public bool StopServer()
    {
        try
        {
            _logger.LogInformation("Shutdown request sent to server.");
            string shutdownResponse;
            shutdownResponse = _rconConnection.SendServerCommand("Shutdown").Result;
            _logger.LogInformation(shutdownResponse);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to stop the server.");
            return false;
        }
    }

    public bool RestartServer()
    {
        try
        {
            StopServer();
            _logger.LogInformation($"Checking server has shutdown...");
            bool serverOnline = true;
            while (serverOnline)
            {
                Thread.Sleep(2000);
                string? checkServerStartup;
                checkServerStartup = _rconConnection.SendServerCommand("Info").Result;
                if (checkServerStartup != null)
                {
                    _logger.LogInformation(checkServerStartup);
                    serverOnline = true;
                }
                else
                {
                    _logger.LogInformation("Server has shutdown.");
                    serverOnline = false;
                    Thread.Sleep(1000);
                }
            }

            _logger.LogInformation("Restarting server...");
            Thread.Sleep(1000);
            StartServer();
            _logger.LogInformation("Server restarted successfully.");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to restart the server.");
            return false;
        }
    }
}
