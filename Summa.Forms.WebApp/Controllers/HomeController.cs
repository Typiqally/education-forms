using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Summa.Forms.WebApp.Models;
using Summa.Forms.WebApp.Services;

namespace Summa.Forms.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFormProxyService _formProxyService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IFormProxyService formProxyService, ILogger<HomeController> logger)
        {
            _formProxyService = formProxyService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Forms()
        {
            var response = await _formProxyService.ListAsync();
            return View(response.Data);
        }

        public IActionResult Library()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}