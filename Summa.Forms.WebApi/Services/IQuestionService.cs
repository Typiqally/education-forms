using System;
using System.Threading.Tasks;
using Summa.Forms.Models;

namespace Summa.Forms.WebApi.Services
{
    public interface IQuestionService
    {
        Task<Question> GetByIdAsync(Guid formId, Guid questionId);
        Task<QuestionOption> AddOption(Guid formId, Guid questionId, QuestionOption option);
        Task RemoveOption(Guid formId, Guid questionId, Guid optionId);
    }
}