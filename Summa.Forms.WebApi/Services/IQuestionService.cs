using System;
using System.Threading.Tasks;
using Summa.Forms.Models;

namespace Summa.Forms.WebApi.Services
{
    public interface IQuestionService
    {
        Task<Question> GetByIdAsync(Form form, Guid questionId);
        Task<QuestionOption> GetOptionByIdAsync(Question question, Guid optionId);
        Task<QuestionOption> AddOption(Question question, QuestionOption option);
        Task RemoveOption(QuestionOption option);
    }
}