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

        public async Task<List<RepositoryForm>> ListAsync()
        {
            return await _context.Repository
                .Include(x => x.Form)
                .ThenInclude(x => x.Questions)
                .ThenInclude(x => x.Options)
                .ToListAsync();
        }

        public async Task AddAsync(Form form)
        {
            var entry = new RepositoryForm
            {
                Form = form,
                PublishDate = DateTime.Now
            };

            await _context.Repository.AddAsync(entry);
            await _context.SaveChangesAsync();
        }
    }
}