using System;
using System.Collections.Generic;
using System.Linq;
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
    public class FormController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IFormService _formService;
        private readonly IQuestionService _questionService;

        public FormController(ApplicationDbContext context, IFormService formService, IQuestionService questionService)
        {
            _context = context;
            _formService = formService;
            _questionService = questionService;
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

            var updated = await _formService.UpdateValuesAsync(form);

            return new JsonResult(updated, JsonSerializationConstants.SerializerOptions);
        }

        [HttpPost("{formId}/question")]
        public async Task<IActionResult> PostQuestion(Guid formId, [FromBody] Question question)
        {
            var created = await _formService.AddQuestionAsync(formId, question);

            return new JsonResult(created, JsonSerializationConstants.SerializerOptions);
        }

        [HttpDelete("{formId}/question/{questionId}")]
        public async Task<IActionResult> DeleteQuestion(Guid formId, Guid questionId)
        {
            await _formService.RemoveQuestionAsync(formId, questionId);

            return NoContent();
        }

        [HttpPost("{formId}/question/{questionId}/option")]
        public async Task<IActionResult> PostOption(Guid formId, Guid questionId, [FromBody] QuestionOption option)
        {
            var created = await _questionService.AddOption(formId, questionId, option);

            return new JsonResult(created, JsonSerializationConstants.SerializerOptions);
        }

        [HttpDelete("{formId}/question/{questionId}/option/{optionId}")]
        public async Task<IActionResult> DeleteOption(Guid formId, Guid questionId, Guid optionId)
        {
            await _questionService.RemoveOption(formId, questionId, optionId);

            return NoContent();
        }

        [HttpGet("{formId}/response")]
        public async Task<IActionResult> GetResponses(Guid formId)
        {
            var responses = await _formService.ListResponsesAsync(formId);
            
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
            var valid = !list.Any(answer =>
                answer.QuestionId == Guid.Empty
                || form.Questions.All(x => x.Id != answer.QuestionId));

            if (!valid)
            {
                return BadRequest("Question identifier is missing");
            }

            var created = await _formService.AddResponseAsync(formId, list);
            return new JsonResult(created, JsonSerializationConstants.SerializerOptions);
        }
    }
}