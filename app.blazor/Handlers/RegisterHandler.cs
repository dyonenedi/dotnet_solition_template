using app.shared.Libs.DTOs.User;
using app.blazor.UI.ViewModels;
using app.shared.Libs.Responses;
using System.Text.Json;

namespace app.blazor.UI.Handlers;

public class RegisterHandler {
    private readonly HttpClient _authHttp;

    public RegisterHandler(IHttpClientFactory httpClientFactory) {
        _authHttp = httpClientFactory.CreateClient("AUTH");
    }

    public async Task<SimpleResponse> SubmitAsync(RegisterViewModel vm) {
        try 
        {
            var dto = new RegisterDTO {
                FullName = vm.FullName ?? "",
                Username = vm.Username,
                Email = vm.Email,
                Password = vm.Password,
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
}