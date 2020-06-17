using System;
using System.Net.Http;
using System.Threading.Tasks;
using Summa.Forms.Models;
using Summa.Forms.WebApp.Json;

namespace Summa.Forms.WebApp.Services
{
    public interface IQuestionProxyService
    {
        Task<JsonHttpResponseMessage<Question>> GetByIdAsync(Guid formId, Guid questionId);
        Task<JsonHttpResponseMessage<QuestionOption>> AddOption(Guid formId, Guid questionId, QuestionOption option);
        Task<HttpResponseMessage> RemoveOption(Guid formId, Guid questionId, Guid optionId);
    }
}