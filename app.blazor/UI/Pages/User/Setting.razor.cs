using Microsoft.AspNetCore.Components;
using app.shared.Libs.DTOs.User;
using MudBlazor;
using app.blazor.Handlers;

namespace app.blazor.UI.Pages.User
{
    public partial class Setting : ComponentBase
    {

        [Inject] public ISnackbar Snackbar { get; set; } = default!;
        [Inject] public ProfileHandler _profileHandler { get; set; } = default!;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        private RegisterDTO UserSettings = new RegisterDTO();
        private MudForm? form;

        protected override async Task OnInitializedAsync()
        {
            var response = await _profileHandler.getUserSettings();
            if (response == null)
            {
                Snackbar.Add("Erro ao carregar configurações do usuário.", Severity.Error);
                return;
            }
            if (response.Status != shared.Libs.Responses.OperationStatus.Success || response.Data == null)
            {
                Snackbar.Add(response.Message ?? "Erro ao carregar configurações do usuário.", Severity.Error);
                return;
            }
            UserSettings = new RegisterDTO
            {
                FullName = response.Data.FullName,
                Username = response.Data.Username,
                Email = response.Data.Email
            };
        }

        private async Task SaveUserSettings()
        {
            Snackbar.Add("Configurações salvas com sucesso!", Severity.Success);
        }
    }
}
