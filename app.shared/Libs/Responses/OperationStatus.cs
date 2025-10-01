namespace app.shared.Libs.Responses;

public enum OperationStatus
{
    Success = 200,            // OK
    Created = 201,            // Created
    ValidationError = 400,    // Bad Request
    Unauthorized = 401,       // Unauthorized
    Forbidden = 403,          // Forbidden
    NotFound = 404,           // Not Found
    Conflict = 409,           // Conflict
    Error = 500,               // Internal Server Error
    Unavailable = 503,         // Serviço temporariamente indisponível
}
