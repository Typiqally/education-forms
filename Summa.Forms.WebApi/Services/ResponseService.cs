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
    public class ResponseService : IResponseService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ResponseService> _logger;

        public ResponseService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, ILogger<ResponseService> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<FormResponse> GetByIdAsync(Guid responseId)
        {
            var subject = _httpContextAccessor.HttpContext.User.GetSubject().AsGuid();
            var response = await _context.Responses
                .Where(x => x.UserId == subject || x.Form.AuthorId == subject)
                .Include(x => x.Answers)
                .ThenInclude(x => x.Question)
                .ThenInclude(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == responseId);

            if (response == null) return null;

            foreach (var answer in response.Answers)
            {
                answer.Category = answer.Question.Category;
            }

            return response;
        }

        public async Task<List<FormResponse>> ListAsync()
        {
            var subject = _httpContextAccessor.HttpContext.User.GetSubject().AsGuid();
            return await _context.Responses
                .Where(x => x.UserId == subject)
                .Include(x => x.Answers)
                .ToListAsync();
        }

        public async Task<List<FormResponse>> ListAsync(Form form)
        {
            var subject = _httpContextAccessor.HttpContext.User.GetSubject().AsGuid();
            return await _context.Responses
                .Where(x => x.UserId == subject || x.Form.AuthorId == subject)
                .Where(x => x.Form.Id == form.Id)
                .Include(x => x.Answers)
                .ToListAsync();
        }
    }
}