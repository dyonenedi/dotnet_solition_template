using app.api.Application.Services;
using app.shared.Libs.DTOs.Feed;
using app.shared.Libs.Responses;

namespace app.api.Application.Endpoints
{
    public static class FeedEndpoint
    {
        public static void MapFeedEndpoints(this WebApplication app)
        {
            var feedEndpoint = app.MapGroup("v1/feed")
                .WithTags("Feed")
                .WithDescription("Feed management endpoints");

            #region Post Post
            feedEndpoint.MapPost("post", async (HttpContext httpContext, PostDto dto, FeedService feedService, IConfiguration config) =>
            {
                var jwtToken = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var jwtSecret = config["Secret"] ?? string.Empty;
                var response = await feedService.PostAsync(dto, jwtToken, jwtSecret);
                return response.Status switch
                {
                    OperationStatus.Success => Results.Ok(response),
                    OperationStatus.Unauthorized => Results.Unauthorized(),
                    OperationStatus.ValidationError => Results.BadRequest(response),
                    OperationStatus.Conflict => Results.Conflict(response),
                    _ => Results.Problem(detail: response.Message, statusCode: 500, title: "Erro interno do servidor")
                };
            })
            .WithName("RegisterPost")
            .WithSummary("Register a new post")
            .WithDescription("Creates a new post with the provided information")
            .Produces<SimpleResponse>(201, "application/json")  // Created
            .Produces<SimpleResponse>(401, "application/json")  // Unauthorized
            .Produces<SimpleResponse>(400, "application/json")  // Bad Request (validação)
            .Produces<SimpleResponse>(409, "application/json")  // Conflict (já existe)
            .ProducesProblem(500);
            #endregion

            #region Get Post
            feedEndpoint.MapGet("getposts", async (HttpContext httpContext, FeedService feedService, IConfiguration config, ErrorLogService errorLogService) =>
            {
                try
                {
                    var jwtToken = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                    var jwtSecret = config["Secret"];
                    if (string.IsNullOrEmpty(jwtToken) || string.IsNullOrEmpty(jwtSecret))
                    {
                        return Results.Unauthorized();
                    }
                    var response = await feedService.GetPostsAsync(jwtToken, jwtSecret);
                    return response.Status switch
                    {
                        OperationStatus.Success => Results.Ok(response),
                        OperationStatus.ValidationError => Results.BadRequest(response),
                        OperationStatus.Unauthorized => Results.Unauthorized(),
                        OperationStatus.Conflict => Results.Conflict(response),
                        _ => Results.Problem(detail: response.Message, statusCode: 500, title: "Erro interno do servidor")
                    };
                }
                catch (Exception ex)
                {
                    // Log de erro específico do endpoint
                    await errorLogService.LogErrorAsync(0, nameof(FeedEndpoint), ex);
                    return Results.Problem(detail: "Erro interno do servidor", statusCode: 500, title: "Erro interno do servidor");
                }
            }).WithName("Get Posts")
            .WithSummary("Obter posts do feed")
            .WithDescription("Obtém todos os posts do feed do usuário")
            .Produces<Response<List<PostDto>>>(200, "application/json")  // OK
            .Produces<Response<List<PostDto>>>(401, "application/json")  // Unauthorized
            .Produces<Response<List<PostDto>>>(400, "application/json")  // Bad Request (validação)
            .Produces<Response<List<PostDto>>>(409, "application/json")  // Conflict (já existe)
            .ProducesProblem(500);
            #endregion

            #region Like Post
            feedEndpoint.MapPost("likepost", async (HttpContext httpContext, PostDto dto, FeedService feedService, IConfiguration config) =>
            {
                var jwtToken = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var jwtSecret = config["Secret"] ?? string.Empty;
                var response = await feedService.LikePostAsync(dto, jwtToken, jwtSecret);
                return response.Status switch
                {
                    OperationStatus.Success => Results.Ok(response),
                    OperationStatus.Unauthorized => Results.Unauthorized(),
                    OperationStatus.ValidationError => Results.BadRequest(response),
                    OperationStatus.Conflict => Results.Conflict(response),
                    _ => Results.Problem(detail: response.Message, statusCode: 500, title: "Erro interno do servidor")
                };
            })
            .WithName("likePost")
            .WithSummary("Like a post")
            .WithDescription("Like a post with the provided information")
            .Produces<SimpleResponse>(201, "application/json")  // Created
            .Produces<SimpleResponse>(401, "application/json")  // Unauthorized
            .Produces<SimpleResponse>(400, "application/json")  // Bad Request (validação)
            .Produces<SimpleResponse>(409, "application/json")  // Conflict (já existe)
            .ProducesProblem(500);
            #endregion
            
            #region Get Liked Post
            feedEndpoint.MapPost("getpostliked", async (
                HttpContext httpContext,
                PostDto dto,
                FeedService feedService,
                IConfiguration config) =>
            {
                var jwtToken = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var jwtSecret = config["Secret"] ?? string.Empty;
                if (dto == null || dto.Id <= 0 || dto.UserId <= 0)
                {
                    return Results.BadRequest("Dados inválidos");
                }
                
                var response = await feedService.GetPostLikeAsync(dto, jwtToken, jwtSecret);
                return response.Status switch
                {
                    OperationStatus.Success => Results.Ok(response),
                    OperationStatus.Unauthorized => Results.Unauthorized(),
                    OperationStatus.ValidationError => Results.BadRequest(response),
                    OperationStatus.Conflict => Results.Conflict(response),
                    _ => Results.Problem(detail: response.Message, statusCode: 500, title: "Erro interno do servidor")
                };
            })
            .WithName("getPostLike")
            .WithSummary("Get like status of a post")
            .WithDescription("Get like status of a post with the provided information")
            .Produces<Response<PostLikeDto>>(200, "application/json")  // OK
            .Produces<Response<PostLikeDto>>(401, "application/json")  // Unauthorized
            .Produces<Response<PostLikeDto>>(400, "application/json")  // Bad Request (validação)
            .Produces<Response<PostLikeDto>>(409, "application/json")  // Conflict (já existe)
            .ProducesProblem(500);
            #endregion
        }
    }
}