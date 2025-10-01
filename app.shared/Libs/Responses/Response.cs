namespace app.shared.Libs.Responses;

public class Response<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string MessageType { get; set; } = Enums.MessageType.Info;
    public T? Data { get; set; }
    public OperationStatus Status { get; set; } = OperationStatus.Success;

    public Response() { }

    public Response(bool success, string message, string messageType = Enums.MessageType.Info, T? data = default, OperationStatus status = OperationStatus.Success)
    {
        Success = success;
        Message = message;
        MessageType = messageType;
        Data = data;
        Status = status;
    }

    #region Static Methods
    public static Response<T> CreateSuccess(T data, string message = "Operação realizada com sucesso")
    {
        return new Response<T>(true, message, Enums.MessageType.Success, data, OperationStatus.Success);
    }

    public static Response<T> CreateError(string message, T? data = default)
    {
        return new Response<T>(false, message, Enums.MessageType.Error, data, OperationStatus.Error);
    }

    public static Response<T> CreateWarning(string message, T? data = default)
    {
        return new Response<T>(false, message, Enums.MessageType.Warning, data, OperationStatus.ValidationError);
    }

    public static Response<T> CreateInfo(string message, T? data = default)
    {
        return new Response<T>(true, message, Enums.MessageType.Info, data, OperationStatus.Success);
    }
    #endregion

    // Fluent helpers
    public Response<T> WithStatus(OperationStatus status)
    {
        Status = status; return this;
    }
}