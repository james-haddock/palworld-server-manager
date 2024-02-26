[ExtendObjectType("Mutation")]
public class MutationRCON
{
    public async Task<SendRconCommandPayload> SendRconCommand(SendRconCommandInput input, [Service] RCONService rconConnection)
    {
        string? command = input.Command;
        if (input.Value != null)
        {
            command += " " + input.Value;
        }

        var response = await rconConnection.SendServerCommand(command);
        return new SendRconCommandPayload { Response = response };
    }
}
