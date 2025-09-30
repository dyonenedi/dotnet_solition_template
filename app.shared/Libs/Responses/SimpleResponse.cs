using app.shared.Libs.Enums;

namespace app.shared.Libs.Responses;

public class SimpleResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string MessageType { get; set; } = Enums.MessageType.Info;

    public SimpleResponse() { }

    public SimpleResponse(bool success, string message, string messageType = Enums.MessageType.Info)
    {
        Success = success;
        Message = message;
        MessageType = messageType;
    }

    #region Static Methods
    public static SimpleResponse CreateSuccess(string message = "Operação realizada com sucesso")
    {
        return new SimpleResponse(true, message, Enums.MessageType.Success);
    }

    public static SimpleResponse CreateError(string message)
    {
        return new SimpleResponse(false, message, Enums.MessageType.Error);
    }

    public static SimpleResponse CreateWarning(string message)
    {
        return new SimpleResponse(false, message, Enums.MessageType.Warning);
    }

    public static SimpleResponse CreateInfo(string message)
    {
        return new SimpleResponse(true, message, Enums.MessageType.Info);
    }
    #endregion
}