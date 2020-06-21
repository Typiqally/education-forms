using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
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
        private readonly IEmailSender _emailSender;
        private readonly ILogger<FormService> _logger;

        public FormService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IEmailSender emailSender, ILogger<FormService> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _emailSender = emailSender;
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

        public async Task<Form> UpdateAsync(Form form, Form updated)
        {
            form.Title = updated.Title;
            form.Description = updated.Description;
            form.Questions = updated.Questions;

            await _context.SaveChangesAsync();

            return updated;
        }

        public async Task<FormResponse> AddResponseAsync(Form form, IEnumerable<QuestionAnswer> answers)
        {
            var subject = _httpContextAccessor.HttpContext.User.GetSubject();
            var email = _httpContextAccessor.HttpContext.User.GetEmail();

            var response = new FormResponse
            {
                FormId = form.Id,
                UserId = Guid.Parse(subject),
                Answers = answers.ToList()
            };

            await _context.Responses.AddRangeAsync(response);
            await _context.SaveChangesAsync();

            await _emailSender.SendEmailAsync(email, $"{form.Title} Results",
                $"You can check your results by <a href='{HtmlEncoder.Default.Encode($"https://localhost:5001/Response/{response.Id}")}'>clicking here</a>.");

            return response;
        }
    }
}