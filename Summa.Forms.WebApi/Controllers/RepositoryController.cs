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
    public class RepositoryController : ControllerBase
    {
        private readonly IRepositoryService _repositoryService;
        private readonly ICategoryService _categoryService;

        public RepositoryController(IRepositoryService repositoryService, ICategoryService categoryService)
        {
            _repositoryService = repositoryService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetForms()
        {
            var forms = await _repositoryService.ListAsync();

            return new JsonResult(forms, JsonSerializationConstants.SerializerOptions);
        }

        [HttpGet("{guid}")]
        public async Task<IActionResult> GetFormsByCategory(Guid guid)
        {
            var category = await _categoryService.GetFormCategoryByIdAsync(guid);
            var forms = await _repositoryService.ListByCategoryAsync(category);

            return new JsonResult(forms, JsonSerializationConstants.SerializerOptions);
        }
    }
}