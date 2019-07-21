using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Essentials
{
    public static class AppConfig
    {
        public static int DefaultTimeoutWaitPeriodInSeconds { get; set; } = 60;
        public static int DefaultApiResponseTimeoutWaitPeriodInSeconds { get; set; } = 60;

        public static int DefaultRetryElementCount { get; set; } = 2;
        public static int WebDriverPageLoadWaitTime { get; set; } = 60;
    }
}
