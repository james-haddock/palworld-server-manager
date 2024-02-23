public class LoginInput
{
    public string User { get; set; }
    public string Password { get; set; }
}

public class UserPayload
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Role { get; set; }
}

public class AuthPayload
{
    public string Token { get; set; }
    public UserPayload User { get; set; }
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

public class LoginPayload
{
    public string Token { get; set; }
    public UserPayload User { get; set; }
}
