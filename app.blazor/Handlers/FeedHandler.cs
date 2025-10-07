using app.shared.Libs.DTOs.Feed;
using app.shared.Libs.Responses;

namespace app.blazor.Handlers
{
    public class FeedHandler
    {
        private readonly HttpClient _apiHttp;
        public FeedHandler(IHttpClientFactory httpClientFactory)
        {
            _apiHttp = httpClientFactory.CreateClient("API");
        }
        public async Task<SimpleResponse> PostAsync(string text)
        {
            var dto = new PostDto
            {
                Text = text
            };
            var response = await _apiHttp.PostAsJsonAsync("v1/feed/post", dto);
            if (response != null && response.IsSuccessStatusCode)
            {
                var dataResponse = await response.Content.ReadFromJsonAsync<SimpleResponse>();
                if (dataResponse != null)
                {
                    return dataResponse;
                }
                else
                {
                    return SimpleResponse.CreateError("Erro ao processar resposta do servidor");
                }
            }
            else
            {
                return SimpleResponse.CreateError("Erro de conexão com o servidor");

            }
        }

        public async Task<Response<List<PostDto>>> GetPostsAsync()
        { 
            var response = await _apiHttp.GetAsync("v1/feed/getposts");
            if (response != null && response.IsSuccessStatusCode)
            {
                var dataResponse = await response.Content.ReadFromJsonAsync<Response<List<PostDto>>>();
                if (dataResponse != null)
                {
                    return dataResponse;
                }
                else
                {
                    return Response<List<PostDto>>.CreateError("Erro ao processar resposta do servidor");
                }
            }
            else
            {
                return Response<List<PostDto>>.CreateError("Erro de conexão com o servidor");

            }
        }
    }
}