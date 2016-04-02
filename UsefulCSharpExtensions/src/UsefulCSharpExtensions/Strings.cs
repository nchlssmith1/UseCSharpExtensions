using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

public static class StringExtensions
{
    /// <summary>
    /// Determine whether a string can be an email or not
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool IsEmail(this string value)
    {
        if (String.IsNullOrEmpty(value))
            return false;

        // Use IdnMapping class to convert Unicode domain names.
        try
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();
            string[] match = value.Split('@');

            string domainName = "";
            if (match.Count() == 2)
                domainName = match[1];
            else
                return false;

            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                return false;
            }
            domainName = "{0}@{1}".FormatWith(match[1], domainName);

            value = Regex.Replace(value, @"(@)(.+)$", domainName,
                                  RegexOptions.None, TimeSpan.FromMilliseconds(200));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }

        // Return true if strIn is in valid e-mail format.
        try
        {
            return Regex.IsMatch(value,
                  @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                  @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                  RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }
    public static string Hash(this string value)
    {

        StringBuilder sb = new StringBuilder();
        foreach (byte b in SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(value)))
            sb.Append(b.ToString("X2"));

        return sb.ToString();
    }
    public static string RemoveChar(this string value, char[] chars)
    {
        return new string(value.Where(ch => !chars.Contains(ch)).ToArray());
    }
    /// <summary>
    /// A shortcut call for string.Format(), it formats strings.
    /// </summary>
    /// <param name="format">The format of the string</param>
    /// <param name="args">The arguments to merge into the provided format</param>
    /// <returns>The formatted, merged string.</returns>ttempted
    public static string AppendWith(this string str, string append, params object[] arg)
    {
        if (str == null)
            throw new ArgumentNullException("String To Append");

        return string.Join(append, arg.AsEnumerable().Concat(new string[] { str }.AsEnumerable()).Select(x => "{0}".FormatWith(x)).ToList());
    }
    /// <summary>
    /// A shortcut call for string.Format(), it formats strings.
    /// </summary>
    /// <param name="format">The format of the string</param>
    /// <param name="args">The arguments to merge into the provided format</param>
    /// <returns>The formatted, merged string.</returns>
    public static string FormatWith(this string format, params object[] args)
    {
        if (format == null)
            throw new ArgumentNullException("format");

        return string.Format(format, args);
    }

    /// <summary>
    /// A shortcut call for string.Format(), it formats strings. 
    /// </summary>
    /// <param name="provider">The format provider to use</param>
    /// <param name="format">The format of the string</param>
    /// <param name="args">The arguments to merge into the provided format</param>
    /// <returns>The formatted, merged string.</returns>
    public static string FormatWith(this string format, IFormatProvider provider, params object[] args)
    {
        if (format == null) 
            throw new ArgumentNullException("format");

        return string.Format(provider, format, args);
    }

    /// <summary>
    /// Return either the provided value, or the provided default value
    /// </summary>
    /// <param name="value"></param>
    /// <param name="fallback"></param>
    /// <returns></returns>
    public static string Or(this string value, string defaultValue)
    {
        if (!string.IsNullOrEmpty(value)) return value;
        else return defaultValue;
    }

    /// <summary>
    /// Shortcut to determine if a string is empty
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Parse the provided decimal as the currency of the provided culture
    /// </summary>
    /// <param name="value">The amount</param>
    /// <param name="cultureName">The culture code to use when formatting</param>
    /// <returns>The formatted string result</returns>
    public static string ToCurrency(this decimal value, string cultureName)
    {
        CultureInfo currentCulture = new CultureInfo(cultureName);
        return (string.Format(currentCulture, "{0:C}", value));
    }

    /// <summary>
    /// Masks the string with the provided mask (default: '*'), leaving a provided number of unmasked characters remaining (default: 4).
    /// </summary>
    /// <param name="unmaskedCharCount">The number of characters to leave unmasked. Defaults to 4.</param>
    /// <param name="mask">The character used as the mask</param>
    /// <returns>The masked string, or the original string if there were not enough characters to leave unmasked.</returns>
    public static string Mask(this string str, int unmaskedCharCount = 4, char mask = '*')
    {
        if (str.Length <= unmaskedCharCount) return str;
        return str.Substring(str.Length - unmaskedCharCount).PadLeft(str.Length, mask);
    }
    
    public static string RemoveSpecialChars(this string str)
    {
        return str.RegexReplace("[^a-zA-Z0-9]");
    }
    
    public static int RemoveAlphaChars(this string str)
    {
        try { return  Convert.ToInt32(str.RegexReplace("[^0-9]")); }
        catch { return 0; }
    }

    public static int ConvertToInt(this string str)
    {
        try { return  Convert.ToInt32(str.RegexReplace("[^0-9]")); }
        catch { return 0; }
    }

    public static bool IsWebUrl(this string str )
    {
        Uri uriResult;
        return Uri.TryCreate(str, UriKind.Absolute, out uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
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