using app.shared.Libs.DTOs.Feed;
using app.shared.Libs.Responses;
using app.api.Application.Repositories;

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
        public async Task<Result<PostDto>> PostAsync(PostDto dto)
        {
            dto.UserId = dto.UserId;
            dto.Text = dto.Text.Trim() ?? string.Empty;

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
        public async Task<Result<List<PostDto>>> GetPostsAsync(int userId)
        {
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
        public async Task<Result<string>> LikePostAsync(PostDto dto)
        {
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
                await _errorLogService.LogErrorAsync(0, nameof(LikePostAsync), ex);
                return Result<string>.Fail("Erro interno ao curtir post", OperationStatus.Error);
            }
        }
        public async Task<Result<bool>> GetPostLikeAsync(PostDto dto)
        {
            try
            {
                var postLike = await _feedRepository.GetPostLikeAsync(dto.Id, dto.UserId);
                if (postLike != null && postLike.IsLiked == true)
                    return Result<bool>.Ok(true, "Status de curtida obtido com sucesso");
                
                return Result<bool>.Ok(false, "Status de curtida obtido com sucesso");
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(0, nameof(GetPostLikeAsync), ex);
                return Result<bool>.Fail("Erro interno ao obter status de curtida", OperationStatus.Error, false);
            }
        }
    }
}