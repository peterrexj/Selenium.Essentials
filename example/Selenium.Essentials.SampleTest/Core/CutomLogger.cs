using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials.SampleTest.Core
{
    public class CutomLogger : ILog
    {
        public void Log(string message)
        {
            Console.WriteLine($"With custom logging: {message}");
        }

        public void Log(string message, Exception ex = null)
        {
            Console.WriteLine($"With custom logging: {message}, Error: {ex.Message}, Error detail: {ex.ToString()}");
        }
    }
}
