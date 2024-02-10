[ExtendObjectType("Mutation")]
public class ServerControlMutation
{
    private readonly ServerControlService _serverControlService;

    public ServerControlMutation(ServerControlService serverControlService)
    {
        _serverControlService = serverControlService;
    }

    public bool restartServer()
    {
        try
        {
            _serverControlService.RestartServer();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Server restart failed: {ex}");
            return false;
        }
    }

    public bool stopServer()
    {
        try
        {
            _serverControlService.StopServer();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Server stop failed: {ex}");
            return false;
        }
    }

    public bool startServer()
    {
        try
        {
            _serverControlService.StartServer();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Server start failed: {ex}");
            return false;
        }
    }
}