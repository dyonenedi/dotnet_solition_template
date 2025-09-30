using app.auth.Application.DB;
using app.auth.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace app.auth.Application.Repositories;
public class UserRepository : IUserRepository
{
    private readonly AuthDbContext _db;
    private IDbContextTransaction transaction = null!;

    #region Constructor and Transaction Methods
    public UserRepository(AuthDbContext db)
    {
        _db = db;
    }
    public async Task BeginTransactionAsync()
    {
        transaction = await _db.Database.BeginTransactionAsync();
    }
    public async Task<int> SaveChangesAsync()
    {
        return await _db.SaveChangesAsync();
    }
    public async Task CommitAsync()
    {
        await _db.Database.CommitTransactionAsync();
    }
    public async Task RollbackAsync()
    {
        await _db.Database.RollbackTransactionAsync();
        transaction.Dispose();
    }
    #endregion

    public async Task<bool> ExistsAsync(string email, string username)
    {
        return await _db.Users.AnyAsync(u => u.Email == email.Trim().ToLower() || u.Username == username.Trim().ToLower());
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
