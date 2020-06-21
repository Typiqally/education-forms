using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Summa.Forms.Models;
using Summa.Forms.WebApp.Extensions;
using Summa.Forms.WebApp.Json;

namespace Summa.Forms.WebApp.Services
{
    public class CategoryProxyService : ICategoryProxyService
    {
        private readonly HttpClient _http;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CategoryProxyService(HttpClient http, IHttpContextAccessor httpContextAccessor)
        {
            _http = http;
            _httpContextAccessor = httpContextAccessor;
        }
        
        public async Task<JsonHttpResponseMessage<List<QuestionCategory>>> ListQuestionCategoriesAsync(Guid formId)
        {
            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"/Form/{formId.ToString()}/Category")
                .SetBearerToken(token);

            return await _http.SendAsync<List<QuestionCategory>>(requestMessage);
        }
    }
}