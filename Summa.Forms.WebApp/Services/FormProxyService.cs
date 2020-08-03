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
    public class FormProxyService : IFormProxyService
    {
        private readonly HttpClient _http;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FormProxyService(HttpClient http, IHttpContextAccessor httpContextAccessor)
        {
            _http = http;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<JsonHttpResponseMessage<Form>> CreateAsync(Guid categoryId)
        {
            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"/Form/{categoryId.ToString()}")
                .SetBearerToken(token);

            return await _http.SendAsync<Form>(requestMessage);
        }

        public async Task<JsonHttpResponseMessage<Form>> GetByIdAsync(Guid formId)
        {
            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"/Form/{formId.ToString()}")
                .SetBearerToken(token);

            return await _http.SendAsync<Form>(requestMessage);
        }

        public async Task<JsonHttpResponseMessage<List<Form>>> ListAsync()
        {
            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "/user/forms")
                .SetBearerToken(token);

            return await _http.SendAsync<List<Form>>(requestMessage);
        }

        public async Task<JsonHttpResponseMessage<List<Form>>> ListByCategoryAsync(FormCategory category)
        {
            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"/user/forms/{category.Id}")
                .SetBearerToken(token);

            return await _http.SendAsync<List<Form>>(requestMessage);
        }

        public async Task<JsonHttpResponseMessage<QuestionCategory>> AddCategoryAsync(Guid formId, QuestionCategory category)
        {
            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"/form/{formId}/category")
                .SetBearerToken(token)
                .SetBody(category);

            return await _http.SendAsync<QuestionCategory>(requestMessage);
        }

        public async Task<HttpResponseMessage> RemoveCategoryAsync(Guid formId, Guid categoryId)
        {
            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"/form/{formId}/category/{categoryId}")
                .SetBearerToken(token);

            return await _http.SendAsync(requestMessage);
        }

        public async Task<JsonHttpResponseMessage<Question>> AddQuestionAsync(Guid formId, Question question)
        {
            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"/form/{formId}/question")
                .SetBearerToken(token)
                .SetBody(question);

            return await _http.SendAsync<Question>(requestMessage);
        }

        public async Task<HttpResponseMessage> RemoveQuestionAsync(Guid formId, Guid questionId)
        {
            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"/form/{formId}/question/{questionId}")
                .SetBearerToken(token);

            return await _http.SendAsync(requestMessage);
        }

        public async Task<JsonHttpResponseMessage<List<FormResponse>>> ListResponsesAsync(Guid formId)
        {
            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"/form/{formId}/response")
                .SetBearerToken(token);

            return await _http.SendAsync<List<FormResponse>>(requestMessage);
        }

        public async Task<JsonHttpResponseMessage<FormResponse>> AddResponseAsync(Guid formId, IEnumerable<QuestionAnswer> answers)
        {
            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"/form/{formId}/response")
                .SetBearerToken(token)
                .SetBody(answers);

            return await _http.SendAsync<FormResponse>(requestMessage);
        }

        public async Task<JsonHttpResponseMessage<Form>> UpdateAsync(Guid formId, Form form)
        {
            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, $"/form/{formId}")
                .SetBearerToken(token)
                .SetBody(form);

            return await _http.SendAsync<Form>(requestMessage);
        }
    }
}