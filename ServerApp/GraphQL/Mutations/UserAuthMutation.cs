[ExtendObjectType("Mutation")]
public class UserAuthMutation
{
    private readonly IAuthService _authService;

    public UserAuthMutation(IAuthService authService)
    {
        _authService = authService;
    }

    private UserPayload CreateUserPayload(User user)
    {
        return new UserPayload
        {
            Id = user.Id,
            Username = user.Username,
            Role = user.Role
        };
    }

    public async Task<LoginPayload> Login(string username, string password)
    {
        var user = await _authService.Authenticate(username, password);
        if (user == null)
        {
            throw new Exception("Invalid username or password.");
        }

        var token = _authService.GenerateJwt(user);

        return new LoginPayload
        {
            Token = token,
            User = CreateUserPayload(user)
        };
    }

    public async Task<RegisterPayload> Register(string username, string password)
    {
        User user = await _authService.RegisterUser(username, password);

        var token = _authService.GenerateJwt(user);

        return new RegisterPayload
        {
            Token = token,
            User = CreateUserPayload(user)
        };
    }

    public async Task<ChangePasswordPayload> ChangePassword(string username, string oldPassword, string newPassword)
    {
        var user = await _authService.ChangePassword(username, oldPassword, newPassword);

        var token = _authService.GenerateJwt(user);

        return new ChangePasswordPayload
        {
            Token = token,
            User = CreateUserPayload(user)
        };
    }
}