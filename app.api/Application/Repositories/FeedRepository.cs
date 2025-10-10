using app.api.Application.DB;
using app.api.Application.Models;
using app.shared.Libs.DTOs.Feed;
using app.shared.Libs.Responses;
using Microsoft.EntityFrameworkCore;

namespace app.api.Application.Repositories
{
    public class FeedRepository : IFeedRepository
    {
        private readonly AppDbContext _db;
        public FeedRepository(AppDbContext dbContext)
        {
            _db = dbContext;
        }
        public async Task<SimpleResponse> AddAsync(PostDto dto)
        {
            if (dto != null)
            {
                await _db.Posts.AddAsync(new Post
                {
                    Text = dto.Text ?? string.Empty,
                    UserId = (int)dto.UserId!,
                    InsertDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                });

                if (await _db.SaveChangesAsync() > 0)
                {
                    return SimpleResponse.CreateSuccess("Post criado com sucesso");
                }
                return SimpleResponse.CreateError("Erro ao salvar post", OperationStatus.Error);
            }
            return SimpleResponse.CreateError("Erro ao validar dados do post", OperationStatus.ValidationError);
        }
        public async Task<List<PostDto>> GetAsync(int? UserId)
        {
            var result = await _db.Posts
                .Where(p => p.UserId == UserId)
                .Join(_db.Users, post => post.UserId, user => user.Id, (post, user) => new { post, user })
                .OrderByDescending(db => db.post.Id)
                .ToListAsync();

            if (result != null && result.Count > 0)
            {
                return result.Select(db => new PostDto
                {
                    Id = db.post.Id,
                    UserId = db.post.UserId,
                    UserName = db.user.FullName,
                    Text = db.post.Text,
                    InsertDate = db.post.InsertDate
                }).ToList();
            }
            else
            {
                return new List<PostDto>();
            }
        }
        public async Task<SimpleResponse> LikePostAsync(PostDto dto)
        {
            if (dto == null || dto.Id == null || dto.Id <= 0 || dto.UserId == null || dto.UserId <= 0)
                return SimpleResponse.CreateError("Dados inválidos", OperationStatus.ValidationError);
            var post = await _db.Posts.FindAsync(dto.Id);
            if (post == null)
                return SimpleResponse.CreateError("Post não encontrado", OperationStatus.NotFound);

            var postLike = await _db.PostLikes.FirstOrDefaultAsync(pl => pl.PostId == dto.Id && pl.LikerId == dto.UserId);
            if (postLike != null)
            {
                postLike.Active = !postLike.Active;
                postLike.UpdateDate = DateTime.Now;
                _db.PostLikes.Update(postLike);
                if (await _db.SaveChangesAsync() > 0)
                {
                    return SimpleResponse.CreateSuccess("Post descurtido com sucesso");
                }
            }
            else
            {
                post.PostLikes.Add(new PostLike
                {
                    LikerId = (int)dto.UserId,
                    PostId = (int)dto.Id,
                    InsertDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Active = true
                });
                if (await _db.SaveChangesAsync() > 0)
                {
                    return SimpleResponse.CreateSuccess("Post curtido com sucesso");
                }
            }

            return SimpleResponse.CreateError("Erro ao curtir post", OperationStatus.Error);
        }
        public async Task<PostLikeDto> GetPostLikeAsync(int? postId, int userId)
        {
            var postLike = await _db.PostLikes.FirstOrDefaultAsync(pl => pl.PostId == postId && pl.LikerId == userId);
            if (postLike != null)
            {
                return new PostLikeDto
                {
                    PostId = postLike.PostId,
                    UserId = postLike.LikerId,
                    IsLiked = postLike.Active ?? false
                };
            }
            return new PostLikeDto
            {
                PostId = (int)postId!,
                UserId = userId,
                IsLiked = false
            };
        }
    }
}