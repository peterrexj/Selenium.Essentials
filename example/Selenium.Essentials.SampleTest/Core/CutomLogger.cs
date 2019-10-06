using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials.SampleTest.Core
{
    public class CutomLogger : ILog
    {
        public void Log(string message, Exception ex = null)
        {
            if (ex == null)
            {
                Console.WriteLine($"Custom logging: {message}");
            }
            else
            {
                Console.WriteLine($"With custom logging: {message}, Error: {ex.Message}, Error detail: {ex.ToString()}");
            }
        }
    }
}
