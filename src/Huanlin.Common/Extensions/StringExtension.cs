using System;
using System.Collections.Generic;
using System.Text;
using Huanlin.Common.Helpers;

namespace Huanlin.Common.Extensions
{
    /// <summary>
    /// String extension methods.
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Reverses a string.
        /// </summary>
        /// <param name = "input">The string to be reversed.</param>
        /// <returns>The reversed string</returns>
        public static string Reverse(this string input)
        {
            if (String.IsNullOrEmpty(input) || (input.Length == 1))
            {
                return input;
            }

            var chars = input.ToCharArray();
            Array.Reverse(chars);
            return new string(chars);
        }

        /// <summary>
        /// Returns the left part of the string.
        /// </summary>
        /// <param name="input">The original string.</param>
        /// <param name="characterCount">The character count to be returned.</param>
        /// <returns>The left part</returns>
        public static string Left(this string input, int characterCount)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            if (characterCount >= input.Length)
                throw new ArgumentOutOfRangeException("characterCount", characterCount, "characterCount must be less than length of string");
            return input.Substring(0, characterCount);
        }

        /// <summary>
        /// Returns the Right part of the string.
        /// </summary>
        /// <param name="input">The original string.</param>
        /// <param name="characterCount">The character count to be returned.</param>
        /// <returns>The right part</returns>
        public static string Right(this string input, int characterCount)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            if (characterCount >= input.Length)
                throw new ArgumentOutOfRangeException("characterCount", characterCount, "characterCount must be less than length of string");
            return input.Substring(input.Length - characterCount);
        }


        public static string EnsureEncloseWith(this string input, string start, string end)
        {
            return input.EnsureStartWith(start).EnsureEndWith(end);
        }

        public static string EnsureStartWith(this string input, string start)
        {
            if (String.IsNullOrEmpty(start))
            {
                throw new Exception($"Argument {nameof(start)} cannot be null or empty string!");
            }
            if (input == null)
                return start;
            if (input.StartsWith(start))
                return input;
            return start + input;
        }

        public static string EnsureEndWith(this string input, string end)
        {
            if (String.IsNullOrEmpty(end))
            {
                throw new Exception($"Argument {nameof(end)} cannot be null or empty string!");
            }

            if (input == null)
                return end;
            if (input.EndsWith(end))
                return input;
            return input + end;
        }

        public static string EnsureEndWithDirectorySeparator(this string input)
        {
            return input.EnsureEndWith(System.IO.Path.DirectorySeparatorChar.ToString());
        }

        public static string EnsureNotStartWith(this string input, string start)
        {
            if (String.IsNullOrEmpty(start))
            {
                throw new Exception($"Argument {nameof(start)} cannot be null or empty string!");
            }

            if (String.IsNullOrEmpty(input))
                return input;
            if (input.StartsWith(start))
            {
                return input.Remove(0, start.Length);
            }
            return input;
        }

        public static string EnsureNotEndWith(this string input, string end)
        {
            if (String.IsNullOrEmpty(end))
            {
                throw new Exception($"Argument {nameof(end)} cannot be null or empty string!");
            }

            if (String.IsNullOrEmpty(input))
                return input;
            if (input.EndsWith(end))
            {
                return input.Substring(0, input.Length - end.Length);
            }
            return input;
        }

        public static string EnsureNotEncloseWith(this string input, string start, string end)
        {
            return input.EnsureNotStartWith(start).EnsureNotEndWith(end);
        }

        #region To X conversions

        /// <summary>
        /// Parses a string into an Enum
        /// </summary>
        /// <typeparam name="T">The type of the Enum</typeparam>
        /// <param name="input">String value to parse</param>
        /// <param name="ignorecase">Ignore the case of the string being parsed</param>
        /// <returns>The Enum corresponding to the stringExtensions</returns>
        public static T ToEnum<T>(this string input, bool ignorecase)
        {
            if (input == null)
                throw new ArgumentNullException("Value");

            input = input.Trim();

            if (input.Length == 0)
                throw new ArgumentNullException("Must specify valid information for parsing in the string.", "value");

            Type t = typeof(T);
            if (!t.IsEnum)
                throw new ArgumentException("Type provided must be an Enum.", "T");

            return (T)Enum.Parse(t, input, ignorecase);
        }

        /// <summary>
        /// Convert to integer.
        /// </summary>
        /// <param name="input">The value.</param>
        /// <param name="defaultvalue">The defaultvalue.</param>
        /// <returns></returns>
        public static int ToInteger(this string input, int defaultvalue) => StrHelper.ToInteger(input, defaultvalue);

        /// <summary>
        /// Toes the double.
        /// </summary>
        /// <param name="input">The value.</param>
        /// <param name="defaultvalue">The defaultvalue.</param>
        /// <returns></returns>
        public static double ToDouble(this string input, double defaultValue)
        {
            return StrHelper.ToDouble(input, defaultValue);
        }

        /// <summary>
        /// Toes the date time.
        /// </summary>
        /// <param name="input">The value.</param>
        /// <param name="defaultValue">The defaultvalue.</param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string input, DateTime? defaultValue)
        {
            return StrHelper.ToDateTime(input, defaultValue);
        }

        /// <summary>
        /// Converts a string value to bool value, supports "T" and "F" conversions.
        /// </summary>
        /// <param name="input">The string value.</param>
        /// <returns>A bool based on the string value</returns>
        public static bool? ToBoolean(this string input)
        {
            return StrHelper.ToBoolean(input);
        }

        #endregion To X conversions

        #region Validation methods

        /// <summary>
        /// Determines whether it is a valid URL.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [is valid URL] [the specified text]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidUrl(this string text)
        {
            System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
            return rx.IsMatch(text);
        }

        /// <summary>
        /// Determines whether it is a valid email address
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [is valid email address] [the specified s]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidEmailAddress(this string email)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(email);
        }

        #endregion Validation methods
    }
}