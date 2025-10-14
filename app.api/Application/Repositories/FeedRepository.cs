using app.api.Application.DB;
using app.api.Application.Models;
using app.shared.Libs.DTOs.Feed;
using app.shared.Libs.Responses;
using Microsoft.EntityFrameworkCore;

namespace app.api.Application.Repositories
{
    public class FeedRepository
    {
        private readonly AppDbContext _db;
        public FeedRepository(AppDbContext dbContext)
        {
            _db = dbContext;
        }
        public async Task<PostDto> AddAsync(PostDto dto)
        {
            var post = new Post();
            post.Text = dto.Text ?? string.Empty;
            post.UserId = (int)dto.UserId!;
            post.InsertDate = DateTime.Now;
            post.UpdateDate = DateTime.Now;

            await _db.Posts.AddAsync(post);
            await _db.SaveChangesAsync();
            return dto;
        }
        public async Task<List<PostDto>> GetAsync(int? UserId)
        {
            var result = await _db.Posts
                .Where(p => p.UserId == UserId)
                .Join(_db.Users, post => post.UserId, user => user.Id, (post, user) => new { post, user })
                .OrderByDescending(db => db.post.Id)
                .Select(db => new PostDto
                {
                    Id = db.post.Id,
                    UserId = db.post.UserId,
                    UserName = db.user.FullName,
                    Text = db.post.Text,
                    InsertDate = db.post.InsertDate
                })
                .ToListAsync();
            return result;
        }
        public async Task<bool> LikePostAsync(PostDto dto)
        {
            var post = await _db.Posts.FindAsync(dto.Id);
            if (post == null)
                return false;
            
            var postLike = await _db.PostLikes.FirstOrDefaultAsync(pl => pl.PostId == dto.Id && pl.LikerId == dto.UserId);
            if (postLike != null)
            {
                postLike.Active = !postLike.Active;
                postLike.UpdateDate = DateTime.Now;
                _db.PostLikes.Update(postLike);
            }
            else
            {
                post.PostLikes.Add(new PostLike
                {
                    LikerId = (int)dto.UserId!,
                    PostId = (int)dto.Id!,
                    InsertDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Active = true
                });
            }
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<PostLikeDto> GetPostLikeAsync(int postId, int userId)
        {
            var postLike = await _db.PostLikes.FirstOrDefaultAsync(pl => pl.PostId == postId && pl.LikerId == userId);
            return new PostLikeDto
            {
                PostId = postId,
                UserId = userId,
                IsLiked = postLike?.Active ?? false
            };
        }
    }
}