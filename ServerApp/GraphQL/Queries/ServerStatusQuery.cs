[ExtendObjectType("Query")]
public class ServerStatusQuery
{
        private readonly ILogger<ServerSettingsQuery> _logger;
        private ServerStatusChecker _serverStatusChecker;

        public ServerStatusQuery(ILogger<ServerSettingsQuery> logger, [Service] ServerStatusChecker serverStatusChecker)
        {
            _logger = logger;
            _serverStatusChecker = serverStatusChecker;
        }

        public string getServerStatus()
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
}