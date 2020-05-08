using System.Collections.Generic;
using System.Threading.Tasks;
using Summa.Forms.Models;

namespace Summa.Forms.WebApp.Services
{
    public interface IRepositoryService
    {
        Task<List<Form>> ListAsync();

        Task AddAsync(Form form);
    }
}