using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Summa.Forms.Models;
using Summa.Forms.WebApp.Json;

namespace Summa.Forms.WebApp.Services
{
    public interface ICategoryProxyService
    {
        Task<JsonHttpResponseMessage<List<QuestionCategory>>> ListQuestionCategoriesAsync(Guid formId);
    }
}