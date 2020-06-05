using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Summa.Forms.Models;

namespace Summa.Forms.WebApi.Services
{
    public interface IFormService
    {
        Task<Form> GetByIdAsync(Guid guid);
        Task<List<Form>> ListAsync();
        Task<List<Form>> ListByCategoryAsync(FormCategory category);
        Task<Question> AddQuestion(Guid formId, Question question);
        Task RemoveQuestion(Guid formId, Guid questionId);
        Task UpdateValuesAsync(Form form);
    }
}