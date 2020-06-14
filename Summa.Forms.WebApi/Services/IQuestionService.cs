using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Summa.Forms.Models;

namespace Summa.Forms.WebApi.Services
{
    public interface IQuestionService
    {
        Task<Question> GetByIdAsync(Guid formId, Guid questionId, QueryTrackingBehavior tracking = QueryTrackingBehavior.TrackAll);
        Task<QuestionOption> AddOption(Guid formId, Guid questionId, QuestionOption option);
        Task RemoveOption(Guid formId, Guid questionId, Guid optionId);
    }
}