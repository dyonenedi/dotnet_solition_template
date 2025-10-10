namespace app.shared.Libs.DTOs.Feed
{
    public class PostDto
    {
        public int? Id { get; set; } = null;
        public int? UserId { get; set; } = null;
        public string? UserName { get; set; } = null;
        public string? Text { get; set; } = null;
        public DateTime? InsertDate { get; set; } = null;
    }
}