using System;

namespace Summa.Forms.WebApi.Extensions
{
    public static class GuidExtensions
    {
        public static Guid AsGuid(this string s)
        {
            return Guid.Parse(s);
        }
    }
}