using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace CMS.Core.Helpers
{
    public static class StringHelper
    {
        /// <summary>
        /// Use it for split every user in string usernames
        /// </summary>
        public static string SplitCharacter = ";";

        /// <summary>
        /// Compares the string against a given pattern.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="pattern">The pattern to match, where "*" means any sequence of characters, and "?" means any single character.</param>
        /// <returns><c>true</c> if the string matches the given pattern; otherwise <c>false</c>.</returns>
        public static bool Like(this string str, string pattern)
        {
            return new Regex(
                "^" + Regex.Escape(pattern).Replace(@"\*", ".*").Replace(@"\?", ".") + "$",
                RegexOptions.IgnoreCase | RegexOptions.Singleline
            ).IsMatch(str);
        }

        public static string ShortName(string str, int lenght = 30)
        {
            if (!string.IsNullOrWhiteSpace(str) && str.Length > lenght)
            {
                var shortName = string.Empty;
                str.Split(new char[] { ' ', '-', '(', ')' }, StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(i => shortName += i[0].ToString());
                return shortName;
            }
            return str;
        }

        /// <summary>
        /// Remove ";" character in the first and last of string user_name
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveSplitCharacterInFirstAndLast(string text)
        {
            if (!string.IsNullOrEmpty(text) && text.Length > 2)
            {
                text = text.Remove(0, 1);
                text = text.Remove(text.Length - 1);
            }
            return text;
        }
    }
}
