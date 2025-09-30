namespace app.auth.Application.Repositories;
public interface ITransactionRepository
{
    Task BeginTransactionAsync();
    Task<int> SaveChangesAsync();
    Task CommitAsync();
    Task RollbackAsync();
}
