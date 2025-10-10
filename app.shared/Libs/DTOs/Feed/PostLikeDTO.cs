namespace app.shared.Libs.DTOs.Feed
{
    public class PostLikeDto
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public bool IsLiked { get; set; }
    }
}