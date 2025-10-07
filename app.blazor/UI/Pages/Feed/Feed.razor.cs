using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using app.blazor.UI.ViewModels.Feed;
using app.blazor.Handlers;
using MudBlazor;
using app.shared.Libs.DTOs.Feed;

namespace app.blazor.UI.Pages.Feed
{
    partial class Feed
    {
        [Inject] private NavigationManager Navigation { get; set; } = null!;
        [Inject] private ISnackbar Snackbar { get; set; } = null!;
        [Inject] private CookieAuthenticationStateProvider AuthProvider { get; set; } = null!;
        [Inject] private FeedHandler _FeedHandler { get; set; } = null!;
        public List<PostDto> Posts { get; set; } = new List<PostDto>();
        private bool isLoading { get; set; } = false;

        private FeedViewModel model { get; set; } = new FeedViewModel();

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (!user.Identity?.IsAuthenticated ?? true)
            {
                Navigation.NavigateTo("/user/login", true);
            }

            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomStart;

            await GetPosts();
        }

        public async Task GetPosts()
        {
            var response = await _FeedHandler.GetPostsAsync();
            if (response != null && response.Success && response.Data != null)
            {
                Posts = response.Data;
            }
            else
            {
                Snackbar.Add("Erro ao carregar posts do feed. Tente novamente mais tarde.", Severity.Error);
            }
        }

        private async Task Submit()
        {
            isLoading = true;
            if (!string.IsNullOrEmpty(model.text) && model.text.Length >= 5 && model.text.Length <= 200)
            {
                var response = await _FeedHandler.PostAsync(model.text);
                if (response == null || !response.Success)
                {
                    Snackbar.Add("Erro ao postar no feed. Tente novamente mais tarde.", Severity.Error);
                }
                else
                {
                    await GetPosts();
                }
                model.text = string.Empty;
            }
            else
            {
                Snackbar.Add("O texto deve ter entre 5 e 200 caracteres.", Severity.Warning);
            }
            isLoading = false;
        }
    }
}