using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration, ILogger<AuthService> logger)
    {
        _userManager = userManager;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<ApplicationUser> Authenticate(string username, string password)
    {
        _logger.LogInformation("Authenticating user {Username}", username);

        try
        {
            _logger.LogDebug("Finding user by username: {Username}", username);
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                _logger.LogWarning("User not found: {Username}", username);
                return null;
            }

            _logger.LogDebug("Checking password for user: {Username}", username);
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);

            if (!isPasswordValid)
            {
                _logger.LogWarning("Invalid password for user: {Username}", username);
                return null;
            }

            _logger.LogInformation("User {Username} authenticated successfully", username);
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while authenticating user {Username}", username);
            throw;
        }
    }

    public string GenerateJwt(ApplicationUser user)
    {
        _logger.LogInformation("Generating JWT for user {Username}", user.UserName);

        try
        {
            _logger.LogDebug("Creating claims for user {Username}", user.UserName);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(ClaimTypes.Role, user.Role) // Add user role claim
            };

            _logger.LogDebug("Retrieving JWT secret from configuration");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            _logger.LogDebug("Creating signing credentials");
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            _logger.LogDebug("Creating JWT token");
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds
            );

            _logger.LogDebug("Writing JWT token");
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            _logger.LogInformation("JWT generated for user {Username}: {TokenString}", user.UserName, tokenString);

            return tokenString;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while generating JWT for user {Username}", user.UserName);
            throw;
        }
    }

    public async Task<IdentityResult> RegisterUser(RegisterInput input)
    {
        _logger.LogInformation("Registering user {Username}", input.Username);
        using (var dbContext = new AppDbContext())
        {
            var user = new ApplicationUser { UserName = input.Username };
            var result = await _userManager.CreateAsync(user, input.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User {Username} registered successfully", input.Username);
            }
            else
            {
                _logger.LogWarning("Registration failed for user {Username}", input.Username);
            }

            return result;
        }
    }

    public async Task<IdentityResult> ChangePassword(ApplicationUser user, string newPassword)
    {
        using (var dbContext = new AppDbContext())
        {
            _logger.LogInformation("Changing password for user {Username}", user.UserName);

            var result = await _userManager.ChangePasswordAsync(user, user.PasswordHash, newPassword);

            if (result.Succeeded)
            {
                _logger.LogInformation("Password changed successfully for user {Username}", user.UserName);
            }
            else
            {
                _logger.LogWarning("Password change failed for user {Username}", user.UserName);
            }

            return result;
        }
    }
}
