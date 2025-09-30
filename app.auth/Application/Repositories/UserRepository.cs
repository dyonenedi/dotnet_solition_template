using app.auth.Application.Db;
using app.auth.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace app.auth.Application.Repositories;
public class UserRepository : IUserRepository
{
    private readonly AuthDbContext _db;

    public UserRepository(AuthDbContext db)
    {
        _db = db;
    }

    public async Task<bool> ExistsAsync(string email, string username)
    {
        return await _db.Users.AnyAsync(u => u.Email == email || u.Username == username);
    }

    public async Task AddAsync(User user)
    {
        await _db.Users.AddAsync(user);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _db.Users.FirstOrDefaultAsync(u => u.Email == email && u.Active == true);
    }
}
