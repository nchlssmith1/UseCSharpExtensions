using System;
using System.Linq;
using System.Text.RegularExpressions;

public static class StringExtensions
{
    public static string RemoveChar(this string value, char[] chars)
    {
        return new string(value.Where(ch => !chars.Contains(ch)).ToArray());
    }

    public static string FormatWith(this string format, params object[] args)
    {
        if (format == null)
            throw new ArgumentNullException("format");

        return string.Format(format, args);
    }

    public static string FormatWith(this string format, IFormatProvider provider, params object[] args)
    {
        if (format == null) 
            throw new ArgumentNullException("format");

        return string.Format(provider, format, args);
    }

    public static string Or(this string value, string defaultValue)
    {
        if (!string.IsNullOrEmpty(value)) return value;
        else return defaultValue;
    }

    public static bool IsEmpty(this string value)
    {
        return string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// Shortcut to determine if a string is null or empty
    /// </summary>
    /// <returns></returns>
    public static bool IsNullOrEmpty(this string value)
    {
        return string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// Shortcut to determine if a string is not empty
    /// </summary>
    /// <returns></returns>
    public static bool IsNotEmpty(this string value)
    {
        return !string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// Shortcut to determine if a string is not null or empty
    /// </summary>
    /// <returns></returns>
    public static bool IsNotNullOrEmpty(this string value)
    {
        return !string.IsNullOrEmpty(value);
    }

    public static string Mask(this string str, int unmaskedCharCount = 4, char mask = '*')
    {
        if (str.Length <= unmaskedCharCount) return str;
        return str.Substring(str.Length - unmaskedCharCount).PadLeft(str.Length, mask);
    }

    private static string RegexReplace(this string str, string regex)
    {
        if (str.IsNullOrEmpty()) return "";
        return new Regex(regex).Replace(str, "");
    }
    public static bool IsRegexMatch(this string str, string regex)
    {
        return new Regex(regex).IsMatch(str);
    }

    public static string RandomString(this string str, int length)
    {
        return ((str.IsNullOrEmpty())?"{0}": str).FormatWith(RandomString(length));
    }

    private static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }

}