public interface IUserRepository
{
    Task<User> GetUserByUsername(string username);
    Task AddUser(User user);
    Task<User> UpdateUser(User user);
}