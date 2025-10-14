using app.shared.Libs.DTOs.Feed;
using app.shared.Libs.Responses;
using app.api.Application.Repositories;
using System.Threading.Tasks.Dataflow;
using app.api.Application.Models;

namespace app.api.Application.Services
{
    public class FeedService
    {
        private readonly FeedRepository _feedRepository;
        private readonly ErrorLogService _errorLogService;

        public FeedService(FeedRepository feedRepository, ErrorLogService errorLogService)
        {
            _feedRepository = feedRepository;
            _errorLogService = errorLogService;
        }
        public async Task<Result<PostDto>> PostAsync(PostDto dto, string jwtToken, string jwtSecret)
        {
            if (string.IsNullOrWhiteSpace(dto?.Text) || dto.Text.Length > 200)
            return Result<PostDto>.Fail("Texto inválido", OperationStatus.ValidationError);

            var (responseMessage, userId) = await JtwValidateToken(jwtToken, jwtSecret);
            if (responseMessage != null)
                return Result<PostDto>.Fail(responseMessage);

            dto.UserId = userId;
            dto.Text = dto.Text.Trim();

            try
            {
                var postDto = await _feedRepository.AddAsync(dto);
                return Result<PostDto>.Ok(postDto, "Post criado com sucesso");
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(0, nameof(PostAsync), ex);
                return Result<PostDto>.Fail("Erro interno ao salvar post");
            }
        }
        public async Task<Result<List<PostDto>>> GetPostsAsync(string jwtToken, string jwtSecret)
        {
            var (responseMessage, userId) = await JtwValidateToken(jwtToken, jwtSecret);
            if (responseMessage != null)
                return Result<List<PostDto>>.Fail(responseMessage);

            try
            {
                var posts = await _feedRepository.GetAsync(userId);
                return Result<List<PostDto>>.Ok(posts, posts.Count > 0 ? "Posts obtidos com sucesso" : "Nenhum post encontrado");
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(0, nameof(GetPostsAsync), ex);
                return Result<List<PostDto>>.Fail("Erro interno ao obter posts", OperationStatus.Error);
            }
        }
        public async Task<Result<string>> LikePostAsync(PostDto dto, string jwtToken, string jwtSecret)
        {
            var (responseMessage, userId) = await JtwValidateToken(jwtToken, jwtSecret);
            if (responseMessage != null)
                return Result<string>.Fail(responseMessage, OperationStatus.ValidationError);

            dto.UserId = userId;

            try
            {
                var repoResponse = await _feedRepository.LikePostAsync(dto);
                if (repoResponse)
                    return Result<string>.Ok(string.Empty);
                else
                    return Result<string>.Fail("Erro ao salvar like", OperationStatus.Error);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(userId, nameof(LikePostAsync), ex);
                return Result<string>.Fail("Erro interno ao curtir post", OperationStatus.Error);
            }
        }
        public async Task<Result<bool>> GetPostLikeAsync(PostDto dto, string jwtToken, string jwtSecret)
        {
            var (responseMessage, userId) = await JtwValidateToken(jwtToken, jwtSecret);
            if (responseMessage != null)
                return Result<bool>.Fail(responseMessage);

            dto.UserId = userId;
            try
            {
                var postLike = await _feedRepository.GetPostLikeAsync(dto.Id, userId);
                if (postLike != null && postLike.IsLiked == true)
                    return Result<bool>.Ok(true, "Status de curtida obtido com sucesso");
                
                return Result<bool>.Ok(false, "Status de curtida obtido com sucesso");
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(0, nameof(GetPostLikeAsync), ex);
                return Result<bool>.Fail("Erro interno ao obter status de curtida", OperationStatus.Error);
            }
        }
        private async Task<(string? responseMessage, int userId)> JtwValidateToken(string jwtToken, string jwtSecret)
        {
            int userId = 0;
            var principal = await Utils.JwtUtils.ValidateToken(jwtToken, jwtSecret);
            if (principal == null)
                return ("Token inválido", default);

            var userIdStr = Utils.JwtUtils.GetUserId(principal);
            if (!int.TryParse(userIdStr, out userId))
                return ("ID do usuário inválido", default);

            return (null, userId);
        }
    }
}