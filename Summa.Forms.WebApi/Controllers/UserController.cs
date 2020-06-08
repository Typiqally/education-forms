﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Summa.Forms.Models;
using Summa.Forms.WebApi.Data;
using Summa.Forms.WebApi.Services;

namespace Summa.Forms.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IFormService _formService;
        private readonly ICategoryService _categoryService;

        public UserController(IFormService formService, ICategoryService categoryService)
        {
            _formService = formService;
            _categoryService = categoryService;
        }

        [HttpGet("forms")]
        public async Task<IActionResult> GetForms()
        {
            var forms = await _formService.ListAsync();

            return new JsonResult(forms, JsonSerializationConstants.SerializerOptions);
        }

        [HttpGet("forms/{guid}")]
        public async Task<IActionResult> GetFormsByCategory(Guid guid)
        {
            var category = await _categoryService.GetFormCategoryByIdAsync(guid);
            var forms = await _formService.ListByCategoryAsync(category);

            return new JsonResult(forms, JsonSerializationConstants.SerializerOptions);
        }
    }
}