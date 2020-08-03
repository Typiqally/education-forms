using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Summa.Forms.Models;
using Summa.Forms.WebApp.Extensions;
using Summa.Forms.WebApp.Json;
using Summa.Forms.WebApp.Services;

namespace Summa.Forms.WebApp.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class FormController : Controller
    {
        private readonly IFormProxyService _formProxyService;
        private readonly ICategoryProxyService _categoryProxyService;
        private readonly IQuestionProxyService _questionProxyService;

        public FormController(IFormProxyService formProxyService, ICategoryProxyService categoryProxyService, IQuestionProxyService questionProxyService)
        {
            _formProxyService = formProxyService;
            _categoryProxyService = categoryProxyService;
            _questionProxyService = questionProxyService;
        }

        [HttpGet("{formId}/edit")]
        public async Task<IActionResult> Edit(Guid formId)
        {
            var response = await _formProxyService.GetByIdAsync(formId);
            if (!response.Message.IsSuccessStatusCode)
            {
                return StatusCode((int) response.Message.StatusCode, response.Message.Content);
            }

            return View(response.Data);
        }

        [HttpGet("{formId}/view")]
        public async Task<IActionResult> View(Guid formId)
        {
            var response = await _formProxyService.GetByIdAsync(formId);
            if (!response.Message.IsSuccessStatusCode)
            {
                return StatusCode((int) response.Message.StatusCode, response.Message.Content);
            }

            return View(response.Data);
        }
        
        [HttpPost("{categoryId}")]
        public async Task<IActionResult> PutForm(Guid categoryId)
        {
            var response = await _formProxyService.CreateAsync(categoryId);

            return response.GetJsonResult(JsonSerializationConstants.SerializerOptions);
        }

        [HttpPut("{formId}")]
        public async Task<IActionResult> PutForm(Guid formId, [FromBody] Form form)
        {
            var response = await _formProxyService.UpdateAsync(formId, form);

            return response.GetJsonResult(JsonSerializationConstants.SerializerOptions);
        }

        [HttpGet("{formId}/category")]
        public async Task<IActionResult> GetCategories(Guid formId)
        {
            var response = await _categoryProxyService.ListQuestionCategoriesAsync(formId);

            return response.GetJsonResult(JsonSerializationConstants.SerializerOptions);
        }


        [HttpPost("{formId}/category")]
        public async Task<IActionResult> PostCategory(Guid formId, [FromBody] QuestionCategory category)
        {
            var response = await _formProxyService.AddCategoryAsync(formId, category);

            return response.GetJsonResult(JsonSerializationConstants.SerializerOptions);
        }

        [HttpDelete("{formId}/category/{categoryId}")]
        public async Task<IActionResult> DeleteCategory(Guid formId, Guid categoryId)
        {
            var response = await _formProxyService.RemoveCategoryAsync(formId, categoryId);

            return response.GetNoContentResult();
        }

        [HttpPost("{formId}/question")]
        public async Task<IActionResult> PostQuestion(Guid formId, [FromBody] Question question)
        {
            var response = await _formProxyService.AddQuestionAsync(formId, question);

            return response.GetJsonResult(JsonSerializationConstants.SerializerOptions);
        }

        [HttpDelete("{formId}/question/{questionId}")]
        public async Task<IActionResult> DeleteQuestion(Guid formId, Guid questionId)
        {
            var response = await _formProxyService.RemoveQuestionAsync(formId, questionId);

            return response.GetNoContentResult();
        }

        [HttpPost("{formId}/question/{questionId}/option")]
        public async Task<IActionResult> PostOption(Guid formId, Guid questionId, [FromBody] QuestionOption option)
        {
            var response = await _questionProxyService.AddOption(formId, questionId, option);

            return response.GetJsonResult(JsonSerializationConstants.SerializerOptions);
        }

        [HttpDelete("{formId}/question/{questionId}/option/{optionId}")]
        public async Task<IActionResult> DeleteOption(Guid formId, Guid questionId, Guid optionId)
        {
            var response = await _questionProxyService.RemoveOption(formId, questionId, optionId);

            return response.GetNoContentResult();
        }

        [HttpPost("{formId}/response")]
        public async Task<IActionResult> PostResponse(Guid formId, [FromBody] IEnumerable<QuestionAnswer> answers)
        {
            var response = await _formProxyService.AddResponseAsync(formId, answers);

            return response.GetJsonResult(JsonSerializationConstants.SerializerOptions);
        }
    }
}