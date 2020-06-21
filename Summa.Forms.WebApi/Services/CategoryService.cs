using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<FormCategory>> ListFormCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<FormCategory> GetFormCategoryByIdAsync(Guid id)
        {
            return await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<QuestionCategory>> ListQuestionCategoriesAsync(Form form)
        {
            return await _context.Forms.Where(x => x.Id == form.Id)
                .Include(x => x.Categories)
                .SelectMany(x => x.Categories)
                .ToListAsync();
        }

        public Task<QuestionCategory> GetQuestionCategoryByIdAsync(Form form, Guid id)
        {
            throw new NotImplementedException();
        }
    }
}