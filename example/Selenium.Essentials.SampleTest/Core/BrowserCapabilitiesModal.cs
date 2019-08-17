using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials.SampleTest.Core
{
    /// <summary>
    /// Model to contain the browser capabilities
    /// </summary>
    public class BrowserCapabilitiesModal
    {
        public string CapabilityName { get; set; }
        public string BrowserName { get; set; }
        public string Platform { get; set; }
        public string Version { get; set; }
        public string ScreenResolution { get; set; }
    }
}
