using System.Collections.Generic;
using System.Threading.Tasks;
using Summa.Forms.Models;

namespace Summa.Forms.WebApp.Services
{
    public interface IFormService
    {
        Task<List<Form>> ListAsync();
        
        Task<List<Form>> ListByCategoryAsync(FormCategory category);

        Task AddQuestionAsync(Form form, Question question);
    }
}