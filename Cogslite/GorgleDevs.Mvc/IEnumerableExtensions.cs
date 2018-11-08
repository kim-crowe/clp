using System;
using System.Collections.Generic;
using System.Linq;

namespace GorgleDevs.Mvc
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<TResult> SelectPage<TSource, TResult>(this IEnumerable<TSource> items, int page, int pageSize, Func<TSource, TResult> selector)        
        {
            return 
                items
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .Select(selector);
        }
    }
}