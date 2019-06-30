using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials.Utilities.Extensions
{
    public static class DateTimeExtensions
    {
        public const string __DateDefaultFormatString = "yyMMddHHmmssfff";

        public static DateTime LocalTime => TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.Local);

        public static string Timestamp => GetTimestamp(__DateDefaultFormatString);

        public static string GetTimestamp(string format) => LocalTime.ToString(format);

        /// <summary>
        /// Returns the total duration of start and end date in time format (h:m:s)
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static string GetTotalDurationAsString(DateTime start, DateTime end)
        {
            TimeSpan duration = end - start;
            StringBuilder res = new StringBuilder();
            if (duration.Hours > 0)
            {
                res.Append(duration.Hours.ToString() + "h ");
            }
            if (duration.Hours > 0 || duration.Minutes > 0)
            { 
                res.Append(duration.Minutes.ToString() + "m ");
            }
            res.Append(duration.Seconds.ToString() + "s");

            return res.ToString();
        }

        /// <summary>
        /// Returns the total duration of start and end date in time stamp format
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static TimeSpan GetTotalDurationAsTimeSpan(DateTime start, DateTime end) => end - start;
    }
}
