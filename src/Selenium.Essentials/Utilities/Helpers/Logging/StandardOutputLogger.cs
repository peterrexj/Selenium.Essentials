using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials
{
    public class StandardOutputLogger : ILog
    {
        public void Log(string message, Exception ex = null)
        {
            if (ex == null)
            {
                Console.WriteLine(message);
            }
            else
            {
                Console.WriteLine($"{message}, Error: {ex.ToString()}");
            }
        }
    }
}
