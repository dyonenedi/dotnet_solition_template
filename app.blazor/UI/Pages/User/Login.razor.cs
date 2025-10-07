using app.blazor.Handlers;
using app.blazor.UI.ViewModels.User;
using app.shared.Libs.Responses;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Microsoft.AspNetCore.Components.Authorization;

namespace app.blazor.UI.Pages.User;

public partial class Login
{
    [Inject] private AuthHandler AuthHandler { get; set; } = null!;
    [Inject] private NavigationManager navigationManager { get; set; } = null!;
    [CascadingParameter] 
    private Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

    private LoginViewModel model = new();
    private bool isLoading = false;
    private string message = string.Empty;
    private Severity messageType = default;
    private SimpleResponse? response;

    protected override async Task OnInitializedAsync()
    {
       var authState = await AuthenticationStateTask;
        var user = authState.User;

        if (user.Identity is { IsAuthenticated: true })
        {
            navigationManager.NavigateTo("/feed");
        }
    }

    private async Task Submit()
    {
        isLoading = true;
        message = string.Empty;
        messageType = Severity.Error;

        try
        {
            response = await AuthHandler.LoginAsyc(model);
            if (!response.Success)
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
