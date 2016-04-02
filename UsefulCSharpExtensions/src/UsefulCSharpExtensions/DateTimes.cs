using System;
using System.Collections.Generic;
using System.Linq;


public static class DateTimeExtensions
{
    /// <summary>
    /// Gets the first week day following a date.
    /// </summary> 
    /// <param name="date">The date.</param> 
    /// <param name="dayOfWeek">The day of week to return.</param> 
    /// <returns>The first dayOfWeek day following date, or date if it is on dayOfWeek.</returns> 
    public static DateTime Next(this DateTime date, DayOfWeek dayOfWeek)
    {
        return date.AddDays((dayOfWeek < date.DayOfWeek ? 7 : 0) + dayOfWeek - date.DayOfWeek);
    }

    /// <summary>
    /// Gets the first week day preceding a date.
    /// </summary> 
    /// <param name="date">The date.</param> 
    /// <param name="dayOfWeek">The day of week to return.</param> 
    /// <returns>The first dayOfWeek day preceding the date.</returns> 
    public static DateTime Previous(this DateTime date, DayOfWeek dayOfWeek)
    {
        return date.AddDays(-7).Next(dayOfWeek);
    }

    /// <summary>
    /// Determine which occurrence of the provided day of the week is the provided date.
    /// </summary> 
    /// <param name="date">The date.</param> 
    /// <param name="dayOfWeek">The day of week to return.</param> 
    /// <returns>The first dayOfWeek day following date, or date if it is on dayOfWeek.</returns> 
    public static int GetDayOfWeekOccurrence(this DateTime date, DayOfWeek dayOfWeek)
    {
        return Math.Max(GetMonthDates(date).Where(c => c.DayOfWeek == dayOfWeek)
            .Count(c => c <= date), 1);
    }

    /// <summary>
    /// Get the nth day of the week for the provided month.
    /// </summary> 
    /// <param name="date">The date.</param> 
    /// <param name="nthWeek">The occurrence of the provided day of the week in the provided month.</param> 
    /// <param name="dayOfWeek">The day of week.</param> 
    /// <returns>The date of the nth day of the week in the provided month.</returns> 
    public static DateTime GetNthDayOfWeek(this DateTime date, int nthWeek, DayOfWeek dayOfWeek)
    {
        return date.BeginningOfMonth().Next(dayOfWeek).AddWeeks(nthWeek - 1);
    }

    /// <summary>
    /// Add weeks to the provided date.
    /// </summary> 
    /// <param name="date">The date.</param> 
    /// <param name="weeks">The number of weeks to add.</param> 
    /// <returns>The first dayOfWeek day following date, or date if it is on dayOfWeek.</returns> 
    public static DateTime AddWeeks(this DateTime date, int weeks)
    {
        return date.AddDays(weeks* 7);
    }

    /// <summary>
    /// Get a DateTime that represents the beginning of the hour of the provided date.
    /// </summary>
    /// <param name="date">The date</param>
    /// <returns></returns>
    public static DateTime BeginningOfHour(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0, 0, date.Kind);
    }

    /// <summary>
    /// Get a DateTime that represents the end of the hour of the provided date.
    /// </summary>
    /// <param name="date">The date</param>
    /// <returns></returns>
    public static DateTime EndOfHour(this DateTime date)
    {
        return date.BeginningOfHour().AddHours(1).AddSeconds(-1).AddTicks(-1);
    }

    /// <summary>
    /// Get a DateTime that represents the beginning of the day of the provided date.
    /// </summary>
    /// <param name="date">The date</param>
    /// <returns></returns>
    public static DateTime BeginningOfDay(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0, date.Kind);
    }

    /// <summary>
    /// Get a DateTime that represents the end of the day of the provided date.
    /// </summary>
    /// <param name="date">The date</param>
    /// <returns></returns>
    public static DateTime EndOfDay(this DateTime date)
    {
        return date.BeginningOfDay().AddDays(1).AddTicks(-1);
    }

    /// <summary>
    /// Get a DateTime that represents the beginning of the month of the provided date.
    /// </summary>
    /// <param name="date">The date</param>
    /// <returns></returns>
    public static DateTime BeginningOfMonth(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, 1, 0, 0, 0, 0, date.Kind);
    }

    /// <summary>
    /// Get a DateTime that represents the end of the month of the provided date.
    /// </summary>
    /// <param name="date">The date</param>
    /// <returns></returns>
    public static DateTime EndOfMonth(this DateTime date)
    {
        return date.BeginningOfMonth().AddMonths(1).AddTicks(-1);
    }

    /// <summary>
    /// Get a DateTime that represents the beginning of the year of the provided date.
    /// </summary>
    /// <param name="date">The date</param>
    /// <returns></returns>
    public static DateTime BeginningOfYear(this DateTime date)
    {
        return new DateTime(date.Year, 1, 1, 0, 0, 0, 0, date.Kind);
    }

    /// <summary>
    /// Get a DateTime that represents the end of the year of the provided date.
    /// </summary>
    /// <param name="date">The date</param>
    /// <returns></returns>
    public static DateTime EndOfYear(this DateTime date)
    {
        return date.BeginningOfYear().AddYears(1).AddTicks(-1);
    }

    /// <summary>
    /// Determines if the provided datetime is a weekend.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static bool IsWeekend(this DateTime date)
    {
        return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
    }

    /// <summary>
    /// Get the dates in the provided month as a list.
    /// </summary>
    /// <param name="date">The date in the desired month and year.</param>
    /// <returns>A list of the dates in the provided month.</returns>
    public static List<DateTime> GetMonthDates(DateTime date)
    {
        return GetMonthDates(date.Month, date.Year);
    }

    /// <summary>
    /// Get the dates in the provided month and year as a list.
    /// </summary>
    /// <param name="month">The desired month.</param>
    /// <param name="year">The desired year.</param>
    /// <returns>A list of the dates in the provided month and year.</returns>
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