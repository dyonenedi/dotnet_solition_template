using app.shared.Libs.DTOs.User;
using app.blazor.UI.ViewModels.User;
using app.shared.Libs.Responses;
using System.Text.Json;
using Microsoft.AspNetCore.Components;

namespace app.blazor.UI.Handlers;

public class AuthHandler
{
    private readonly HttpClient _authHttp;
    private readonly HttpClient _apiHttp;
    private readonly HttpClient _blazorHttp;
    private readonly NavigationManager _nav;

    public AuthHandler(IHttpClientFactory httpClientFactory, NavigationManager NavigationManager)
    {
        _authHttp = httpClientFactory.CreateClient("AUTH");
        _apiHttp = httpClientFactory.CreateClient("API");
        _blazorHttp = httpClientFactory.CreateClient("BLAZOR");
        _nav = NavigationManager;
    }

    public async Task<SimpleResponse> RegisterAsync(RegisterViewModel user)
    {
        try
        {
            var dto = new RegisterDTO
            {
                FullName = user.FullName ?? "",
                Username = user.Username,
                Email = user.Email,
                Password = user.Password,
            };

            var response = await _authHttp.PostAsJsonAsync("v1/user/register", dto);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Sempre tenta deserializar a resposta da API
            var apiResponse = JsonSerializer.Deserialize<SimpleResponse>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Se conseguiu deserializar, retorna a resposta da API
            if (apiResponse != null)
            {
                return apiResponse;
            }

            // Fallback apenas se não conseguir deserializar (situação muito rara)
            return response.IsSuccessStatusCode
                ? SimpleResponse.CreateSuccess("Operação realizada com sucesso")
                : SimpleResponse.CreateError("Erro na operação");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            return SimpleResponse.CreateError("Erro de conexão com o servidor");
        }
        catch (TaskCanceledException ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            return SimpleResponse.CreateError("Timeout na conexão");
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            return SimpleResponse.CreateError("Erro ao processar resposta do servidor");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            return SimpleResponse.CreateError("Erro inesperado");
        }
    }

    public async Task<SimpleResponse> LoginAsyc(LoginViewModel user)
    {
        if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
            return new SimpleResponse();
        try
        {
            var dto = new LoginDTO()
            {
                Email = user.Email,
                Password = user.Password,
            };

            var response = await _authHttp.PostAsJsonAsync("v1/user/login", dto);
            if (response != null && response.IsSuccessStatusCode)
            {
                var dataResponse = await response.Content.ReadFromJsonAsync<Response<LoginDTO>>();
                if (dataResponse != null && dataResponse.Success && dataResponse.Data != null)
                {
                    var responseData = dataResponse.Data;
                    if (!string.IsNullOrWhiteSpace(responseData.Token))
                    {
                        var accessToken = responseData.Token;
                        _nav.NavigateTo($"/api/auth/set-token?token={Uri.EscapeDataString(accessToken)}", forceLoad: true);
                        return SimpleResponse.CreateSuccess("Logando...");
                    }
                    else
                    {
                        return SimpleResponse.CreateError("Erro ao processar token do servidor");
                    }
                }
                else
                {
                    Console.WriteLine($"Unexpected error: {dataResponse?.Message}");
                    return SimpleResponse.CreateError("Erro ao processar resposta do servidor");
                }
            }
            else
            {
                return SimpleResponse.CreateError("Erro de conexão com o servidor");
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            return SimpleResponse.CreateError("Erro de conexão com o servidor");
        }
        catch (TaskCanceledException ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            return SimpleResponse.CreateError("Timeout na conexão");
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            return SimpleResponse.CreateError("Erro ao processar resposta do servidor");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            return SimpleResponse.CreateError("Erro inesperado");
        }
    }
}