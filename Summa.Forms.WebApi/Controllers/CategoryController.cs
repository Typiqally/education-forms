using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Summa.Forms.WebApi.Services;

namespace Summa.Forms.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }
        
        [HttpGet("form")]
        public async Task<IActionResult> GetFormCategories()
        {
            var categories = await _service.ListFormCategoriesAsync();

            return new JsonResult(categories, JsonSerializationConstants.SerializerOptions);
        }
    }
}