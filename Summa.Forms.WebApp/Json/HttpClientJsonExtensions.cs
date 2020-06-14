using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Summa.Forms.WebApp.Json
{
    public static class HttpClientJsonExtensions
    {
        public static async Task<JsonHttpResponseMessage<T>> SendAsync<T>(
            this HttpClient httpClient,
            HttpRequestMessage requestMessage)
        {
            var response = await httpClient.SendAsync(requestMessage);
            return await response.Deserialize<T>();
        }
        
        public static async Task<JsonHttpResponseMessage<T>> Deserialize<T>(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                return new JsonHttpResponseMessage<T>(response);
            }

            await using var contentStream = await response.Content.ReadAsStreamAsync();
            var data = await JsonSerializer.DeserializeAsync<T>(contentStream, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return new JsonHttpResponseMessage<T>(response, data);
        }

        public static async Task<JsonHttpResponseMessage<T>> SendAsync<T>(
            this HttpClient httpClient,
            string uri = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri ?? "/");
            return await httpClient.SendAsync<T>(request);
        }
    }
}