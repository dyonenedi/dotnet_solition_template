using app.auth.Application.Models;

namespace app.auth.Application.Repositories;
public interface IUserRepository: ITransactionRepository
{
    Task<bool> ExistsAsync(string email, string username);
    Task AddAsync(User user);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(int id);
}
