using app.blazor.Handlers;
using app.blazor.UI.ViewModels.User;
using app.shared.Libs.Responses;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace app.blazor.UI.Pages.User;

public partial class Register
{
    [Inject] private AuthHandler AuthHandler { get; set; } = null!;
    [Inject] private NavigationManager navigationManager { get; set; } = null!;
    private RegisterViewModel model = new();
    private bool isLoading = false;
    private string message = string.Empty;
    private Severity messageType = default;
    private SimpleResponse? response;

    private async Task Submit()
    {
        isLoading = true;
        message = string.Empty;
        messageType = Severity.Error;

        try
        {
            response = await AuthHandler.RegisterAsync(model);
            if (response.Success)
            {
                message = "Cadastro realizado com sucesso!";
                messageType = Severity.Success;
                model = new();
                navigationManager.NavigateTo("/user/login");
            }
            else
            {
                message = response.Message;
            }
        }
        catch (Exception ex)
        {
            message = $"Erro interno: {ex.Message}";
        }
        finally
        {
            isLoading = false;
        }
    }
}
