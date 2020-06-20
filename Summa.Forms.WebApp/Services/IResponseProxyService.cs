using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Summa.Forms.Models;
using Summa.Forms.WebApp.Json;

namespace Summa.Forms.WebApp.Services
{
	public interface IResponseProxyService
	{
		Task<JsonHttpResponseMessage<FormResponse>> GetByIdAsync(Guid responseId);
		Task<JsonHttpResponseMessage<List<FormResponse>>> ListAsync();
		Task<JsonHttpResponseMessage<List<FormResponse>>> ListAsync(Guid formId);
	}
}