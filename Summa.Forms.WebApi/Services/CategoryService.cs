using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Summa.Forms.Models;
using Summa.Forms.WebApi.Data;

namespace Summa.Forms.WebApi.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<FormCategory>> ListAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<FormCategory> GetByIdAsync(Guid id)
        {
            return await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}