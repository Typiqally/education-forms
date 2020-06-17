using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Summa.Forms.Models;

namespace Summa.Forms.WebApi.Services
{
    public interface ICategoryService
    {
        Task<List<FormCategory>> ListAsync();
        Task<FormCategory> GetByIdAsync(Guid id);
    }
}