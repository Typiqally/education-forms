﻿using System;
using System.Collections.Generic;
 using System.Threading.Tasks;
 using Summa.Forms.Models;
 using Summa.Forms.WebApp.Json;

 namespace Summa.Forms.WebApp.Services
{
    public class RepositoryProxyService : IRepositoryProxyService
    {
        public Task<JsonHttpResponseMessage<List<RepositoryForm>>> ListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<JsonHttpResponseMessage<List<RepositoryForm>>> ListByCategoryAsync(FormCategory category)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(Form form)
        {
            throw new NotImplementedException();
        }
    }
}