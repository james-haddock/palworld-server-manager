public class Player
{
    public string ?Username { get; set; }
    public string ?SteamId { get; set; }
}

public class ServerStats
{
    public string ?IpAddress { get; set; }
    public string ?ServerInfo { get; set; }
    public string ?ServerStatus { get; set; }
    // public List<Player> ?PlayersOnline { get; set; }
    public string ?ServerUpTime { get; set; }
}
