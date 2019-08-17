using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials
{
    public class RemoteDriverAccessModel
    {
        public Dictionary<string, string> Capabilities { get; set; }
        public string RemoteHubUrl { get; set; }
        public int CommandTimeoutInSeconds { get; set; }
    }
}
