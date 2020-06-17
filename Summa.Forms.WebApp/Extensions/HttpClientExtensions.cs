using System.Net.Http;
using System.Net.Http.Headers;

namespace Summa.Forms.WebApp.Extensions
{
    public static class HttpClientExtensions
    {
        public static void SetBearerToken(this HttpClient httpClient, string token)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}