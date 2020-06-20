using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Summa.Forms.Models;
using Summa.Forms.WebApi.Data;
using Summa.Forms.WebApi.Extensions;

namespace Summa.Forms.WebApi.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<QuestionService> _logger;

        public QuestionService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, ILogger<QuestionService> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<Question> GetByIdAsync(Form form, Guid questionId)
        {
            var subject = _httpContextAccessor.HttpContext.User.GetSubject().AsGuid();
            return await _context.Forms
                .Where(x => x.AuthorId == subject)
                .Where(x => x.Id == form.Id)
                .Include(x => x.Questions)
                .ThenInclude(x => x.Options)
                .SelectMany(x => x.Questions)
                .FirstOrDefaultAsync(x => x.Id == questionId);
        }

        public async Task<QuestionOption> GetOptionByIdAsync(Question question, Guid optionId)
        {
            return question.Options.FirstOrDefault(x => x.Id == optionId);
        }

        public async Task<QuestionOption> AddOption(Question question, QuestionOption option)
        {
            option.Type = question.Type;
            question.Options.Add(option);

            await _context.SaveChangesAsync();

            return option;
        }

        public async Task RemoveOption(QuestionOption option)
        {
            _context.Entry(option).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }
    }
}