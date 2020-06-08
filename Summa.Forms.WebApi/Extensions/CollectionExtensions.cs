using System.Collections.Generic;

namespace Summa.Forms.WebApi.Extensions
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> collection, params T[] values)
        {
            foreach (var value in values)
            {
                collection.Add(value);
            }
        }
    }
}