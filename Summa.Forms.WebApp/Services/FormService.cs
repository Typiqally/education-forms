using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Summa.Forms.Models;
using Summa.Forms.WebApp.Extensions;

namespace Summa.Forms.WebApp.Services
{
    public class FormService : IFormService
    {
        private readonly HttpClient _http;

        public FormService(HttpClient http)
        {
            _http = http;
        }

        public async Task<Form> GetByIdAsync(Guid guid)
        {
            return await SendRequestAsync<Form>($"/User/Forms/{guid.ToString()}");
        }

        public async Task<List<Form>> ListAsync()
        {
            return await SendRequestAsync<List<Form>>("/User/Forms");
        }

        public async Task<List<Form>> ListByCategoryAsync(FormCategory category)
        {
            return await SendRequestAsync<List<Form>>($"/User/Forms/Category/{category.Id}");
        }

        public async Task AddQuestionAsync(Form form, Question question)
        {
            throw new NotImplementedException();
        }

        private async Task<T> SendRequestAsync<T>(string uri)
        {
            var response = await _http.SendAsync<T>(uri);

            if (!response.Message.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Could not fetch user forms");
            }

            return response.Data;
        }
    }
}