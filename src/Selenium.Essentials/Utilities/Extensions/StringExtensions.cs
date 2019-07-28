using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Selenium.Essentials.Utilities.Extensions
{
    public static class StringExtensions
    {

        #region Conversion
        /// <summary>
        /// Converts the string to integer. Returns 0 if the conversion fails and does not throw any exception
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static int ToInteger(this string text)
        {
            try
            {
                int.TryParse(text, out int result);
                return result;
            }
            catch (Exception)
            {
                //Log the exception here
                return 0;
            }
        }

        /// <summary>
        /// Converts the string to integer. Returns 0 if the conversion fails and does not throw any exception
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static long ToLong(this string text)
        {
            try
            {
                long.TryParse(text, out long result);
                return result;
            }
            catch (Exception)
            {
                //Log the exception here
                return 0;
            }
        }

        /// <summary>
        /// Converts the string to decimal. Returns 0 if the conversion fails and does not throw any exception
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string text)
        {
            try
            {
                decimal.TryParse(text, out decimal result);
                return result;
            }
            catch (Exception)
            {
                //Log the exception here
                return 0;
            }
        }

        /// <summary>
        /// Returns string to a bool value. Conditions for true values are 'yes', 'true', 'enable', 'enabled'
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool ToBool(this string text)
        {
            if (text.IsEmpty())
                return false;

            return text.EqualsIgnoreCase("yes") ||
                   text.EqualsIgnoreCase("true") ||
                   text.EqualsIgnoreCase("Enable") ||
                   text.EqualsIgnoreCase("Enabled") ||
                   text.Equals("1");
        }

        /// <summary>
        /// Converts the string to double. Returns 0 if the conversion fails and does not throw any exception
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDouble(this string value)
        {
            try
            {
                double.TryParse(value, out double result);
                return result;
            }
            catch (Exception)
            {
                //Log the exception here
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string value)
        {
            try
            {
                if (value.IsEmpty())
                    return DateTime.MinValue;

                DateTime.TryParse(value, out DateTime result);
                return result;
            }
            catch (Exception)
            {
                //Log the exception here
                return DateTime.MinValue;
            }

        }

        /// <summary>
        /// Returns a dynamic object in the JSON format
        /// </summary>
        /// <param name="value">The value which is to be converted to JSON</param>
        /// <param name="exposeError">True if you want to throw the conversion error</param>
        /// <returns></returns>
        public static dynamic ToJson(this string value, bool exposeError = false)
        {
            try
            {
                return JsonConvert.DeserializeObject<dynamic>(value);
            }
            catch (Exception)
            {
                if (exposeError)
                {
                    throw;
                }
                else
                {
                    return null;
                }
            }
        }

        public static T ToJson<T>(this string value, bool exposeError = false)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            catch (Exception)
            {
                if (exposeError)
                {
                    throw;
                }
                else
                {
                    return default;
                }
            }
        }
        #endregion


        public static bool IsEmpty(this string text) 
            => string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text);

        public static bool HasValue(this string value) => !IsEmpty(value);

        public static string EmptyIfNull(this string value) => value ?? string.Empty;

        public static bool EqualsIgnoreCase(this string value, string compareValue) 
            => value.EmptyIfNull().Equals(compareValue, StringComparison.InvariantCultureIgnoreCase);


        public static string SurroundWith(this string str, string surroundString)
        {
            if (!str.StartsWith(surroundString))
            {
                str = $@"{surroundString}{str}";
            }
            if (!str.EndsWith(surroundString))
            {
                str = $@"{str}{surroundString}";
            }

            return str;
        }

        public static IEnumerable<string> ApplyJsonPathExpression(this string value, string jsonFilter) => JObject.Parse(value.EmptyIfNull()).SelectTokens(jsonFilter).Select(s => Convert.ToString(s)).ToArray();
        public static IEnumerable<string> SplitAndTrim(this string value, string splitString) => value.Split(new[] { splitString }, StringSplitOptions.RemoveEmptyEntries)
                .Select(d => d.Trim())
                .Where(d => d.HasValue());
        public static IEnumerable<string> SplitByWithDistinct(this string value, string splitString) => SplitAndTrim(value, splitString)
                 .Distinct();


        public static bool Contains(this string haystack, string pin, StringComparison comparisonOptions) => haystack.IndexOf(pin, comparisonOptions) >= 0;
        public static bool ContainsIgnoreCase(this string data, string value) => data.Contains(value, StringComparison.InvariantCultureIgnoreCase);
        public static bool ContainsIgnoreCase(this string data, string[] value) => value.Any(str => data.ContainsIgnoreCase(str));

        public static string ConvertToDbFormatColumnName(this string value) => "_" + Regex.Replace(value, @"[^\w]+", "").ToLower();

        public static string StartWithCompareThenTrim(this string value, string[] checks)
            => checks.Any(value.StartsWith)
                ? checks.Where(value.StartsWith)
                    .Select(l => value.Substring(l.Length, value.Length - l.Length)).FirstOrDefault()
                : value;
        
        public static string EndWithCompareThenTrim(this string value, string[] checks)
            => checks.Any(value.EndsWith)
                ? checks.Where(value.EndsWith)
                    .Select(l => value.Substring(0, value.Length - l.Length)).FirstOrDefault()
                : value;
        

        public static string StripQuotes(this string value)
            => value
            .StartWithCompareThenTrim(new[] { "\"" })
            .EndWithCompareThenTrim(new[] { "\"" });

        public static string ExtractNumber(this string original)
            => new string(original.Where(c => Char.IsNumber(c)).ToArray());

        public static string ConvertToValidFileName(this string name, int length = 0)
        {
            StringBuilder result = new StringBuilder();
            foreach (var str in name)
            {
                if (Char.IsLetterOrDigit(str))
                    result.Append(str);
            }

            if (length > 0 && result.Length > length)
            {
                return result.ToString().Substring(0, length);
            }
            else
            {
                return result.ToString();
            }
        }

        public static string ReplaceMultiple(this string value, string replaceWith, params string[] replaceContents)
        {
            replaceContents.Iter(r => value = value.Replace(r, replaceWith));
            return value;
        }
    }
}
