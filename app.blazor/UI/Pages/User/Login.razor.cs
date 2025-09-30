using app.blazor.UI.Handlers;
using app.blazor.UI.ViewModels;
using app.shared.Libs.Responses;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace app.blazor.UI.Pages.User;

public partial class Login
{
    [Inject] private RegisterHandler registerHandler { get; set; } = null!;
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
            response = await registerHandler.SubmitAsync(model);
            if (response.Success)
            {
                navigationManager.NavigateTo("/feed");
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
