using System.Collections.Generic;
using System.Threading.Tasks;
using Summa.Forms.Models;

namespace Summa.Forms.WebApi.Services
{
    public interface IRepositoryService
    {
        Task<List<RepositoryForm>> ListAsync();
        Task AddAsync(Form form);
    }
}