﻿using System.Security.Claims;

namespace Summa.Forms.WebApi.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetSubject(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}