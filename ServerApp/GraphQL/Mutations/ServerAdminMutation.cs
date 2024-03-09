using HotChocolate;
using HotChocolate.Types;

[ExtendObjectType("Mutation")]
public class ServerAdminMutation
{
    private readonly ServerAdminService _serverAdminService;

    public ServerAdminMutation(ServerAdminService serverAdminService)
    {
        _serverAdminService = serverAdminService;
    }
}
