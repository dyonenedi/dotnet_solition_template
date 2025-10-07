using app.shared.Libs.DTOs.Feed;
using app.shared.Libs.Responses;
using app.api.Application.Repositories;
using app.api.Application.Models;

namespace app.api.Application.Services
{
    public class FeedService
    {
        private readonly IFeedRepository _feedRepository;

        public FeedService(IFeedRepository feedRepository)
        {
            _feedRepository = feedRepository;
        }

        public async Task<SimpleResponse> PostAsync(PostDto dto, string jwtToken, string jwtSecret)
        {
            if (dto != null && dto.Text.Length > 0 && dto.Text.Length <= 200)
            {
                var principal = Utils.JwtUtils.ValidateToken(jwtToken, jwtSecret);
                if (principal == null)
                    return SimpleResponse.CreateError("Token inválido", OperationStatus.ValidationError);
                var userIdStr = Utils.JwtUtils.GetUserId(principal);
                if (!int.TryParse(userIdStr, out int userId))
                    return SimpleResponse.CreateError("ID do usuário inválido", OperationStatus.ValidationError);
                dto.UserId = userId;
                dto.Text = dto.Text.Trim();
                return await _feedRepository.AddAsync(dto);
            }
            return SimpleResponse.CreateError("Erro ao salvar post", OperationStatus.ValidationError);
        }

        public async Task<Response<List<PostDto>>> GetPostsAsync(string jwtToken, string jwtSecret)
        {
            var principal = Utils.JwtUtils.ValidateToken(jwtToken, jwtSecret);
            if (principal == null)
                return Response<List<PostDto>>.CreateError("Token inválido", new List<PostDto>(), OperationStatus.ValidationError);
            var userIdStr = Utils.JwtUtils.GetUserId(principal);
            if (!int.TryParse(userIdStr, out int userId))
                return Response<List<PostDto>>.CreateError("ID do usuário inválido", new List<PostDto>(), OperationStatus.ValidationError);
            var posts = await _feedRepository.GetAsync(userId);
            if (posts != null && posts.Count > 0)
            {
                return Response<List<PostDto>>.CreateSuccess(posts, "Posts obtidos com sucesso");
            }
            else
            {
                return Response<List<PostDto>>.CreateError("Nenhum post encontrado", new List<PostDto>(), OperationStatus.NotFound);
            }
        }
    }
}