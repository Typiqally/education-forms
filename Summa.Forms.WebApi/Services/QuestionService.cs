using System;
using System.Collections.Generic;
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

        public async Task<Question> GetByIdAsync(Guid formId, Guid questionId)
        {
            var subject = _httpContextAccessor.HttpContext.User.GetSubject();
            return await _context.Forms
                .Where(x => x.AuthorId.ToString() == subject)
                .Where(x => x.Id == formId)
                .Include(x => x.Questions)
                .ThenInclude(x => x.Options)
                .SelectMany(x => x.Questions)
                .FirstOrDefaultAsync(x => x.Id == questionId);
        }

        public async Task<QuestionOption> AddOption(Guid formId, Guid questionId, QuestionOption option)
        {
            var question = await GetByIdAsync(formId, questionId);

            option.Type = question.Type;
            question.Options.Add(option);

            await _context.SaveChangesAsync();

            return option;
        }

        public async Task RemoveOption(Guid formId, Guid questionId, Guid optionId)
        {
            var question = await GetByIdAsync(formId, questionId);
            var option = question.Options.FirstOrDefault(x => x.Id == optionId);

            question.Options.Remove(option);

            await _context.SaveChangesAsync();
        }

        public async Task<FormResponse> AddResponseAsync(Guid formId, IEnumerable<QuestionAnswer> answers)
        {
            var subject = _httpContextAccessor.HttpContext.User.GetSubject();
            var response = new FormResponse
            {
                FormId = formId,
                UserId = Guid.Parse(subject),
                Answers = answers.ToList()
            };

            await _context.Responses.AddRangeAsync(response);
            await _context.SaveChangesAsync();

            return response;
        }
    }
}