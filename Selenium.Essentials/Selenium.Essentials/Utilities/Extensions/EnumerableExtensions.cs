using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Essentials.Utilities.Extensions
{
    public static class EnumerableExtensions
    {
        [DebuggerStepThrough]
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> items) => items ?? Enumerable.Empty<T>();

        /// <summary>
        /// Iterates the sequence and calls the given action with the item
        /// </summary>
        [DebuggerStepThrough]
        public static void Iter<TItem>(this IEnumerable<TItem> items, Action<TItem> action)
        {
            items.Iteri((item, _) => action(item));
        }

        /// <summary>
        /// Iterates the sequence and calls the given action with the item and its index in the sequence
        /// </summary>
        [DebuggerStepThrough]
        public static void Iteri<TItem>(this IEnumerable<TItem> items, Action<TItem, int> action)
        {
            var index = 0;
            foreach (var item in items.EmptyIfNull())
                action(item, index++);
        }

        public static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> source, string propertyName, string value)
        {
            Expression<Func<TEntity, bool>> whereExpression = x => x.GetType().InvokeMember(propertyName, BindingFlags.GetProperty, null, x, null).EmptyIfNull().IndexOf(value, StringComparison.InvariantCultureIgnoreCase) >= 0;

            return source.Where(whereExpression);
        }

        public static IQueryable<TEntity> WhereHasValue<TEntity>(this IQueryable<TEntity> source, string propertyName)
        {
            Expression<Func<TEntity, bool>> whereExpression = x => x.GetType().InvokeMember(propertyName, BindingFlags.GetProperty, null, x, null).EmptyIfNull().HasValue();

            return source.Where(whereExpression);
        }
    }
}
