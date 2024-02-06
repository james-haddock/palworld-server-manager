 using CoreRCON;
using CoreRCON.Parsers.Standard;
using System.Net;

public class RCONConnection
{
    private readonly RCON server;
public RCONConnection(string ipAddress, int port, string password)
{
    IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
    server = new RCON(endpoint, password);
}
public async Task<string> SendCommandToServer(string serverCommand)
{
    string response = await server.SendCommandAsync(serverCommand);
    return response;
}
}