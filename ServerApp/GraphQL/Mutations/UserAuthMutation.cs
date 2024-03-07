using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using HotChocolate;

[ExtendObjectType("Mutation")]
public class UserAuthMutation
{
    private readonly IAuthService _authService;
    private readonly ILogger<UserAuthMutation> _logger;

    public UserAuthMutation(IAuthService authService, ILogger<UserAuthMutation> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    public async Task<UserPayload> Login(LoginInput input)
    {
        _logger.LogInformation("Login attempt for user {Username}", input.Username);

        var user = await _authService.Authenticate(input.Username, input.Password);
        if (user == null)
        {
            _logger.LogWarning("Invalid username or password for user {Username}", input.Username);
            throw new Exception("Invalid username or password.");
        }

        var token = _authService.GenerateJwt(user);

        _logger.LogInformation("User {Username} logged in successfully", input.Username);

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
        _logger.LogInformation("Registration attempt for user {Username}", input.Username);

        var result = await _authService.RegisterUser(input);

        if (!result.Succeeded)
        {
            _logger.LogWarning("Registration failed for user {Username}", input.Username);
            throw new Exception("Registration failed.");
        }

        var user = await _authService.Authenticate(input.Username, input.Password);
        var token = _authService.GenerateJwt(user);

        _logger.LogInformation("User {Username} registered successfully", input.Username);

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
        _logger.LogInformation("Password change attempt for user {Username}", input.Username);

        var user = await _authService.Authenticate(input.Username, input.OldPassword);
        if (user == null)
        {
            _logger.LogWarning("Invalid username or password for user {Username}", input.Username);
            throw new Exception("Invalid username or password.");
        }

        var result = await _authService.ChangePassword(user, input.NewPassword);

        if (!result.Succeeded)
        {
            _logger.LogWarning("Password change failed for user {Username}", input.Username);
            throw new Exception("Password change failed.");
        }

        var token = _authService.GenerateJwt(user);

        _logger.LogInformation("Password for user {Username} changed successfully", input.Username);

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
