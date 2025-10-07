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
                    Text = dto.Text,
                    UserId = dto.UserId,
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
        
        public async Task<List<PostDto>> GetAsync(int UserId)
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
    }
}