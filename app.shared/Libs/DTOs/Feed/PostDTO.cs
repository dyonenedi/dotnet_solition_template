namespace app.shared.Libs.DTOs.Feed
{
    public class PostDto
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string Text { get; set; } = null!;
        public DateTime InsertDate { get; set; }
    }
}