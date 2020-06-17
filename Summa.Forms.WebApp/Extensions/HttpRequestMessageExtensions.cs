using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Summa.Forms.WebApp.Extensions
{
    public static class HttpRequestMessageExtensions
    {
        public static HttpRequestMessage SetBearerToken(this HttpRequestMessage requestMessage, string token)
        {
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return requestMessage;
        }

        public static HttpRequestMessage SetBody(this HttpRequestMessage requestMessage, object data)
        {
            requestMessage.Content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            return requestMessage;
        }
    }
}