using app.shared.Libs.DTOs.User;
using app.blazor.UI.ViewModels.User;
using app.shared.Libs.Responses;
using System.Text.Json;
using app.blazor.Utils;
using Blazored.LocalStorage;

namespace app.blazor.UI.Handlers;

public class AuthHandler
{
    private readonly HttpClient _authHttp;
    private readonly JwtAuthenticationStateProvider _jwtAuthProvider;

    public AuthHandler(IHttpClientFactory httpClientFactory, ILocalStorageService storage, JwtAuthenticationStateProvider JwtAuthenticationStateProvider)
    {
        _authHttp = httpClientFactory.CreateClient("AUTH");
        _jwtAuthProvider = JwtAuthenticationStateProvider;
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
        if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password)) {
            return new SimpleResponse();
        }

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
                    // Atualiza o token no ViewModel
                    var responseData = JsonSerializer.Deserialize<LoginDTO>(dataResponse.Data.ToString() ?? "");
                    if (responseData != null && !string.IsNullOrWhiteSpace(responseData.Token))
                    {
                        var accessToken = responseData.Token;
                        await _jwtAuthProvider.SetAsync(accessToken);
                        _authHttp.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                        return SimpleResponse.CreateSuccess("Logado com sucesso");
                    } else {
                        return SimpleResponse.CreateError("Erro ao processar resposta do servidor");
                    }
                } else {
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