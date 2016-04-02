using System;
using System.Collections.Generic;
using System.Linq;


public static class DateTimeExtensions
{
    public static DateTime Next(this DateTime date, DayOfWeek dayOfWeek)
    {
        return date.AddDays((dayOfWeek < date.DayOfWeek ? 7 : 0) + dayOfWeek - date.DayOfWeek);
    }

    public static DateTime Previous(this DateTime date, DayOfWeek dayOfWeek)
    {
        return date.AddDays(-7).Next(dayOfWeek);
    }

    public static int GetDayOfWeekOccurrence(this DateTime date, DayOfWeek dayOfWeek)
    {
        return Math.Max(GetMonthDates(date).Where(c => c.DayOfWeek == dayOfWeek)
            .Count(c => c <= date), 1);
    }

    public static DateTime GetNthDayOfWeek(this DateTime date, int nthWeek, DayOfWeek dayOfWeek)
    {
        return date.BeginningOfMonth().Next(dayOfWeek).AddWeeks(nthWeek - 1);
    }

    public static DateTime AddWeeks(this DateTime date, int weeks)
    {
        return date.AddDays(weeks* 7);
    }

    public static DateTime BeginningOfHour(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0, 0, date.Kind);
    }

    public static DateTime EndOfHour(this DateTime date)
    {
        return date.BeginningOfHour().AddHours(1).AddSeconds(-1).AddTicks(-1);
    }

    public static DateTime BeginningOfDay(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0, date.Kind);
    }

    public static DateTime EndOfDay(this DateTime date)
    {
        return date.BeginningOfDay().AddDays(1).AddTicks(-1);
    }

    public static DateTime BeginningOfMonth(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, 1, 0, 0, 0, 0, date.Kind);
    }

    public static DateTime EndOfMonth(this DateTime date)
    {
        return date.BeginningOfMonth().AddMonths(1).AddTicks(-1);
    }

    public static DateTime BeginningOfYear(this DateTime date)
    {
        return new DateTime(date.Year, 1, 1, 0, 0, 0, 0, date.Kind);
    }

    public static DateTime EndOfYear(this DateTime date)
    {
        return date.BeginningOfYear().AddYears(1).AddTicks(-1);
    }

    public static bool IsWeekend(this DateTime date)
    {
        return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
    }

    public static List<DateTime> GetMonthDates(DateTime date)
    {
        return GetMonthDates(date.Month, date.Year);
    }

    public static List<DateTime> GetMonthDates(int month, int year)
    {
        var dates = new List<DateTime>();

        var firstOfMonth = new DateTime(year, month, 1);

        var currentDay = firstOfMonth;
        while (firstOfMonth.Month == currentDay.Month)
        {
            dates.Add(currentDay);
            currentDay = currentDay.AddDays(1);
        }

        return dates;
    }
}