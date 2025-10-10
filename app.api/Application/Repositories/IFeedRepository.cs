using app.shared.Libs.DTOs.Feed;
using app.shared.Libs.Responses;

namespace app.api.Application.Repositories
{
    public interface IFeedRepository
    {
        Task<SimpleResponse> AddAsync(PostDto dto);
        Task<List<PostDto>> GetAsync(int? UserId);
        Task<SimpleResponse> LikePostAsync(PostDto dto);
        Task<PostLikeDto> GetPostLikeAsync(int? postId, int userId);
    }
}