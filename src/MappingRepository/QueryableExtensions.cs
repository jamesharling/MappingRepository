using System;
using System.Linq;
using System.Linq.Expressions;

namespace MappingRepository
{
    internal static class QueryableExtensions
    {
        internal static IOrderedQueryable<T> OrderBy<T, TKey>(this IQueryable<T> source, Expression<Func<T, TKey>> order, bool isAscending) =>
            isAscending ? source.OrderBy(order) : source.OrderByDescending(order);
    }
}
