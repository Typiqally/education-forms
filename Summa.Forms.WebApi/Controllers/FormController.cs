using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Summa.Forms.Models;
using Summa.Forms.WebApi.Data;
using Summa.Forms.WebApi.Extensions;
using Summa.Forms.WebApi.Services;

namespace Summa.Forms.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class FormController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFormService _formService;
        private readonly IQuestionService _questionService;
        private readonly IResponseService _responseService;
        private readonly ICategoryService _categoryService;

        public FormController(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IFormService formService,
            IQuestionService questionService,
            IResponseService responseService,
            ICategoryService categoryService)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _formService = formService;
            _questionService = questionService;
            _responseService = responseService;
            _categoryService = categoryService;
        }

        [HttpGet("{formId}")]
        public async Task<IActionResult> GetForm(Guid formId)
        {
            var form = await _formService.GetByIdAsync(formId);
            return new JsonResult(form, JsonSerializationConstants.SerializerOptions);
        }

        [HttpPost("{categoryId}")]
        public async Task<IActionResult> PostForm(Guid categoryId)
        {
            var category = await _categoryService.GetFormCategoryByIdAsync(categoryId);
            var value = await _formService.CreateAsync(category);

            return new JsonResult(value, JsonSerializationConstants.SerializerOptions);
        }

        [HttpPut("{formId}")]
        public async Task<IActionResult> PutForm(Guid formId, [FromBody] Form updated)
        {
            var subject = _httpContextAccessor.HttpContext.User.GetSubject().AsGuid();
            var form = await _context.Forms.Where(x => x.Id == formId)
                .Include(x => x.Category)
                .Include(x => x.Questions)
                .ThenInclude(x => x.Options)
                .FirstOrDefaultAsync();

            if (form.AuthorId != subject)
            {
                return Unauthorized();
            }

            if (form.Id != updated.Id)
            {
                return BadRequest("Id's don't match");
            }

            var value = await _formService.UpdateAsync(form, updated);
            return new JsonResult(value, JsonSerializationConstants.SerializerOptions);
        }

        [HttpGet("{formId}/category")]
        public async Task<IActionResult> GetCategories(Guid formId)
        {
            var form = await _formService.GetByIdAsync(formId);
            var categories = await _categoryService.ListQuestionCategoriesAsync(form);

            return new JsonResult(categories, JsonSerializationConstants.SerializerOptions);
        }

        [HttpPost("{formId}/category")]
        public async Task<IActionResult> PostCategory(Guid formId, [FromBody] QuestionCategory question)
        {
            var subject = _httpContextAccessor.HttpContext.User.GetSubject().AsGuid();
            var form = await _formService.GetByIdAsync(formId);

            if (form.AuthorId != subject)
            {
                return Unauthorized();
            }

            var created = await _formService.AddCategoryAsync(form, question);

            return new JsonResult(created, JsonSerializationConstants.SerializerOptions);
        }

        [HttpDelete("{formId}/category/{categoryId}")]
        public async Task<IActionResult> DeleteCategory(Guid formId, Guid categoryId)
        {
            var subject = _httpContextAccessor.HttpContext.User.GetSubject().AsGuid();
            var form = await _formService.GetByIdAsync(formId);

            if (form.AuthorId != subject)
            {
                return Unauthorized();
            }

            var category = form.Categories.FirstOrDefault(x => x.Id == categoryId);
            await _formService.RemoveCategoryAsync(category);

            return NoContent();
        }

        [HttpGet("{formId}/question/{questionId}")]
        public async Task<IActionResult> GetQuestion(Guid formId, Guid questionId)
        {
            var form = await _formService.GetByIdAsync(formId);
            var question = await _questionService.GetByIdAsync(form, questionId);

            return new JsonResult(question, JsonSerializationConstants.SerializerOptions);
        }


        [HttpPost("{formId}/question")]
        public async Task<IActionResult> PostQuestion(Guid formId, [FromBody] Question question)
        {
            var subject = _httpContextAccessor.HttpContext.User.GetSubject().AsGuid();
            var form = await _formService.GetByIdAsync(formId);

            if (form.AuthorId != subject)
            {
                return Unauthorized();
            }

            var created = await _formService.AddQuestionAsync(form, question);

            return new JsonResult(created, JsonSerializationConstants.SerializerOptions);
        }

        [HttpDelete("{formId}/question/{questionId}")]
        public async Task<IActionResult> DeleteQuestion(Guid formId, Guid questionId)
        {
            var subject = _httpContextAccessor.HttpContext.User.GetSubject().AsGuid();
            var form = await _formService.GetByIdAsync(formId);

            if (form.AuthorId != subject)
            {
                return Unauthorized();
            }

            var question = await _questionService.GetByIdAsync(form, questionId);

            await _formService.RemoveQuestionAsync(question);

            return NoContent();
        }

        [HttpPost("{formId}/question/{questionId}/option")]
        public async Task<IActionResult> PostOption(Guid formId, Guid questionId, [FromBody] QuestionOption option)
        {
            var subject = _httpContextAccessor.HttpContext.User.GetSubject().AsGuid();
            var form = await _formService.GetByIdAsync(formId);

            if (form.AuthorId != subject)
            {
                return Unauthorized();
            }

            var question = await _questionService.GetByIdAsync(form, questionId);
            var created = await _questionService.AddOption(question, option);

            return new JsonResult(created, JsonSerializationConstants.SerializerOptions);
        }

        [HttpDelete("{formId}/question/{questionId}/option/{optionId}")]
        public async Task<IActionResult> DeleteOption(Guid formId, Guid questionId, Guid optionId)
        {
            var subject = _httpContextAccessor.HttpContext.User.GetSubject().AsGuid();
            var form = await _formService.GetByIdAsync(formId);

            if (form.AuthorId != subject)
            {
                return Unauthorized();
            }

            var question = await _questionService.GetByIdAsync(form, questionId);
            if (question.Options.Count <= 1)
            {
                return BadRequest("A question must have at least one option");
            }

            var option = await _questionService.GetOptionByIdAsync(question, optionId);
            await _questionService.RemoveOption(option);

            return NoContent();
        }

        [HttpGet("{formId}/response")]
        public async Task<IActionResult> GetResponses(Guid formId)
        {
            var form = await _formService.GetByIdAsync(formId);
            var responses = await _responseService.ListAsync(form);

            return new JsonResult(responses, JsonSerializationConstants.SerializerOptions);
        }

        [HttpPost("{formId}/response")]
        public async Task<IActionResult> PostResponse(Guid formId, [FromBody] IEnumerable<QuestionAnswer> answers)
        {
            var subject = _httpContextAccessor.HttpContext.User.GetSubject().AsGuid();
            var form = await _formService.GetByIdAsync(formId);

            if (form.AuthorId != subject)
            {
                return Unauthorized();
            }

            var list = answers.ToList();
            var valid = !list.Any(answer => answer.QuestionId == Guid.Empty || form.Questions.All(x => x.Id != answer.QuestionId));
            if (!valid)
            {
                return BadRequest("Question identifier is missing");
            }

            var created = await _formService.AddResponseAsync(form, list);
            return new JsonResult(created, JsonSerializationConstants.SerializerOptions);
        }
    }
}