using app.auth.Application.Db;

namespace app.auth.Application.Services;

public class ErrorLogService 
{
    private readonly AuthDbContext _db;
    public ErrorLogService(AuthDbContext db) 
    {
        _db = db;
    }

    public async Task LogErrorAsync(int userId, string source, Exception ex) 
    {
        try 
        {
            var errorLog = new Models.ErrorLog 
            {
                UserId = userId,
                Source = source,
                ExceptionType = ex.GetType().Name,
                Message = ex.Message,
                StackTrace = ex.StackTrace ?? "",
                CreateDate = DateTime.UtcNow
            };

            _db.ErrorLogs.Add(errorLog);
            await _db.SaveChangesAsync();
        }
        catch 
        {
            // Evitar loops infinitos de logging
            // Opcional: log em arquivo ou EventLog do Windows
        }
    }
}