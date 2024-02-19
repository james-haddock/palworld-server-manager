
[ExtendObjectType("Query")]
public class ServerStatusQuery
{
    private readonly ILogger<ServerStatusQuery> _logger;
    private ServerStatusChecker _serverStatusChecker;

    public ServerStatusQuery(ILogger<ServerStatusQuery> logger, [Service] ServerStatusChecker serverStatusChecker)
    {
        _logger = logger;
        _serverStatusChecker = serverStatusChecker;
    }

    public string GetServerStatus()
    {
        try
        {
            _logger.LogInformation("Retrieving server status for client.");
            return _serverStatusChecker.ServerStatus;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting server status");
            throw;
        }
    }

    public async Task<ServerStats> GetServerInfo()
    {
        try
        {
            _logger.LogInformation("Retrieving server info for client.");
            return new ServerStats
            {
                IpAddress = await GetIpAddress.GetPublicIpAddressAsync(),
                ServerInfo = _serverStatusChecker.ServerInfo,
                ServerStatus = _serverStatusChecker.ServerStatus,
                // PlayersOnline = _serverStatusChecker.PlayersOnline,
                ServerUpTime = _serverStatusChecker.ServerUpTime
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting server info");
            throw;
        }
    }
}
