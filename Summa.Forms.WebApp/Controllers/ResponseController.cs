using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Summa.Forms.WebApp.Services;

namespace Summa.Forms.WebApp.Controllers
{
	[Authorize]
	[Route("[controller]")]
	public class ResponseController : Controller
	{
		private readonly IResponseProxyService _responseProxyService;
		
		public ResponseController(IResponseProxyService responseProxyService)
		{
			_responseProxyService = responseProxyService;
		}
		
		[HttpGet("{responseId}")]
		public async Task<IActionResult> Index(Guid responseId)
		{
			var response = await _responseProxyService.GetByIdAsync(responseId);
			if (!response.Message.IsSuccessStatusCode)
			{
				return StatusCode((int) response.Message.StatusCode, response.Message.Content);
			}

			return View(response.Data);
		}
	}
}