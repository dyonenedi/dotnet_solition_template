using app.api.Application.DB;

namespace app.api.Application.Services;

public class ErrorLogService 
{
    private readonly AppDbContext _db;
    public ErrorLogService(AppDbContext db) 
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
        catch (Exception e)
        {
            Console.WriteLine($"Failed to log error to database: {e.Message}\n{e.StackTrace}");
        }
    }
}