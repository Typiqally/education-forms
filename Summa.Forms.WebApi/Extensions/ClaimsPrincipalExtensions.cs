using System.Security.Claims;

namespace Summa.Forms.WebApi.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetSubject(this ClaimsPrincipal claimsPrincipal)
        {
            return "025CF29C-AA1D-4793-AC18-FF4B2334DA4F";
            //return claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}