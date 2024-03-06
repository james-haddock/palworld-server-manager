using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public string Role { get; set; }
}
