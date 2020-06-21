﻿using System.Threading.Tasks;

 namespace Summa.Forms.WebApi.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
