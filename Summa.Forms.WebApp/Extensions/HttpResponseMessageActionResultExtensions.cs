using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Summa.Forms.WebApp.Json;

namespace Summa.Forms.WebApp.Extensions
{
    public static class HttpResponseMessageActionResultExtensions
    {
        public static IActionResult GetJsonResult<T>(this JsonHttpResponseMessage<T> response, JsonSerializerOptions options)
        {
            if (!response.Message.IsSuccessStatusCode)
            {
                return new ObjectResult(response.Message.Content)
                {
                    StatusCode = (int?) response.Message.StatusCode
                };
            }

            return new JsonResult(response.Data, options);
        }

        public static IActionResult GetNoContentResult(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                return new ObjectResult(response.Content)
                {
                    StatusCode = (int?) response.StatusCode
                };
            }

            return new NoContentResult();
        }
    }
}