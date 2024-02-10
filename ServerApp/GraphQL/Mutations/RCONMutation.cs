[ExtendObjectType("Mutation")]
public class MutationRCON
{
    public async Task<SendRconCommandPayload> SendRconCommand(SendRconCommandInput input, [Service] RCONConnection rconConnection)
    {
        string command = input.Command;
        if (input.Value != null)
        {
            command += " " + input.Value;
        }

        var response = await rconConnection.SendCommandToServer(command);
        return new SendRconCommandPayload { Response = response };
    }
}