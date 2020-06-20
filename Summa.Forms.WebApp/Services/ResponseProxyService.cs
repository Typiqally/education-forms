using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Summa.Forms.Models;
using Summa.Forms.WebApp.Extensions;
using Summa.Forms.WebApp.Json;

namespace Summa.Forms.WebApp.Services
{
	public class ResponseProxyService : IResponseProxyService
	{
		private readonly HttpClient _http;
		private readonly IHttpContextAccessor _httpContextAccessor;
		
		public ResponseProxyService(HttpClient http, IHttpContextAccessor httpContextAccessor)
		{
			_http = http;
			_httpContextAccessor = httpContextAccessor;
		}
		
		public async Task<JsonHttpResponseMessage<FormResponse>> GetByIdAsync(Guid responseId)
		{
			var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
			var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"/response/{responseId}")
				.SetBearerToken(token);

			return await _http.SendAsync<FormResponse>(requestMessage);
		}

		public Task<JsonHttpResponseMessage<List<FormResponse>>> ListAsync()
		{
			throw new NotImplementedException();
		}

		public Task<JsonHttpResponseMessage<List<FormResponse>>> ListAsync(Guid formId)
		{
			throw new NotImplementedException();
		}
	}
}