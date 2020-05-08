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
    public class RepositoryService : IRepositoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<RepositoryService> _logger;

        public RepositoryService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, ILogger<RepositoryService> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<List<Form>> ListAsync()
        {
            var subject = _httpContextAccessor.HttpContext.User.GetSubject();
            var entries = await _context.Repository
                .Where(x => x.Form.AuthorId.ToString() == subject)
                .ToListAsync();

            var forms = entries.Select(x => x.Form).ToList();

            return forms;
        }

        public async Task AddAsync(Form form)
        {
            var entry = new RepositoryEntry()
            {
                Form = form
            };

           await _context.Repository.AddAsync(entry);
           await _context.SaveChangesAsync();
        }
    }
}