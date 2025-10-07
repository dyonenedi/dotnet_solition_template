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

            #region Post Feed
            feedEndpoint.MapPost("post", async (HttpContext httpContext, PostDto dto, FeedService feedService, IConfiguration config) =>
            {
                var jwtToken = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var jwtSecret = config["Jwt:Secret"] ?? string.Empty;
                var response = await feedService.PostAsync(dto, jwtToken, jwtSecret);
                return response.Status switch
                {
                    OperationStatus.Success => Results.Ok(response),
                    OperationStatus.ValidationError => Results.BadRequest(response),
                    OperationStatus.Conflict => Results.Conflict(response),
                    _ => Results.Problem(detail: response.Message, statusCode: 500, title: "Erro interno do servidor")
                };
            })
            .WithName("RegisterPost")
            .WithSummary("Register a new post")
            .WithDescription("Creates a new post with the provided information")
            .Produces<SimpleResponse>(201, "application/json")  // Created
            .Produces<SimpleResponse>(400, "application/json")  // Bad Request (validação)
            .Produces<SimpleResponse>(409, "application/json")  // Conflict (já existe)
            .ProducesProblem(500);
            #endregion

            #region Get Post Feeds
            feedEndpoint.MapGet("getposts", async (HttpContext httpContext, FeedService feedService, IConfiguration config) =>
            {
                var jwtToken = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var jwtSecret = config["Jwt:Secret"] ?? string.Empty;
                var response = await feedService.GetPostsAsync(jwtToken, jwtSecret);
                return response.Status switch
                {
                    OperationStatus.Success => Results.Ok(response),
                    OperationStatus.ValidationError => Results.BadRequest(response),
                    OperationStatus.Conflict => Results.Conflict(response),
                    _ => Results.Problem(detail: response.Message, statusCode: 500, title: "Erro interno do servidor")
                };
            }).WithName("Get Posts")
            .WithSummary("Obter posts do feed")
            .WithDescription("Obtém todos os posts do feed do usuário")
            .Produces<Response<List<PostDto>>>(200, "application/json")  // OK
            .Produces<Response<List<PostDto>>>(400, "application/json")  // Bad Request (validação)
            .Produces<Response<List<PostDto>>>(409, "application/json")  // Conflict (já existe)
            .ProducesProblem(500);
            #endregion 
            
        }
    }
}