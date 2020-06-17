using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Summa.Forms.Models;
using Summa.Forms.WebApp.Extensions;
using Summa.Forms.WebApp.Json;

namespace Summa.Forms.WebApp.Services
{
    public class QuestionProxyService : IQuestionProxyService
    {
        private readonly HttpClient _http;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public QuestionProxyService(HttpClient http, IHttpContextAccessor httpContextAccessor)
        {
            _http = http;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<JsonHttpResponseMessage<Question>> GetByIdAsync(Guid formId, Guid questionId)
        {
            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"/form/{formId}/question/${questionId}")
                .SetBearerToken(token);

            return await _http.SendAsync<Question>(requestMessage);
        }

        public async Task<JsonHttpResponseMessage<QuestionOption>> AddOption(Guid formId, Guid questionId, QuestionOption option)
        {
            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"/form/{formId}/question/{questionId}/option")
                .SetBearerToken(token)
                .SetBody(option);

            return await _http.SendAsync<QuestionOption>(requestMessage);
        }

        public async Task<HttpResponseMessage> RemoveOption(Guid formId, Guid questionId, Guid optionId)
        {
            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete,
                    $"/form/{formId.ToString()}/question/{questionId.ToString()}/option/{optionId.ToString()}")
                .SetBearerToken(token);

            return await _http.SendAsync(requestMessage);
        }
    }
}