public class ServerStatusChecker
{
    private readonly ILogger<ServerControlService> _logger;
    private RCONService _rconConnection;
    private string serverStatus = "Offline";
    private CancellationTokenSource cts = new CancellationTokenSource();

    public ServerStatusChecker([Service] RCONService rconConnection, ILogger<ServerControlService> logger)
    {
        _logger = logger;
        _rconConnection = rconConnection;
        Task.Run(() => CheckServerStatus(cts.Token));
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
            _logger.LogInformation("Retrieving server status for app.");
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
                    _logger.LogInformation("Server is not online.");
                    ServerStatus = "Offline";
                }
            }
            catch
            {
                _logger.LogInformation("Server is not online.");
                ServerStatus = "Offline";
            }
            Thread.Sleep(3000);
        }
}
public void StopChecking()
{
    cts.Cancel();
}
}
