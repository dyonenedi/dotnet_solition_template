using app.auth.Application.Services;
using app.shared.Libs.DTOs.User;
using app.shared.Libs.Responses;
using app.auth.Application.Models;

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
                    OperationStatus.Success => Results.Created($"/v1/user/{dto.Username}", response),
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
                    OperationStatus.ValidationError => Results.BadRequest(response),
                    OperationStatus.Unauthorized => Results.Unauthorized(),
                    _ => Results.Problem(detail: response.Message, statusCode: 500, title: "Erro interno do servidor")
                };
            })
            .WithName("LoginUser")
            .WithSummary("Authenticate user")
            .WithDescription("Authenticates user with email and password")
            .Produces<Response<User>>(200, "application/json")   // Success
            .Produces<Response<User>>(400, "application/json")   // Bad Request (validação)
            .Produces(401)                                       // Unauthorized (credenciais inválidas)
            .ProducesProblem(500);
            #endregion
        }
    }
}