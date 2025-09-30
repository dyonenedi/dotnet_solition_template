using app.auth.Application.Db;
using app.auth.Application.Models;
using app.auth.Application.Repositories;
using app.auth.Application.UOW;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

public class UnitOfWork : IUnitOfWork {
    private readonly AuthDbContext _db;
    private IDbContextTransaction? _transaction;
    public IUserRepository Users { get; }
    public DbSet<UserRole> UserRoles => _db.UserRoles;

    public UnitOfWork(AuthDbContext db, IUserRepository userRepository) {
        _db = db;
        Users = userRepository;
    }

    public async Task BeginTransactionAsync() {
        _transaction = await _db.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync() {
        if (_transaction != null) await _transaction.CommitAsync();
    }

    public async Task RollbackAsync() {
        if (_transaction != null) await _transaction.RollbackAsync();
    }

    public async Task<int> SaveChangesAsync() {
        return await _db.SaveChangesAsync();
    }
}
