using OpenQA.Selenium;
using Pj.Library;
using System;
using System.Linq;
using System.Net;

namespace Selenium.Essentials
{
    [Serializable]
    public class SeleniumDriverProxyProvider
    {
        public WebProxy? BuildWebProxy(RemoteDriverAccessModel? browserGridCapability)
        {
            return browserGridCapability != null && browserGridCapability.ProxyAddress.HasValue()
                ? new WebProxy
                {
                    Address = new Uri(browserGridCapability.ProxyAddress),
                    BypassProxyOnLocal = browserGridCapability.BypassProxyOnLocal,
                    Credentials = browserGridCapability.ProxyCredentials
                }
                : null;
        }

        public Proxy? BuildSeleniumProxy(RemoteDriverAccessModel? browserGridCapability) => ToSeleniumProxy(BuildWebProxy(browserGridCapability));

        public Proxy? ToSeleniumProxy(WebProxy? proxy)
        {
            if (proxy == null)
            {
                return null;
            }

            var seleniumProxy = new Proxy
            {
                HttpProxy = proxy.Address.ToString(),
                SslProxy = proxy.Address.ToString(),
                FtpProxy = proxy.Address.ToString(),
            };

            if (!proxy.BypassProxyOnLocal) return seleniumProxy;

            seleniumProxy.AddBypassAddresses("localhost");
            if (proxy.BypassList != null)
            {
                seleniumProxy.AddBypassAddresses(proxy.BypassList.OfType<string>());
            }
            return seleniumProxy;
        }
    }
}
