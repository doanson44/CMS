using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CMS.Core.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// Remove unicode character (unicode) of given string
        /// </summary>
        /// <param name="input">The input string to remove mark</param>
        /// <returns>The string without mark</returns>
        public static string RemoveMark(this string input)
        {
            var regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            var formD = input.Normalize(NormalizationForm.FormD);

            return regex.Replace(formD, string.Empty)
                .Replace('\u0111', 'd')
                .Replace('\u0110', 'D');
        }

        public static string Standardized(this string input, string delimiter = null)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            if (delimiter == null)
                delimiter = string.Empty;

            var slug = input.RemoveMark().ToLower();

            // remove invalid chars
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", string.Empty);

            // convert multiple spaces into one
            slug = Regex.Replace(slug, "\\s+", " ").Trim();
            slug = Regex.Replace(slug, "\\s", delimiter);
            slug = Regex.Replace(slug, "-+", delimiter);

            return slug;
        }

        /// <summary>
        /// Convert a string into friendly url form with hiphen between words
        /// </summary>
        /// <param name="input">The input string</param>
        /// <param name="length">The maximum length of result</param>
        /// <returns>The string result</returns>
        public static string ToFriendlyUrl(this string input, int length = 125)
        {
            return input.Standardized("-")
                .Trim('-')
                .Truncate(length, string.Empty);
        }

        /// <summary>
        /// Cut given string for shorter string
        /// </summary>
        /// <param name="s">Given string to cut</param>
        /// <param name="length">Max length to display</param>
        /// <param name="ellipsis">The end string if over length</param>
        /// <returns>The cutted string</returns>
        public static string Truncate(this string s, int length = 50, string ellipsis = "...")
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            if (s.Length <= length)
                return s;

            return s.Substring(0, length).Trim() + ellipsis;
        }

        /// <summary>
        /// If the input string is null or whitespace, return the fallback value
        /// </summary>
        /// <param name="source">The source string</param>
        /// <param name="fallback">The fallback value</param>
        /// <returns>The final result</returns>
        public static string UseFallback(this string source, string fallback)
        {
            if (!string.IsNullOrWhiteSpace(source))
            {
                return source;
            }

            return fallback;
        }

        /// <summary>
        /// Join the collection of string into single separate by the specify separator
        /// </summary>
        /// <param name="list">The source strings</param>
        /// <param name="separator">The separator</param>
        /// <returns>The result string</returns>
        public static string JoinLines(this IEnumerable<string> list, string separator = "\r\n")
        {
            if (list?.Any() != true)
            {
                return string.Empty;
            }

            return string.Join(separator, list);
        }

        public static string SafeReplace(this string input, string find, string replace)
        {
            var textToFind = string.Format(@"\b{0}\b", find);
            return Regex.Replace(input, textToFind, replace);
        }

        public static string FormatUserName(this string userName, IEnumerable<string> unsupportedCharacters)
        {
            return string.Concat(userName.Split(unsupportedCharacters.ToArray(), StringSplitOptions.RemoveEmptyEntries));
        }

        public static string RemoveSpace(string input)
        {
            return Regex.Replace(input, @"\s+", "");
        }

        public static string FormatLastName(this string input)
        {
            return new string(input.Select((ch, index) => (index == 0) ? ch : char.ToLower(ch)).ToArray());
        }

        public static string BuildSearchTerm(this string searchText)
        {
            searchText = string.IsNullOrEmpty(searchText) ? string.Empty : searchText;

            var searchTerm = searchText.Trim().Split(" ").Where(x => !string.IsNullOrEmpty(x));
            searchText = string.Join(" ", searchTerm);

            return $"%{searchText}%";
        }

        public static string BuildFullTextSearchTerm(this string keyword)
        {
            keyword = string.IsNullOrEmpty(keyword) ? string.Empty : keyword;

            var keywords = keyword.Trim().Split(" ").Where(x => !string.IsNullOrEmpty(x)).ToList();

            return '"' + $"{string.Join(" ", keywords)}*" + '"';
        }
    }
}
