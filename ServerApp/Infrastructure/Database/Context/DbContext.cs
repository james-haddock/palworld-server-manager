using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext()
        : base(GetContextOptions())
    {
    }

    private static DbContextOptions<AppDbContext> GetContextOptions()
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlite("Data Source=PalworldServerManagerDb.db");
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Debug);
        return optionsBuilder.Options;
    }
}
