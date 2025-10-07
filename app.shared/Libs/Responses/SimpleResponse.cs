using app.shared.Libs.Enums;

namespace app.shared.Libs.Responses;

public class SimpleResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string MessageType { get; set; } = Enums.MessageType.Info;
    public OperationStatus Status { get; set; } = OperationStatus.Success;

    public SimpleResponse() { }

    public SimpleResponse(bool success, string message, string messageType = Enums.MessageType.Info, OperationStatus status = OperationStatus.Success)
    {
        Success = success;
        Message = message;
        MessageType = messageType;
        Status = status;
    }

    #region Static Methods
    public static SimpleResponse CreateSuccess(string message = "Operação realizada com sucesso")
    {
        return new SimpleResponse(true, message, Enums.MessageType.Success, OperationStatus.Success);
    }

    public static SimpleResponse CreateError(string message, OperationStatus status = OperationStatus.Error)
    {
        return new SimpleResponse(false, message, Enums.MessageType.Error, status);
    }

    public static SimpleResponse CreateWarning(string message)
    {
        return new SimpleResponse(false, message, Enums.MessageType.Warning, OperationStatus.ValidationError);
    }

    public static SimpleResponse CreateInfo(string message)
    {
        return new SimpleResponse(true, message, Enums.MessageType.Info, OperationStatus.Success);
    }
    #endregion

    // Fluent helpers
    public SimpleResponse WithStatus(OperationStatus status)
    {
        Status = status; return this;
    }
}