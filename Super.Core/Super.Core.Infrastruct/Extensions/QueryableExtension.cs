using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Super.Core.Infrastruct.Extensions
{
    public static class QueryableExtension
    {
        public static IQueryable<T> Paging<T>(this IOrderedQueryable<T> exp, int pageIndex, int pageSize)
        {
            return exp.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public static IQueryable<T> Paging<T, K>(this IQueryable<T> exp, Expression<Func<T, K>> orderFiled, int pageIndex, int pageSize, bool isAsc)
        {
            if (isAsc)
            {
                return exp.OrderBy(orderFiled).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            else
            {
                return exp.OrderByDescending(orderFiled).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
        }
    }
}
