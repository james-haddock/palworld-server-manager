using Microsoft.AspNetCore.Identity;

public interface IAuthService
{
    Task<ApplicationUser> Authenticate(string username, string password);
    string GenerateJwt(ApplicationUser user);
    Task<IdentityResult> RegisterUser(RegisterInput input);
    Task<IdentityResult> ChangePassword(ApplicationUser user, string newPassword);
}
