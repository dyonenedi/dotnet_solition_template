using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using app.blazor.UI.ViewModels.Feed;
using MudBlazor;

namespace app.blazor.UI.Pages.Feed
{
    partial class Feed
    {
        [Inject] private NavigationManager Navigation { get; set; } = null!;
        [Inject] private CookieAuthenticationStateProvider AuthProvider { get; set; } = null!;
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
        }

        private async Task Submit()
        {
            isLoading = true;
            if (!string.IsNullOrEmpty(model.text) && model.text.Length >= 5 && model.text.Length <= 200)
            {
                model.text = string.Empty;
                // Lógica para enviar o post
                await Task.Delay(2000); // Simula um atraso para o envio
            }
            else
            {
                // Lógica para exibir mensagem de erro
            }
            isLoading = false;
        }
    }
}