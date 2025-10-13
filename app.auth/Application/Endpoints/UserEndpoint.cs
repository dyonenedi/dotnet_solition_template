using app.auth.Application.Services;
using app.shared.Libs.DTOs.User;
using app.shared.Libs.Responses;
using app.auth.Application.Utils;

namespace app.auth.Application.Endpoints
{
    public static class UserEndpoint
    {
        public static void MapUserEndpoints(this WebApplication app)
        {

            var userGroup = app.MapGroup("v1/user")
                .WithTags("User")
                .WithDescription("User management endpoints");

            #region REGISTER
            userGroup.MapPost("register", async (
                RegisterDTO dto,
                UserService userService) =>
            {
                var response = await userService.RegisterAsync(dto);

                return response.Status switch
                {
                    OperationStatus.Success => Results.Ok(response),
                    OperationStatus.ValidationError => Results.BadRequest(response),
                    OperationStatus.Conflict => Results.Conflict(response),
                    _ => Results.Problem(detail: response.Message, statusCode: 500, title: "Erro interno do servidor")
                };
            })
            .WithName("RegisterUser")
            .WithSummary("Register a new user")
            .WithDescription("Creates a new user account with the provided information")
            .Produces<SimpleResponse>(201, "application/json")  // Created
            .Produces<SimpleResponse>(400, "application/json")  // Bad Request (validação)
            .Produces<SimpleResponse>(409, "application/json")  // Conflict (já existe)
            .ProducesProblem(500);
            #endregion

            #region LOGIN
            userGroup.MapPost("login", async (
                LoginDTO dto,
                UserService userService) =>
            {
                var response = await userService.AuthenticateAsync(dto);

                return response.Status switch
                {
                    OperationStatus.Success => Results.Ok(response),
                    OperationStatus.Unauthorized => Results.Json(response, statusCode: 401),
                    OperationStatus.ValidationError => Results.BadRequest(response),
                    _ => Results.Problem(detail: response.Message, statusCode: 500, title: "Erro interno do servidor")
                };
            })
            .WithName("LoginUser")
            .WithSummary("Authenticate user")
            .WithDescription("Authenticates user with email and password")
            .Produces<Response<LoginDTO>>(200, "application/json")   // Success
            .Produces<Response<LoginDTO>>(401, "application/json")   // Unauthorized (credenciais inválidas)
            .Produces<Response<LoginDTO>>(400, "application/json")   // Bad Request (validação)
            .ProducesProblem(500);
            #endregion

            #region GET USER DATA
            userGroup.MapGet("getUserData", async (
                HttpContext httpContext,
                UserService userService) =>
            {
                var config = httpContext.RequestServices.GetRequiredService<IConfiguration>();
                var JwtUtils = httpContext.RequestServices.GetRequiredService<IJwtTokenService>();
                var jwtToken = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var jwtSecret = config["Secret"] ?? string.Empty;
                if (string.IsNullOrEmpty(jwtToken) || string.IsNullOrEmpty(jwtSecret))
                {
                    return Results.Json(Response<UserDTO>.CreateError("Token inexistente", null, OperationStatus.Unauthorized), statusCode: 401);
                }

                var principal = await JwtUtils.ValidateToken(jwtToken, jwtSecret);
                if (principal == null)
                    return Results.Json(Response<UserDTO>.CreateError("Token inválido", null, OperationStatus.Unauthorized), statusCode: 401);
                var userId = principal?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Results.Json(Response<UserDTO>.CreateError("Token inválido", null, OperationStatus.Unauthorized), statusCode: 401);
                    
                var response = await userService.GetUserByIdAsync(int.Parse(userId));

                return response.Status switch
                {
                    OperationStatus.Success => Results.Ok(response),
                    OperationStatus.NotFound => Results.NotFound(response),
                    OperationStatus.Unauthorized => Results.Json(response, statusCode: 401),
                    _ => Results.Problem(detail: response.Message, statusCode: 500, title: "Erro interno do servidor")
                };
            }).WithName("GetUserData")
            .WithSummary("Get user data")
            .WithDescription("Get user data by token")
            .Produces<Response<UserDTO>>(200, "application/json")  // OK
            .Produces<Response<UserDTO>>(401, "application/json")  // Unauthorized
            .Produces<Response<UserDTO>>(400, "application/json")  // Bad Request (validação)
            .Produces<Response<UserDTO>>(409, "application/json")  // Conflict (já existe)
            .ProducesProblem(500);
            #endregion
        }
    }
}