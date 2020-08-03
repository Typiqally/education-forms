using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Summa.Forms.Models;
using Summa.Forms.WebApp.Json;

namespace Summa.Forms.WebApp.Services
{
    public interface IFormProxyService
    {
        Task<JsonHttpResponseMessage<Form>> CreateAsync(Guid categoryId);
        Task<JsonHttpResponseMessage<Form>> GetByIdAsync(Guid formId);
        Task<JsonHttpResponseMessage<List<Form>>> ListAsync();
        Task<JsonHttpResponseMessage<List<Form>>> ListByCategoryAsync(FormCategory category);
        Task<JsonHttpResponseMessage<Form>> UpdateAsync(Guid formId, Form form);
        Task<JsonHttpResponseMessage<QuestionCategory>> AddCategoryAsync(Guid formId, QuestionCategory category);
        Task<HttpResponseMessage> RemoveCategoryAsync(Guid formId, Guid categoryId);
        Task<JsonHttpResponseMessage<Question>> AddQuestionAsync(Guid formId, Question question);
        Task<HttpResponseMessage> RemoveQuestionAsync(Guid formId, Guid questionId);
        Task<JsonHttpResponseMessage<List<FormResponse>>> ListResponsesAsync(Guid formId);
        Task<JsonHttpResponseMessage<FormResponse>> AddResponseAsync(Guid formId, IEnumerable<QuestionAnswer> answers);
    }
}