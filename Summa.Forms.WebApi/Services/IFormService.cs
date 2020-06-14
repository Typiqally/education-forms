using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Summa.Forms.Models;

namespace Summa.Forms.WebApi.Services
{
    public interface IFormService
    {
        Task<Form> GetByIdAsync(Guid formId, bool authorize = true);
        Task<List<Form>> ListAsync();
        Task<List<Form>> ListByCategoryAsync(FormCategory category);
        Task<Question> AddQuestionAsync(Guid formId, Question question);
        Task RemoveQuestionAsync(Guid formId, Guid questionId);
        Task<Form> UpdateAsync(Guid formId, Form form);
        Task<List<FormResponse>> ListResponsesAsync(Guid formId);
        Task<FormResponse> AddResponseAsync(Guid formId, IEnumerable<QuestionAnswer> answers);
    }
}