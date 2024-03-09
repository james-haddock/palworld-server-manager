public class ServerStatusChecker
{
    private readonly ILogger<ServerControlService> _logger;
    private RCONService _rconConnection;
    private string serverStatus = "Offline";
    private string serverInfo = "Offline";
    private List<Player> playersOnline = new List<Player>();
    private string serverUpTime = "Offline";
    private CancellationTokenSource cts = new CancellationTokenSource();
    private DateTime? serverStartTime = null;

    public ServerStatusChecker([Service] RCONService rconConnection, ILogger<ServerControlService> logger)
    {
        _logger = logger;
        _rconConnection = rconConnection;
        Task.Run(() => CheckServerStatus(cts.Token));
    }

    public string ServerUpTime
    {
        get
        {
            if (serverStartTime.HasValue)
            {
                TimeSpan uptime = DateTime.Now - serverStartTime.Value;
                return $"{uptime.Days}d {uptime.Hours}h {uptime.Minutes}m {uptime.Seconds}s";
            }
            else
            {
                return "Offline";
            }
        }
    }

    public string ServerStatus
    {
        get { return serverStatus; }
        private set { serverStatus = value; }
    }

    public string ServerInfo
    {
        get { return serverInfo; }
        private set { serverInfo = value; }
    }
    // public List<Player> PlayersOnline
    // {
    //     get { return playersOnline; }
    //     private set { playersOnline = value; }
    // }

    private async Task CheckServerStatus(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            // _logger.LogInformation("Retrieving server status for app.");
            try
            {
                string? checkServerStartup = await _rconConnection.SendServerCommand("Info");
                if (checkServerStartup != null)
                {
                    // _logger.LogInformation(checkServerStartup);
                    // _logger.LogInformation("Server is online.");
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
            try
            {
                string? serverInfo = await _rconConnection.SendServerCommand("Info");
                if (serverInfo != null)
                {
                    // _logger.LogInformation(serverInfo);
                    ServerInfo = serverInfo;
                }
                else
                {
                    _logger.LogInformation("Server info is not available.");
                    ServerInfo = "Server info is not available.";
                }
            }
            catch
            {
                _logger.LogInformation("Server info is not available.");
                ServerInfo = "Server info is not available.";
            }
            //     try
            //     {
            //         List<Player> playersOnline = await _rconConnection.SendServerCommand("ShowPlayers");
            //         if (playersOnline != null)
            //         {
            //             _logger.LogInformation(playersOnline);
            //             PlayersOnline = playersOnline;
            //         }
            //         else
            //         {
            //             _logger.LogInformation("No players are online.");
            //             PlayersOnline = "No players are online.";
            //         }
            //     }
            //     catch
            //     {
            //         _logger.LogInformation("No players are online.");
            //         PlayersOnline = "No players are online.";
            //     }
            //     Thread.Sleep(3000);
            if (ServerStatus == "Online")
            {
                if (!serverStartTime.HasValue)
                {
                    serverStartTime = DateTime.Now;
                }
            }
            else
            {
                serverStartTime = null;
            }
            Thread.Sleep(3000);
        }
    }
    public void StopChecking()
    {
        cts.Cancel();
    }
}
