using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Summa.Forms.Models;

namespace Summa.Forms.WebApi.Services
{
    public interface IFormService
    {
        Task<Form> GetByIdAsync(Guid guid);
        Task<List<Form>> ListAsync();
        Task<List<Form>> ListAsync(FormCategory category);
        Task<Question> AddQuestionAsync(Form form, Question question);
        Task RemoveQuestionAsync(Question question);
        Task<Form> UpdateAsync(Form form, Form updated);
        Task<FormResponse> AddResponseAsync(Form form, IEnumerable<QuestionAnswer> answers);
    }
}