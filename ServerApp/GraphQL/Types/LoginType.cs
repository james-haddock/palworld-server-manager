public class LoginInput
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class UserPayload
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Role { get; set; }
    public string Token { get; set; }
    public string ErrorMessage { get; set; }
}

public class RegisterInput
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class RegisterPayload
{
    public string Token { get; set; }
    public UserPayload User { get; set; }
}

public class ChangePasswordInput
{
    public string Username { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}

public class ChangePasswordPayload
{
    public string Token { get; set; }
    public UserPayload User { get; set; }
}
