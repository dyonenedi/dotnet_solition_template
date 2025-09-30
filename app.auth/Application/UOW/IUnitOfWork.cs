using app.auth.Application.Models;
using app.auth.Application.Repositories;
using Microsoft.EntityFrameworkCore;

namespace app.auth.Application.UOW;
public interface IUnitOfWork
{
    IUserRepository Users { get; }
    DbSet<UserRole> UserRoles { get; }
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}
