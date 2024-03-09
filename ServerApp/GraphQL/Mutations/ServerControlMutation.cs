using HotChocolate;
using HotChocolate.Types;

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
            return _serverControlService.RestartServer();
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
            return _serverControlService.StopServer();
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
            return _serverControlService.StartServer();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Server start failed: {ex}");
            return false;
        }
    }
}
