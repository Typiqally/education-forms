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
            var category = await _categoryService.GetByIdAsync(guid);
            var forms = await _formService.ListByCategoryAsync(category);

            return new JsonResult(forms, JsonSerializationConstants.SerializerOptions);
        }
    }
}