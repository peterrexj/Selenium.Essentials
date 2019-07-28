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
            => new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static Regex AsRegex(this string pattern, RegexOptions options)
            => new Regex(pattern, options);

        public static bool RegexMatching(this string text, 
            string regexMatchExpression) 
            => text.RegexMatchExtractFirstValue(regexMatchExpression).HasValue();

        public static string RegexMatchExtractFirstValue(this string text, 
            string regexMatchExpression)
            => Regex.Match(text, regexMatchExpression).Value;

        public static string RegexMatchGroupValue(this string text, 
            string regexMatchExpression, 
            int groupValue) 
            => Regex.Match(text, regexMatchExpression).Groups[groupValue].Value;

        public static IEnumerable<string> RegexMatchGroupValue(
            this string text, 
            string regexMatchExpression) 
            => from Match match in Regex.Matches(text, regexMatchExpression)
                    select match.Value;
    }
}
