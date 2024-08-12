using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Selenium.Essentials
{
    public class RemoteDriverAccessModel
    {
        public Dictionary<string, object> Capabilities { get; set; }
        public string RemoteHubUrl { get; set; }
        public int CommandTimeoutInSeconds { get; set; }
        public string Platform { get; set; }
        public string BrowserVersion { get; set; }

        public string ProxyAddress { get; set; } 
        public bool BypassProxyOnLocal { get; set; }  
        public ICredentials ProxyCredentials { get; set; }
    }
}
