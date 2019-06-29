using Selenium.Essentials.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Selenium.Essentials.Core
{
    public static class Utility
    {
        public static Assembly AppAssembly => Assembly.GetExecutingAssembly();
        public static string ExecutingFolder => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string LogOutputFolder { get; set; } = Path.Combine(ExecutingFolder, "Logs", $"{DateTimeExtensions.Timestamp}.csv");

    }
}
