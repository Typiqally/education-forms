﻿using System;
using System.Collections.Generic;
 using System.Threading.Tasks;
 using Summa.Forms.Models;

 namespace Summa.Forms.WebApp.Services
{
    public class RepositoryService : IRepositoryService
    {
        public Task<List<RepositoryForm>> ListAsync()
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(Form form)
        {
            throw new NotImplementedException();
        }
    }
}