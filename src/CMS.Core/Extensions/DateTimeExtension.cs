using System;

namespace CMS.Core.Extensions;

public static class DateTimeExtension
{
    /// <summary>
    /// return the start date in month of given date
    /// </summary>
    /// <param name="date">the date</param>
    /// <returns>the start date</returns>
    public static DateTime ToStartOfMonth(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, 1);
    }

    /// <summary>
    /// return the end date in month of given date
    /// </summary>
    /// <param name="date">the date</param>
    /// <returns>the end date</returns>
    public static DateTime ToEndOfMonth(this DateTime date)
    {
        return date.ToStartOfMonth().AddMonths(1).AddDays(-1);
    }

    /// <summary>
    /// Compare 2 date without second compare
    /// </summary>
    /// <param name="dleft"></param>
    /// <param name="dright"></param>
    /// <returns></returns>
    public static bool EqualWithoutSecond(this DateTime? dleft, DateTime? dright)
    {
        if (dleft == null && dright == null)
        {
            return true;
        }

        if ((dleft.HasValue && dright == null) || (dleft == null && dright.HasValue))
        {
            return false;
        }

        var left = dleft.Value;
        var right = dright.Value;

        return left.Date == right.Date &&
               left.Hour == right.Hour && left.Minute == right.Minute;
    }

    public static bool EqualDate(this DateTime? left, DateTime? right)
    {
        return left?.Date == right?.Date;
    }

    public static DateTime SetTimeSpan(this DateTime date, TimeSpan time)
    {
        return new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds);
    }
}
