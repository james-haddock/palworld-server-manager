using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using (var dbContext = new AppDbContext())
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var existingUser = await userManager.FindByNameAsync("testuser");

            if (existingUser == null)
            {
                var newUser = new ApplicationUser
                {
                    UserName = "testuser",
                    Email = "test@example.com",
                    Role = "User"
                };

                var result = await userManager.CreateAsync(newUser, "Passw0rd!");

                if (result.Succeeded)
                {
                    Console.WriteLine("User created successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to create user.");
                }
            }
            else
            {
                Console.WriteLine("User already exists.");
            }
        }
    }
}
