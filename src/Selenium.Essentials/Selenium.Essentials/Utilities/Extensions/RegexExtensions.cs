using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace Selenium.Essentials.Utilities.Extensions
{
    public static class RegexExtensions
    {
        public static Regex AsRegex(this string pattern)
        {
            return new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        public static Regex AsRegex(this string pattern, RegexOptions options)
        {
            return new Regex(pattern, options);
        }

        public static bool RegexMatching(this string text, string regexMatchExpression) 
            => text.RegexMatchExtractFirstValue(regexMatchExpression).HasValue();

        /// <summary>
        /// Match the regex expression and return the fist match
        /// </summary>
        /// <param name="text"></param>
        /// <param name="regexMatchExpression"></param>
        /// <returns></returns>
        public static string RegexMatchExtractFirstValue(this string text, string regexMatchExpression)
        {
            return Regex.Match(text, regexMatchExpression).Value;
        }

        /// <summary>
        /// Match the regex expression and get the value from the group
        /// </summary>
        /// <param name="text"></param>
        /// <param name="regexMatchExpression"></param>
        /// <param name="groupValue"></param>
        /// <returns></returns>
        public static string RegexMatchGroupValue(this string text, string regexMatchExpression, int groupValue) 
            => Regex.Match(text, regexMatchExpression).Groups[groupValue].Value;

        public static IEnumerable<string> RegexMatchGroupValue(
            this string text, 
            string regexMatchExpression) 
            => from Match match in Regex.Matches(text, regexMatchExpression)
                    select match.Value;
    }
}
