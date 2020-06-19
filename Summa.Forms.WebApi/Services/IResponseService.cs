using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Summa.Forms.Models;

namespace Summa.Forms.WebApi.Services
{
    public interface IResponseService
    {
        Task<FormResponse> GetByIdAsync(Guid responseId);
        Task<List<FormResponse>> ListAsync();
        Task<List<FormResponse>> ListAsync(Form form);
    }
}