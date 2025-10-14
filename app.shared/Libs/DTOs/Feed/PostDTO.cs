namespace app.shared.Libs.DTOs.Feed
{
    public class PostDto
    {
        public int Id { get; set; } = default;
        public int UserId { get; set; } = default;
        public string? UserName { get; set; } = null;
        public string Text { get; set; } = string.Empty;
        public DateTime? InsertDate { get; set; } = null;
    }
}