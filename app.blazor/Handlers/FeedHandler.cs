using app.shared.Libs.DTOs.Feed;
using app.shared.Libs.Responses;
using Microsoft.AspNetCore.Components;

namespace app.blazor.Handlers
{
    public class FeedHandler
    {
        private readonly HttpClient _apiHttp;
        private readonly CookieAuthenticationStateProvider _authStateProvider;
        private readonly NavigationManager _navigation;
        public FeedHandler(IHttpClientFactory httpClientFactory, CookieAuthenticationStateProvider authStateProvider, NavigationManager navigationManager)
        {
            _navigation = navigationManager;
            _authStateProvider = authStateProvider;
            _apiHttp = httpClientFactory.CreateClient("API");
            var jwtToken = _authStateProvider.GetJwtToken();
            if (string.IsNullOrWhiteSpace(jwtToken))
                return;
            _apiHttp.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);
        }
        public async Task<Result<PostDto>> PostAsync(PostDto dto)
        {
            var response = await _apiHttp.PostAsJsonAsync("v1/feed/post", dto);
            if (response == null)
                return Result<PostDto>.Fail("Erro de conexão com o servidor", OperationStatus.Error);
            
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _navigation.NavigateTo("/user/logout", true);
                return Result<PostDto>.Fail("Usuário não autorizado", OperationStatus.Unauthorized);
            }

            if (!response.IsSuccessStatusCode)
                return Result<PostDto>.Fail("Erro na requisição", OperationStatus.Error);
            
            try
            {
                var result = await response.Content.ReadFromJsonAsync<Result<PostDto>>();
                return result ?? Result<PostDto>.Fail("Resposta vazia ou inválida", OperationStatus.Error);
            }
            catch (Exception ex)
            {
                return Result<PostDto>.Fail(ex.Message, OperationStatus.Error);
            }
        }
        public async Task<Result<List<PostDto>>> GetPostsAsync()
        {
            var response = await _apiHttp.GetAsync("v1/feed/getposts");
            if (response == null)
                return Result<List<PostDto>>.Fail("Erro de conexão com o servidor", OperationStatus.Error);
            
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _navigation.NavigateTo("/user/logout", true);
                return Result<List<PostDto>>.Fail("Usuário não autorizado", OperationStatus.Unauthorized);
            }

            if (!response.IsSuccessStatusCode)
                return Result<List<PostDto>>.Fail("Erro de conexão com o servidor", OperationStatus.Error);

            try
            {
                var result = await response.Content.ReadFromJsonAsync<Result<List<PostDto>>>();
                return result ?? Result<List<PostDto>>.Fail("Resposta vazia ou inválida", OperationStatus.Error);
            }
            catch (Exception ex)
            {
                return Result<List<PostDto>>.Fail(ex.Message, OperationStatus.Error);
            }
        }
        public async Task<Result<string>> LikePost(PostDto dto)
        {
            var response = await _apiHttp.PostAsJsonAsync("v1/feed/likepost", dto);
            if (response == null)
                return Result<string>.Fail("Erro de conexão com o servidor", OperationStatus.Error);
            
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _navigation.NavigateTo("/user/logout", true);
                return Result<string>.Fail("Usuário não autorizado", OperationStatus.Unauthorized);
            }

            if (!response.IsSuccessStatusCode)
                return Result<string>.Fail("Erro na requisição", OperationStatus.Error);

            try
            {
                var result = await response.Content.ReadFromJsonAsync<Result<string>>();
                return result ?? Result<string>.Fail("Resposta vazia ou inválida", OperationStatus.Error);
            }
            catch (Exception ex)
            {
                return Result<string>.Fail(ex.Message, OperationStatus.Error);
            }
        }
        public async Task<Result<bool>> getLiked(PostDto dto)
        {
            var response = await _apiHttp.PostAsJsonAsync($"v1/feed/getpostliked/", dto);
            if (response == null)
                return Result<bool>.Fail("Erro de conexão com o servidor");
            
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _navigation.NavigateTo("/user/logout", true);
                return Result<bool>.Fail("Usuário não autorizado", OperationStatus.Unauthorized);
            }

            if (!response.IsSuccessStatusCode)
                return Result<bool>.Fail("Erro na requisição");

            try
            {
                var result = await response.Content.ReadFromJsonAsync<Result<bool>>();
                return result ?? Result<bool>.Fail("Resposta vazia ou inválida");
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail(ex.Message);
            }
        }
    }
}