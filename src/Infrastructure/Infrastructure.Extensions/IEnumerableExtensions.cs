#region references

using System.Collections.Generic;
using System.Linq;

#endregion

namespace Infrastructure.Extensions
{
    public static class IEnumerableExtensions
    {
        #region Public Methods

        public static bool IsLast<T>(this IEnumerable<T> items, T item)
        {
            T last = items.LastOrDefault();
            return last != null && item.Equals(last);
        }

        public static bool IsFirst<T>(this IEnumerable<T> items, T item)
        {
            T first = items.FirstOrDefault();
            return first != null && item.Equals(first);
        }

        public static bool IsFirstOrLast<T>(this IEnumerable<T> items, T item)
        {
            return items.IsFirst(item) || items.IsLast(item);
        }

        #endregion
    }
}