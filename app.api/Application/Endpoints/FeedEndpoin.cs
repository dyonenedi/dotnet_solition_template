using app.api.Application.Services;
using app.shared.Libs.DTOs.Feed;
using app.shared.Libs.Responses;

namespace app.api.Application.Endpoints
{
    public static class FeedEndpoint
    {
        // Método auxiliar para validar e extrair JWT
        private static bool TryGetJwt(HttpContext httpContext, IConfiguration config, out string jwtToken, out string jwtSecret)
        {
            jwtToken = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            jwtSecret = config["Secret"] ?? string.Empty;
            return !string.IsNullOrEmpty(jwtToken) && !string.IsNullOrEmpty(jwtSecret);
        }
        public static void MapFeedEndpoints(this WebApplication app)
        {
            var feedEndpoint = app.MapGroup("v1/feed")
                .WithTags("Feed")
                .WithDescription("Feed management endpoints");

            #region Post Post
            feedEndpoint.MapPost("post", async (HttpContext httpContext, PostDto dto, FeedService feedService, IConfiguration config) =>
            {
                if (!TryGetJwt(httpContext, config, out var jwtToken, out var jwtSecret))
                {
                    return Results.Unauthorized();
                }
                var result = await feedService.PostAsync(dto, jwtToken, jwtSecret);
                return result.Status switch
                {
                    OperationStatus.Success => Results.Ok(result),
                    OperationStatus.Unauthorized => Results.Unauthorized(),
                    OperationStatus.ValidationError => Results.BadRequest(result),
                    OperationStatus.Conflict => Results.Conflict(result),
                    _ => Results.Problem(detail: result.Message, statusCode: 500)
                };
            })
            .WithName("RegisterPost")
            .WithSummary("Register a new post")
            .WithDescription("Creates a new post with the provided information")
            .Produces<Result<PostDto>>(201, "application/json")  // Created
            .Produces<Result<PostDto>>(401, "application/json")  // Unauthorized
            .Produces<Result<PostDto>>(400, "application/json")  // Bad Request (validação)
            .Produces<Result<PostDto>>(409, "application/json")  // Conflict (já existe)
            .ProducesProblem(500);
            #endregion

            #region Get Post
            feedEndpoint.MapGet("getposts", async (HttpContext httpContext, FeedService feedService, IConfiguration config, ErrorLogService errorLogService) =>
            {
                if (!TryGetJwt(httpContext, config, out var jwtToken, out var jwtSecret))
                {
                    return Results.Unauthorized();
                }
                var result = await feedService.GetPostsAsync(jwtToken, jwtSecret);
                return result.Status switch
                {
                    OperationStatus.Success => Results.Ok(result),
                    OperationStatus.ValidationError => Results.BadRequest(result),
                    OperationStatus.Unauthorized => Results.Unauthorized(),
                    OperationStatus.Conflict => Results.Conflict(result),
                    _ => Results.Problem(detail: result.Message, statusCode: 500, title: "Erro interno do servidor")
                };
            }).WithName("Get Posts")
            .WithSummary("Obter posts do feed")
            .WithDescription("Obtém todos os posts do feed do usuário")
            .Produces<Result<List<PostDto>>>(200, "application/json")  // OK
            .Produces<Result<List<PostDto>>>(401, "application/json")  // Unauthorized
            .Produces<Result<List<PostDto>>>(400, "application/json")  // Bad Request (validação)
            .Produces<Result<List<PostDto>>>(409, "application/json")  // Conflict (já existe)
            .ProducesProblem(500);
            #endregion

            #region Like Post
            feedEndpoint.MapPost("likepost", async (HttpContext httpContext, PostDto dto, FeedService feedService, IConfiguration config) =>
            {
                if (!TryGetJwt(httpContext, config, out var jwtToken, out var jwtSecret))
                {
                    return Results.Unauthorized();
                }
                var result = await feedService.LikePostAsync(dto, jwtToken, jwtSecret);
                return result.Status switch
                {
                    OperationStatus.Success => Results.Ok(result),
                    OperationStatus.Unauthorized => Results.Unauthorized(),
                    OperationStatus.ValidationError => Results.BadRequest(result),
                    OperationStatus.Conflict => Results.Conflict(result),
                    _ => Results.Problem(detail: result.Message, statusCode: 500, title: "Erro interno do servidor")
                };
            })
            .WithName("likepost")
            .WithSummary("Get like status of a post")
            .WithDescription("Get like status of a post with the provided information")
            .Produces<Result<string>>(200, "application/json")  // OK
            .Produces<Result<string>>(401, "application/json")  // Unauthorized
            .Produces<Result<string>>(400, "application/json")  // Bad Request (validação)
            .Produces<Result<string>>(409, "application/json")  // Conflict (já existe)
            .ProducesProblem(500);
            #endregion

            #region Get Posts
            feedEndpoint.MapPost("getpostliked", async (HttpContext httpContext, PostDto dto, FeedService feedService, IConfiguration config) =>
            {
                if (!TryGetJwt(httpContext, config, out var jwtToken, out var jwtSecret))
                {
                    return Results.Unauthorized();
                }
                var result = await feedService.GetPostLikeAsync(dto, jwtToken, jwtSecret);
                return result.Status switch
                {
                    OperationStatus.Success => Results.Ok(result),
                    OperationStatus.Unauthorized => Results.Unauthorized(),
                    OperationStatus.ValidationError => Results.BadRequest(result),
                    OperationStatus.Conflict => Results.Conflict(result),
                    _ => Results.Problem(detail: result.Message, statusCode: 500, title: "Erro interno do servidor")
                };
            })
            .WithName("getLikedPost")
            .WithSummary("Get Liked Post")
            .WithDescription("Get Liked Post with the provided information")
            .Produces<Result<bool>>(200, "application/json")  // OK
            .Produces<Result<bool>>(401, "application/json")  // Unauthorized
            .Produces<Result<bool>>(400, "application/json")  // Bad Request (validação)
            .Produces<Result<bool>>(409, "application/json")  // Conflict (já existe)
            .ProducesProblem(500);
            #endregion
        }
    }
}