using app.shared.Libs.DTOs.Feed;
using app.shared.Libs.Responses;
using app.api.Application.Repositories;

namespace app.api.Application.Services
{
    public class FeedService
    {
        private readonly IFeedRepository _feedRepository;

        public FeedService(IFeedRepository feedRepository)
        {
            _feedRepository = feedRepository;
        }
        public async Task<SimpleResponse> PostAsync(PostDto dto)
        {
            if (dto != null && dto.Text.Length > 0 && dto.Text.Length <= 200)
            {
                dto.Text = dto.Text.Trim();
                dto.UserId = 1; // Temporário, depois pegar do token
                return await _feedRepository.AddAsync(dto);
            }
            return SimpleResponse.CreateError("Erro ao salvar post", OperationStatus.ValidationError);
        }

        public async Task<Response<List<PostDto>>> GetPostsAsync()
        {
            var userId = 1; // ID Manually puttet 
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