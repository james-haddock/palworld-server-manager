public class LoginInput
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class AuthPayload
{
    public string Token { get; set; }
    public User User { get; set; }
}
