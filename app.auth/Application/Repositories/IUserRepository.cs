using app.auth.Application.Models;

namespace app.auth.Application.Repositories;
public interface IUserRepository
{
    Task<bool> ExistsAsync(string email, string username);
    Task AddAsync(User user);
    Task<User?> GetByEmailAsync(string email);
}
