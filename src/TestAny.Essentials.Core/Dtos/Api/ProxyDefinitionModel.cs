using System;
using System.Collections.Generic;
using System.Text;

namespace TestAny.Essentials.Core.Dtos.Api
{
    public class ProxyDefinitionModel
    {
        public string Host { get; set; }
        public long Port { get; set; }
        public bool ByPassProxyOnLocal { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public bool UseOnlyInContinousIntegration { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public ProxyDefinitionModel()
        {
            Port = 8080;
            ByPassProxyOnLocal = false;
            UseDefaultCredentials = true;
        }
    }
}
