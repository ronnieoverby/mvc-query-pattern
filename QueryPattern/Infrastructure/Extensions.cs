using System.Collections.Generic;
using System.Linq;

namespace QueryPattern.Infrastructure
{
    public static class Extensions
    {
      public static IQueryable<T> Page<T>(this IQueryable<T> source, int page, int pageSize)
        {
            var skip = CalculateSkip(page, pageSize);
            return source.Skip(skip).Take(pageSize);
        }

        public static IEnumerable<T> Page<T>(this IEnumerable<T> source, int page, int pageSize)
        {
            var skip = CalculateSkip(page, pageSize);
            return source.Skip(skip).Take(pageSize);
        }

        private static int CalculateSkip(int page, int pageSize)
        {
            return page*pageSize - pageSize;
        }
    }
}