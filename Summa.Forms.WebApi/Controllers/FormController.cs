using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Summa.Forms.Models;
using Summa.Forms.WebApi.Data;
using Summa.Forms.WebApi.Services;

namespace Summa.Forms.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class FormController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IFormService _formService;
        private readonly IQuestionService _questionService;
        private readonly IResponseService _responseService;

        public FormController(ApplicationDbContext context, IFormService formService, IQuestionService questionService, IResponseService responseService)
        {
            _context = context;
            _formService = formService;
            _questionService = questionService;
            _responseService = responseService;
        }

        [HttpGet("{formId}")]
        public async Task<IActionResult> GetForm(Guid formId)
        {
            var form = await _formService.GetByIdAsync(formId);
            return new JsonResult(form, JsonSerializationConstants.SerializerOptions);
        }

        [HttpPut("{formId}")]
        public async Task<IActionResult> PutForm(Guid formId, [FromBody] Form form)
        {
            if (formId != form.Id)
            {
                return BadRequest();
            }

            var updated = await _formService.UpdateAsync(form);
            return new JsonResult(updated, JsonSerializationConstants.SerializerOptions);
        }

        [HttpPost("{formId}/question")]
        public async Task<IActionResult> PostQuestion(Guid formId, [FromBody] Question question)
        {
            var form = await _formService.GetByIdAsync(formId);
            var created = await _formService.AddQuestionAsync(form, question);

            return new JsonResult(created, JsonSerializationConstants.SerializerOptions);
        }

        [HttpGet("{formId}/question/{questionId}")]
        public async Task<IActionResult> GetQuestion(Guid formId, Guid questionId)
        {
            var form = await _formService.GetByIdAsync(formId);
            var question = await _questionService.GetByIdAsync(form, questionId);

            return new JsonResult(question, JsonSerializationConstants.SerializerOptions);
        }

        [HttpDelete("{formId}/question/{questionId}")]
        public async Task<IActionResult> DeleteQuestion(Guid formId, Guid questionId)
        {
            var form = await _formService.GetByIdAsync(formId);
            var question = await _questionService.GetByIdAsync(form, questionId);
            
            await _formService.RemoveQuestionAsync(question);

            return NoContent();
        }

        [HttpPost("{formId}/question/{questionId}/option")]
        public async Task<IActionResult> PostOption(Guid formId, Guid questionId, [FromBody] QuestionOption option)
        {
            var form = await _formService.GetByIdAsync(formId);
            var question = await _questionService.GetByIdAsync(form, questionId);
            var created = await _questionService.AddOption(question, option);

            return new JsonResult(created, JsonSerializationConstants.SerializerOptions);
        }

        [HttpDelete("{formId}/question/{questionId}/option/{optionId}")]
        public async Task<IActionResult> DeleteOption(Guid formId, Guid questionId, Guid optionId)
        {
            var form = await _formService.GetByIdAsync(formId);
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
            var form = await _context.Forms
                .Where(x => x.Id == formId)
                .Include(x => x.Questions)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            var list = answers.ToList();
            var valid = !list.Any(
                answer => answer.QuestionId == Guid.Empty
                          || form.Questions.All(x => x.Id != answer.QuestionId)
            );

            if (!valid)
            {
                return BadRequest("Question identifier is missing");
            }

            var created = await _formService.AddResponseAsync(form, list);
            return new JsonResult(created, JsonSerializationConstants.SerializerOptions);
        }
    }
}