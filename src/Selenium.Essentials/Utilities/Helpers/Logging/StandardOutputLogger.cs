using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials
{
    public class StandardOutputLogger : ILog
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }

        public void Log(string message, Exception ex = null)
        {
            Console.WriteLine($"{message}, Error: {ex.ToString()}");
        }
    }
}
