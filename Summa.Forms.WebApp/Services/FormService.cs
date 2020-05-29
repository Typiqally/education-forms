using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Summa.Forms.Models;
using Summa.Forms.WebApp.Data;
using Summa.Forms.WebApp.Extensions;

namespace Summa.Forms.WebApp.Services
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
                //.Where(x => x.AuthorId.ToString() == subject)
                .Where(x => x.Id == guid)
                .Include(x => x.Questions)
                .ThenInclude(x => x.Options)
                .FirstAsync();

            return form;
        }

        public async Task<List<Form>> ListAsync()
        {
            var subject = _httpContextAccessor.HttpContext.User.GetSubject();
            var forms = await _context.Forms
                .Where(x => x.AuthorId.ToString() == subject)
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
    }
}