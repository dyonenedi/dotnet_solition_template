namespace app.shared.Libs.Responses;

public class Response<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string MessageType { get; set; } = Enums.MessageType.Info;
    public T? Data { get; set; }

    public Response() { }

    public Response(bool success, string message, string messageType = Enums.MessageType.Info, T? data = default)
    {
        Success = success;
        Message = message;
        MessageType = messageType;
        Data = data;
    }

    #region Static Methods
    public static Response<T> CreateSuccess(T data, string message = "Operação realizada com sucesso")
    {
        return new Response<T>(true, message, Enums.MessageType.Success, data);
    }

    public static Response<T> CreateError(string message, T? data = default)
    {
        return new Response<T>(false, message, Enums.MessageType.Error, data);
    }

    public static Response<T> CreateWarning(string message, T? data = default)
    {
        return new Response<T>(false, message, Enums.MessageType.Warning, data);
    }

    public static Response<T> CreateInfo(string message, T? data = default)
    {
        return new Response<T>(true, message, Enums.MessageType.Info, data);
    }
    #endregion
}