using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Essentials
{
    /// <summary>
    /// Static Framework settings and configuration
    /// </summary>
    public static class SeAppConfig
    {
        /// <summary>
        /// Total amount of time for wait operations in web driver. 
        /// Default is set to 60 seconds
        /// </summary>
        public static int DefaultTimeoutWaitPeriodInSeconds { get; set; } = 60;

        /// <summary>
        /// Total amount of time for wait operations in Api request. 
        /// Default is set to 60 seconds
        /// </summary>
        public static int DefaultApiResponseTimeoutWaitPeriodInSeconds { get; set; } = 60;

        /// <summary>
        /// How many times to retry when finding an element in web driver. 
        /// Default is set to 2
        /// </summary>
        public static int DefaultRetryElementCount { get; set; } = 2;

        /// <summary>
        /// Timeout in seconds for page request on web driver. This timeout is used for command timeout for web driver
        /// Default is set to 60 seconds
        /// </summary>
        public static int WebDriverPageLoadWaitTime { get; set; } = 60;
    }
}
