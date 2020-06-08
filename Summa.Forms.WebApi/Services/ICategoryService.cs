﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Summa.Forms.Models;

namespace Summa.Forms.WebApi.Services
{
    public interface ICategoryService
    {
        Task<List<FormCategory>> ListFormCategoriesAsync();
        Task<FormCategory> GetFormCategoryByIdAsync(Guid id);
        Task<List<QuestionCategory>> ListQuestionCategoriesAsync();
        Task<QuestionCategory> GetQuestionCategoryByIdAsync(Guid id);
    }
}