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
    public class ResponseController : ControllerBase
    {
        private readonly IResponseService _responseService;

        public ResponseController(IResponseService responseService)
        {
            _responseService = responseService;
        }

        [HttpGet("{responseId}")]
        public async Task<IActionResult> GetResponse(Guid responseId)
        {
            var response = await _responseService.GetByIdAsync(responseId);
            return new JsonResult(response, JsonSerializationConstants.SerializerOptions);
        }
    }
}