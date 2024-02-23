using System.Threading.Tasks;

public interface IAuthService
{
    Task<User> Authenticate(string username, string password);
    string GenerateJwt(User user);
    Task<User> RegisterUser(string username, string password);
    Task<User> ChangePassword(string username, string oldPassword, string newPassword);
}