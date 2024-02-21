using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

[ExtendObjectType("Mutation")]
public class UserAuthMutation
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;

    public UserAuthMutation(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    public async Task<LoginPayload> Login(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {

            return null;
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        if (!result.Succeeded)
        {

            return null;
        }


        var token = _tokenService.GenerateToken(user);

        return new LoginPayload
        {
            Token = token,
            User = user
        };
    }
}

public class LoginPayload
{
    public string Token { get; set; }
    public ApplicationUser User { get; set; }
}
