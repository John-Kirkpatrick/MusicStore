#region references

using System;

#endregion

namespace Artists.Domain.Application.Extensions
{
    public static class DateExtensions
    {
        #region Public Methods

        public static bool IsWeekend(this DateTime dt)
        {
            return dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday;
        }

        #endregion
    }
}