﻿using System.Threading.Tasks;

 namespace Summa.Forms.Identity.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
