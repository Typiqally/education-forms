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
    public class FormService : IFormService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<FormService> _logger;

        public FormService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, ILogger<FormService> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<Form> GetByIdAsync(Guid guid)
        {
            var form = await _context.Forms.Where(x => x.Id == guid)
                .Include(x => x.Category)
                .Include(x => x.Questions)
                .ThenInclude(x => x.Options)
                .FirstOrDefaultAsync();

            return form;
        }

        public async Task<List<Form>> ListAsync()
        {
            var subject = _httpContextAccessor.HttpContext.User.GetSubject().AsGuid();
            var forms = await _context.Forms
                .Where(x => x.AuthorId == subject)
                .Include(x => x.Category)
                .AsNoTracking()
                .ToListAsync();

            return forms;
        }

        public async Task<List<Form>> ListAsync(FormCategory category)
        {
            var subject = _httpContextAccessor.HttpContext.User.GetSubject().AsGuid();
            var forms = await _context.Forms
                .Where(x => x.AuthorId == subject)
                .Where(x => x.Category == category)
                .AsNoTracking()
                .ToListAsync();

            return forms;
        }

        public async Task<Question> AddQuestionAsync(Form form, Question question)
        {
            question.Options = new List<QuestionOption>();
            switch (question.Type)
            {
                case QuestionType.MultipleChoice:
                    question.Options.Add(new QuestionOption
                    {
                        Type = question.Type,
                        Title = "Option 1"
                    });
                    break;
                case QuestionType.LinearScale:
                    question.Options.AddRange(
                        new QuestionOption
                        {
                            Index = 0,
                            Type = question.Type,
                            Value = 1
                        }, new QuestionOption
                        {
                            Index = 1,
                            Type = question.Type,
                            Value = 10
                        });
                    break;
                case QuestionType.Open:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            form.Questions.Add(question);

            await _context.SaveChangesAsync();

            return question;
        }

        public async Task RemoveQuestionAsync(Question question)
        {
            _context.Entry(question).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        public async Task<Form> UpdateAsync(Form form)
        {
            try
            {
                _context.Update(form);
                _context.Entry(form).Property(x => x.Id).IsModified = false;
                _context.Entry(form).Property(x => x.AuthorId).IsModified = false;
                _context.Entry(form).Property(x => x.TimeCreated).IsModified = false;
                _context.Entry(form).Property(x => x.Title).IsModified = form.Title != null;
                _context.Entry(form).Reference(x => x.Category).IsModified = false;
            }
            catch (DbUpdateConcurrencyException)
            {
                _logger.LogWarning($"Concurrency exception occured when trying to update form {form.Id}");
            }

            await _context.SaveChangesAsync();

            return form;
        }

        public async Task<FormResponse> AddResponseAsync(Form form, IEnumerable<QuestionAnswer> answers)
        {
            var subject = _httpContextAccessor.HttpContext.User.GetSubject();
            var response = new FormResponse
            {
                FormId = form.Id,
                UserId = Guid.Parse(subject),
                Answers = answers.ToList()
            };

            await _context.Responses.AddRangeAsync(response);
            await _context.SaveChangesAsync();

            return response;
        }
    }
}