#region references

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace Domain.Application.Extensions
{
    /// <summary>
    ///     Contains extension methods for working with IQueryables.
    /// </summary>
    public static class QueryableExtensions
    {
        #region Public Methods

        /// <summary>
        ///     Determines whether the IQueryable has been ordered.
        /// </summary>
        /// <typeparam name="T">The entity type of the IQueryable.</typeparam>
        /// <param name="queryable">The IQueryable to check for ordering.</param>
        /// <returns>True if ordered; false otherwise.</returns>
        public static bool IsOrdered<T>(this IQueryable<T> queryable)
        {
            // https://stackoverflow.com/a/31252271/1778164
            return queryable.Expression.Type == typeof(IOrderedQueryable<T>);
        }

        /// <summary>
        ///     Orders the query on the specified property name in ascending order.
        /// </summary>
        /// <typeparam name="T">The entity type of the query to sort.</typeparam>
        /// <param name="query">The query to sort.</param>
        /// <param name="propertyName">The name of the property on which to sort.</param>
        /// <param name="defaultPropertyExpression">
        ///     The default sort expression to use if the property name
        ///     is not specified or is not found.
        /// </param>
        /// <returns>The query sorted on the specified property in ascending order.</returns>
        public static IQueryable<T> OrderByPropertyNameAscending<T>(
            this IQueryable<T> query,
            string propertyName,
            Expression<Func<T, object>> defaultPropertyExpression = null
        ) where T : class
        {
            Expression<Func<T, object>> sortExpression =
                GetPropertyExpression<T>(propertyName) ?? defaultPropertyExpression;

            if (sortExpression != null)
            {
                query = query.OrderBy(sortExpression);
            }

            return query;
        }

        /// <summary>
        ///     Orders the query on the specified property name in descending order.
        /// </summary>
        /// <typeparam name="T">The entity type of the query to sort.</typeparam>
        /// <param name="query">The query to sort.</param>
        /// <param name="propertyName">The name of the property on which to sort.</param>
        /// <param name="defaultPropertyExpression">
        ///     The default sort expression to use if the property name
        ///     is not specified or is not found.
        /// </param>
        /// <returns>The query sorted on the specified property in descending order.</returns>
        public static IQueryable<T> OrderByPropertyNameDescending<T>(
            this IQueryable<T> query,
            string propertyName,
            Expression<Func<T, object>> defaultPropertyExpression = null
        ) where T : class
        {
            Expression<Func<T, object>> sortExpression =
                GetPropertyExpression<T>(propertyName) ?? defaultPropertyExpression;

            if (sortExpression != null)
            {
                query = query.OrderByDescending(sortExpression);
            }

            return query;
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///     Builds an expression that represents access of the specified property on the specified type.
        /// </summary>
        /// <typeparam name="T">The class type represented by the returned expression.</typeparam>
        /// <param name="propertyName">The property represented by the returned expression.</param>
        /// <returns>An expression that represents access of the specified property on the specified type.</returns>
        private static Expression<Func<T, object>> GetPropertyExpression<T>(string propertyName) where T : class
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return null;
            }

            Expression<Func<T, object>> propertyExpression = null;

            PropertyInfo[] properties = typeof(T).GetProperties();
            PropertyInfo targetProperty = properties.FirstOrDefault(p => p.Name.ToLower() == propertyName.ToLower());

            if (targetProperty != null)
            {
                ParameterExpression paramExpression = Expression.Parameter(typeof(T));
                MemberExpression memberExpression = Expression.Property(paramExpression, targetProperty);
                Expression memberAsObjectExpression = Expression.Convert(memberExpression, typeof(object));

                propertyExpression = Expression.Lambda<Func<T, object>>(memberAsObjectExpression, paramExpression);
            }

            return propertyExpression;
        }

        #endregion
    }
}