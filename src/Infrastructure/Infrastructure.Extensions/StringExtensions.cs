#region references

using System.Collections.Generic;
using System.Linq;

#endregion

namespace Infrastructure.Extensions
{
    public static class StringExtensions {
        #region Public Methods

        public static bool IsNumeric(
            this string value
        ) {
            return value.All(char.IsNumber);
        }

        public static string GetCommaDelimitedList(
            this IEnumerable<string> stringList
        ) {
            string retVal = string.Empty;

            IEnumerable<string> enumerable = stringList.ToList();

            return enumerable.Aggregate(
                retVal, (current, str ) => current + str + (str == enumerable.Last() ? string.Empty : ", ")
            );
        }

        #endregion
    }
}