using Microsoft.AspNetCore.Components;
using app.shared.Libs.DTOs.Feed;

namespace app.blazor.UI.Pages.Feed.Components
{
    public partial class FeedList
    {
        [Parameter] public PostDto Post { get; set; } = null!;
    }
}