using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

namespace Core.LINQ.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Include<T> (this IQueryable<T> sequence, string path) {
            var objectQuery = sequence as ObjectQuery<T>;
            if (objectQuery != null) {
                return objectQuery.Include(path);
            }
            return sequence;
        }
    }
}