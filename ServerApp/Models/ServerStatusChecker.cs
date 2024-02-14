public class ServerStatusChecker
{
    private readonly ILogger<ServerControlService> _logger;
    private RCONService _rconConnection;
    private string serverStatus;
    private CancellationTokenSource cts = new CancellationTokenSource();

    public ServerStatusChecker([Service] RCONService rconConnection, ILogger<ServerControlService> logger)
    {
        _logger = logger;
        _rconConnection = rconConnection;
        CheckServerStatus(cts.Token);
    }

    public string ServerStatus
    {
        get { return serverStatus; }
        private set { serverStatus = value; }
    }

    private async Task CheckServerStatus(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                string? checkServerStartup = await _rconConnection.SendServerCommand("Info");
                if (checkServerStartup != null)
                {
                    _logger.LogInformation(checkServerStartup);
                    _logger.LogInformation("Server is online.");
                    ServerStatus = "Online";
                }
                else
                {
                    _logger.LogInformation("Server is not online yet.");
                    ServerStatus = "Offline";
                }
            }
            catch
            {
                _logger.LogError("Error while checking server status.");
                await Task.Delay(3000, cancellationToken);
                ServerStatus = "Offline";
            }
        }
    }

    public void StopChecking()
    {
        cts.Cancel();
    }
}