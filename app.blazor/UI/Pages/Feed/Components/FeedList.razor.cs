using Microsoft.AspNetCore.Components;
using app.shared.Libs.DTOs.Feed;
using app.blazor.Handlers;
using MudBlazor;

namespace app.blazor.UI.Pages.Feed.Components
{
    public partial class FeedList
    {
        [Parameter] public PostDto Post { get; set; } = null!;
        [Inject] public FeedHandler FeedHandler { get; set; } = null!;
        [Inject] public ISnackbar Snackbar { get; set; } = null!;
        private bool isLiked = false;

        protected override async Task OnInitializedAsync()
        {
            if (Post == null || Post.Id == null || Post.Id <= 0)
                return;
            var response = await FeedHandler.getLiked((int)Post.Id);
            if (response == null || !response.Success || response.Data == null || response.Data.PostId != Post.Id)
            {
                Snackbar.Add(response?.Message ?? "Erro ao pegar likes do post", Severity.Error);
            } else
            {
                isLiked = response.Data.IsLiked;
            }
        }
        public async Task LikePost()
        {
            if (Post != null && Post.Id != null && Post.Id > 0)
            {
                isLiked = !isLiked;
                var response = await FeedHandler.LikePost(Post.Id);
                if (response == null || !response.Success)
                {
                    isLiked = !isLiked;
                    Snackbar.Add(response?.Message ?? "Erro ao curtir post", Severity.Error);
                }
            }
        }
    }
}