using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class ServerAdminService
{
    private Process? _serverProcess;
    private string _serverExecutable;
    private readonly ILogger<ServerControlService> _logger;
    private RCONService _rconConnection;

    public ServerAdminService([Service] RCONService rconConnection, ILogger<ServerControlService> logger)
    {
        _logger = logger;
        _rconConnection = rconConnection;
    }

}
