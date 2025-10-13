using app.shared.Libs.DTOs.User;
using app.shared.Libs.Responses;

namespace app.blazor.Handlers;

public class ProfileHandler
{
    private readonly HttpClient _authClient;
    private readonly CookieAuthenticationStateProvider _authStateProvider;

    public ProfileHandler(IHttpClientFactory HttpClientFactory, CookieAuthenticationStateProvider authStateProvider)
    {
        _authClient = HttpClientFactory.CreateClient("AUTH");
        _authStateProvider = authStateProvider;

        var jwtToken = _authStateProvider.GetJwtToken();
        if (string.IsNullOrWhiteSpace(jwtToken))
            return;
        _authClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

    }
    public async Task<Response<RegisterDTO>> getUserSettings()
    {
        try
        {
            var response = await _authClient.GetFromJsonAsync<Response<RegisterDTO>>("v1/user/getUserData");
            return response ?? new Response<RegisterDTO>();
        }
        catch (HttpRequestException ex)
        {
            return new Response<RegisterDTO>
            {
                Status = OperationStatus.ValidationError, // Use an appropriate error status
                Message = $"Erro ao buscar dados do usuário: {ex.Message}",
                Data = null
            };
        }
        catch (Exception ex)
        {
            return new Response<RegisterDTO>
            {
                Status = OperationStatus.ValidationError, // Use an appropriate error status
                Message = $"Erro inesperado: {ex.Message}",
                Data = null
            };
        }
    }
}
