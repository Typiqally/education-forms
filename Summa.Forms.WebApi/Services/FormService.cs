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

        public async Task AddQuestionAsync(Form form, Question question)
        {
            form.Questions.Add(question);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Form form)
        {
            var subject = _httpContextAccessor.HttpContext.User.GetSubject();
            var original = await _context.Forms
                .Where(x => x.AuthorId.ToString() == subject)
                .Where(x => x.Id == form.Id)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            form.Id = original.Id;
            form.AuthorId = original.AuthorId;
            form.TimeCreated = original.TimeCreated;

            _context.Update(form);

            await _context.SaveChangesAsync();
        }
    }
}