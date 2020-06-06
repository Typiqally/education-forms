using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Summa.Forms.Models;
using Summa.Forms.WebApi.Data;
using Summa.Forms.WebApi.Data.Migrations;
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
            var subject = _httpContextAccessor.HttpContext.User.GetSubject();
            var form = await _context.Forms
                .Where(x => x.AuthorId.ToString() == subject)
                .Where(x => x.Id == guid)
                .Include(x => x.Category)
                .Include(x => x.Questions)
                .ThenInclude(x => x.Options)
                .FirstOrDefaultAsync();

            return form;
        }

        public async Task<List<Form>> ListAsync()
        {
            var subject = _httpContextAccessor.HttpContext.User.GetSubject();
            var forms = await _context.Forms
                .Where(x => x.AuthorId.ToString() == subject)
                .Include(x => x.Category)
                .ToListAsync();

            return forms;
        }

        public async Task<List<Form>> ListByCategoryAsync(FormCategory category)
        {
            var subject = _httpContextAccessor.HttpContext.User.GetSubject();
            var forms = await _context.Forms
                .Where(x => x.AuthorId.ToString() == subject)
                .Where(x => x.Category == category)
                .ToListAsync();

            return forms;
        }

        public async Task<Question> AddQuestion(Guid formId, Question question)
        {
            var form = await GetByIdAsync(formId);

            question.Options = new List<QuestionOption>();
            if (question.Type == QuestionType.LinearScale)
            {
                question.Options.AddRange(new[]
                {
                    new QuestionOption
                    {
                        Index = 0,
                        Type = question.Type,
                        Value = "1"
                    },
                    new QuestionOption
                    {
                        Index = 1,
                        Type = question.Type,
                        Value = "10"
                    }
                });
            }

            form.Questions.Add(question);

            await _context.SaveChangesAsync();

            return question;
        }

        public async Task RemoveQuestion(Guid formId, Guid questionId)
        {
            var form = await GetByIdAsync(formId);
            var question = form.Questions.FirstOrDefault(x => x.Id == questionId);

            _context.Entry(question).State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateValuesAsync(Form form)
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
        }
    }
}