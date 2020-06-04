using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetForms(Guid formId)
        {
            return new JsonResult(await _formService.GetByIdAsync(formId), JsonSerializationConstants.SerializerOptions);
        }

        [HttpPost("{formId}/question/{questionId}/option")]
        public async Task<IActionResult> AddOption(Guid formId, Guid questionId, [FromBody] QuestionOption option)
        {
           var newOption = await _questionService.AddOption(formId, questionId, option);

            return new JsonResult(newOption, JsonSerializationConstants.SerializerOptions);
        }
        
        [HttpDelete("{formId}/question/{questionId}/option/{optionId}")]
        public async Task<IActionResult> DeleteOption(Guid formId, Guid questionId, Guid optionId)
        {
            await _questionService.RemoveOption(formId, questionId, optionId);

            return NoContent();
        }

        [HttpPut("{formId}")]
        public async Task<IActionResult> PutForm(Guid formId, [FromBody] Form form)
        {
            if (formId != form.Id)
            {
                return BadRequest();
            }

            await _formService.UpdateValuesAsync(form);

            return NoContent();
        }
    }
}