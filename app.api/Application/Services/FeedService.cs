using app.shared.Libs.DTOs.Feed;
using app.shared.Libs.Responses;
using app.api.Application.Repositories;
using System.Threading.Tasks.Dataflow;

namespace app.api.Application.Services
{
    public class FeedService
    {
        private readonly IFeedRepository _feedRepository;
        private readonly ErrorLogService _errorLogService;

        public FeedService(IFeedRepository feedRepository, ErrorLogService errorLogService)
        {
            _feedRepository = feedRepository;
            _errorLogService = errorLogService;
        }
        public async Task<SimpleResponse> PostAsync(PostDto dto, string jwtToken, string jwtSecret)
        {
            try
            {
                if (dto?.Text == null || dto.Text.Length == 0 || dto.Text.Length > 200)
                    return SimpleResponse.CreateError("Erro ao salvar post", OperationStatus.ValidationError);

                var principal = await Utils.JwtUtils.ValidateToken(jwtToken, jwtSecret);
                if (principal == null)
                    return SimpleResponse.CreateError("Token inválido", OperationStatus.Unauthorized);

                var userIdStr = Utils.JwtUtils.GetUserId(principal);
                if (!int.TryParse(userIdStr, out int userId))
                    return SimpleResponse.CreateError("ID do usuário inválido", OperationStatus.ValidationError);

                dto.UserId = userId;
                dto.Text = dto.Text.Trim();

                return await _feedRepository.AddAsync(dto);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(0, nameof(PostAsync), ex);
                return SimpleResponse.CreateError("Erro interno ao salvar post", OperationStatus.Error);
            }
        }
        public async Task<Response<List<PostDto>>> GetPostsAsync(string jwtToken, string jwtSecret)
        {

            var principal = await Utils.JwtUtils.ValidateToken(jwtToken, jwtSecret);
            if (principal == null)
                return Response<List<PostDto>>.CreateError("Token inválido", new List<PostDto>(), OperationStatus.Unauthorized);
            var userIdStr = Utils.JwtUtils.GetUserId(principal);
            if (!int.TryParse(userIdStr, out int userId))
            {
                Console.WriteLine("ID do usuário inválido");
                return Response<List<PostDto>>.CreateError("ID do usuário inválido", new List<PostDto>(), OperationStatus.ValidationError);
            }

            var posts = await _feedRepository.GetAsync(userId);
            if (posts == null)
                return Response<List<PostDto>>.CreateError("Erro ao obter posts", new List<PostDto>(), OperationStatus.Error);
            if (posts.Count > 0)
            {
                return Response<List<PostDto>>.CreateSuccess(posts, "Posts obtidos com sucesso");
            }
            else
            {
                return Response<List<PostDto>>.CreateSuccess(posts, "Nenhum post encontrado");
            }

        }
        public async Task<SimpleResponse> LikePostAsync(PostDto dto, string jwtToken, string jwtSecret)
        {
            try
            {
                if (dto?.Id == null || dto.Id <= 0)
                    return SimpleResponse.CreateError("Erro ao curtir post", OperationStatus.ValidationError);

                var principal = await Utils.JwtUtils.ValidateToken(jwtToken, jwtSecret);
                if (principal == null)
                    return SimpleResponse.CreateError("Token inválido", OperationStatus.Unauthorized);

                var userIdStr = Utils.JwtUtils.GetUserId(principal);
                if (!int.TryParse(userIdStr, out int userId))
                    return SimpleResponse.CreateError("ID do usuário inválido", OperationStatus.ValidationError);

                dto.UserId = userId;

                return await _feedRepository.LikePostAsync(dto);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(0, nameof(LikePostAsync), ex);
                return SimpleResponse.CreateError("Erro interno ao curtir post", OperationStatus.Error);
            }
        }
        public async Task<Response<PostLikeDto>> GetPostLikeAsync(PostDto dto, string jwtToken, string jwtSecret)
        {
            try
            {
                if (dto?.Id == null || dto.Id <= 0)
                    return Response<PostLikeDto>.CreateError("Erro ao obter status de curtida", null, OperationStatus.ValidationError);

                var principal = await Utils.JwtUtils.ValidateToken(jwtToken, jwtSecret);
                if (principal == null)
                    return Response<PostLikeDto>.CreateError("Token inválido", null, OperationStatus.Unauthorized);

                var userIdStr = Utils.JwtUtils.GetUserId(principal);
                if (!int.TryParse(userIdStr, out int userId))
                    return Response<PostLikeDto>.CreateError("ID do usuário inválido", null, OperationStatus.ValidationError);

                dto.UserId = userId;

                var postLike = await _feedRepository.GetPostLikeAsync(dto.Id, userId);
                if (postLike == null)
                    return Response<PostLikeDto>.CreateError("Erro ao obter status de curtida", null, OperationStatus.Error);

                return Response<PostLikeDto>.CreateSuccess(postLike, "Status de curtida obtido com sucesso");
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(0, nameof(GetPostLikeAsync), ex);
                return Response<PostLikeDto>.CreateError("Erro interno ao obter status de curtida", null, OperationStatus.Error);
            }
        }
    }
}