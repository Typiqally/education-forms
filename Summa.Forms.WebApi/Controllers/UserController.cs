using System;
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
        private readonly ApplicationDbContext _context;
        private readonly IFormService _formService;

        public UserController(ApplicationDbContext context, IFormService formService)
        {
            _context = context;
            _formService = formService;
        }

        [HttpGet("forms")]
        public async Task<IActionResult> GetForms()
        {
            return new JsonResult(await _formService.ListAsync(), JsonSerializationConstants.SerializerOptions);
        }

        [HttpGet("forms/{guid}")]
        public async Task<IActionResult> GetFormsByCategory(Guid guid)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == guid);

            return new JsonResult(await _formService.ListByCategoryAsync(category), JsonSerializationConstants.SerializerOptions);
        }
    }
}