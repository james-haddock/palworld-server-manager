using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ApplicationUser> GetUserByUsername(string username)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == username);
    }

    public async Task<IdentityResult> AddUser(ApplicationUser user)
    {
        var result = await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        return result.Result;
    }

    public async Task<ApplicationUser> UpdateUser(ApplicationUser user)
    {
        var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
        if (existingUser == null)
            return null;

        _dbContext.Entry(existingUser).CurrentValues.SetValues(user);
        await _dbContext.SaveChangesAsync();
        return existingUser;
    }
}
