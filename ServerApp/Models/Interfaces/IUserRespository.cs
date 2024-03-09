using Microsoft.AspNetCore.Identity;

public interface IUserRepository
{
    Task<ApplicationUser> GetUserByUsername(string username);
    Task<IdentityResult> AddUser(ApplicationUser user);
    Task<ApplicationUser> UpdateUser(ApplicationUser user);
}
