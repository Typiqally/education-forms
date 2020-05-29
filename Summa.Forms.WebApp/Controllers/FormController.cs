using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Summa.Forms.WebApp.Services;

namespace Summa.Forms.WebApp.Controllers
{
    public class FormController : Controller
    {
        private readonly IFormService _formService;

        public FormController(IFormService formService)
        {
            _formService = formService;
        }

        // GET
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            return View(await _formService.GetByIdAsync(id));
        }
    }
}