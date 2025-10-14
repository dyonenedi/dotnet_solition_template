using app.shared.Libs.Responses;

public class Result<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public OperationStatus Status { get; set; }

    public static Result<T> Ok(T? data, string? message = null) =>
    new() { Success = true, Data = data, Message = message, Status = OperationStatus.Success };
    public static Result<T> Fail(string message, OperationStatus status = OperationStatus.Error, T? data = default) =>
    new() { Success = false, Message = message, Status = status, Data = data };
    }
