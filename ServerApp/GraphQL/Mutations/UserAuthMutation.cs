[ExtendObjectType("Mutation")]
public class UserAuthMutation
{
    private readonly IAuthService _authService;

    public UserAuthMutation(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<UserPayload> Login(LoginInput input)
    {
        var user = await _authService.Authenticate(input.Username, input.Password);
        if (user == null)
            throw new Exception("Invalid username or password.");

        var token = _authService.GenerateJwt(user);

        return new UserPayload
        {
            Id = user.Id,
            Username = user.UserName,
            Role = user.Role,
            Token = token
        };
    }

    public async Task<RegisterPayload> Register(RegisterInput input)
    {
        var result = await _authService.RegisterUser(input);

        if (!result.Succeeded)
            throw new Exception("Registration failed.");

        var user = await _authService.Authenticate(input.Username, input.Password);
        var token = _authService.GenerateJwt(user);

        return new RegisterPayload
        {
            Token = token,
            User = new UserPayload
            {
                Id = user.Id,
                Username = user.UserName,
                Role = user.Role
            }
        };
    }

    public async Task<ChangePasswordPayload> ChangePassword(ChangePasswordInput input)
    {
        var user = await _authService.Authenticate(input.Username, input.OldPassword);
        if (user == null)
            throw new Exception("Invalid username or password.");

        var result = await _authService.ChangePassword(user, input.NewPassword);

        if (!result.Succeeded)
            throw new Exception("Password change failed.");

        var token = _authService.GenerateJwt(user);

        return new ChangePasswordPayload
        {
            Token = token,
            User = new UserPayload
            {
                Id = user.Id,
                Username = user.UserName,
                Role = user.Role
            }
        };
    }
}
