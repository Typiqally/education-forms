using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Summa.Forms.WebApi.Services;

namespace Summa.Forms.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IFormService _formService;
        private readonly ICategoryService _categoryService;
        private readonly IResponseService _responseService;

        public UserController(IFormService formService, ICategoryService categoryService, IResponseService responseService)
        {
            _formService = formService;
            _categoryService = categoryService;
            _responseService = responseService;
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
            var forms = await _formService.ListAsync(category);

            return new JsonResult(forms, JsonSerializationConstants.SerializerOptions);
        }

        [HttpGet("responses")]
        public async Task<IActionResult> GetResponses()
        {
            var responses = await _responseService.ListAsync();
            return new JsonResult(responses, JsonSerializationConstants.SerializerOptions);
        }
        
        [HttpGet("responses/{guid}")]
        public async Task<IActionResult> GetResponsesByForm(Guid guid)
        {
            var form = await _formService.GetByIdAsync(guid);
            var responses = await _responseService.ListAsync(form);

            return new JsonResult(responses, JsonSerializationConstants.SerializerOptions);
        }
    }
}