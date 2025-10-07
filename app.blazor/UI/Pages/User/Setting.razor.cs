using Microsoft.AspNetCore.Components;
using app.shared.Libs.DTOs.User;
using MudBlazor;

namespace app.blazor.UI.Pages.User
{
    public partial class Setting : ComponentBase
    {

        [Inject] public ISnackbar Snackbar { get; set; } = default!;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        private RegisterDTO UserSettings = new RegisterDTO();
        private MudForm? form;
        private bool isLoading = false;

        private async Task SaveUserSettings()
        {
            isLoading = true;
            // Simulação de chamada de API
            await Task.Delay(1000);
            Snackbar.Add("Configurações salvas com sucesso!", Severity.Success);
            isLoading = false;
        }
    }
}
