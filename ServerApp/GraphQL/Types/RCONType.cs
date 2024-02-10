public class SendRconCommandInput
{
    public string Command { get; set; }
    public string? Value { get; set; }
}

public class SendRconCommandPayload
{
    public string Response { get; set; }
}