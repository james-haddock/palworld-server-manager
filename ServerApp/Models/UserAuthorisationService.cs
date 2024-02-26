using Isopoh.Cryptography.Argon2;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> Authenticate(string username, string password)
    {
        var user = await _userRepository.GetUserByUsername(username);

        if (user == null)
        {
            return null;
        }

        var isValid = Argon2.Verify(user.Password, password);

        if (isValid)
        {
            return user;
        }

        return null;
    }

    public string GenerateJwt(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super secret key"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public async Task<User> RegisterUser(string username, string password)
    {
        var existingUser = await _userRepository.GetUserByUsername(username);
        if (existingUser != null)
        {
            throw new Exception("A user with this username already exists.");
        }

        var hashedPassword = Argon2.Hash(password);

        var user = new User
        {
            Username = username,
            Password = hashedPassword
        };

        await _userRepository.AddUser(user);

        return user;
    }

    public async Task<User> ChangePassword(string username, string oldPassword, string newPassword)
    {
        var user = await Authenticate(username, oldPassword);
        if (user == null)
        {
            throw new Exception("Invalid username or old password.");
        }

        var newHashedPassword = Argon2.Hash(newPassword);

        user.Password = newHashedPassword;

        var result = await _userRepository.UpdateUser(user);
        if (result == null)
        {
            throw new Exception("Password change failed.");
        }

        return result;
    }
}
