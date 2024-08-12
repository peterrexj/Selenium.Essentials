using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using OpenQA.Selenium.Chrome;

namespace Selenium.Essentials
{
    [Serializable]
    public class SeleniumDriverProviderRemote
    {
        private readonly SeleniumDriverCapabilitiesProvider _browserCapabilitiesProvider = new();

        public IWebDriver GetDriver(string browserType, RemoteDriverAccessModel remoteDriverAccessModel)
        {
            Enum.TryParse(browserType, true, out BrowserType parsedBrowserType);

            return GetDriver(parsedBrowserType, remoteDriverAccessModel);
        }

        public IWebDriver GetDriver(BrowserType browserType, RemoteDriverAccessModel? remoteDriverAccessModel)
        {
            if (remoteDriverAccessModel == null)
            {
                throw new ArgumentNullException(nameof(remoteDriverAccessModel));
            }

            if (!Enum.IsDefined(typeof(BrowserType), browserType))
                throw new InvalidEnumArgumentException(nameof(browserType), (int)browserType, typeof(BrowserType));

            var capabilities =
                _browserCapabilitiesProvider.GetCapability(browserType, isRemote: true, remoteDriverAccessModel);

            var driver = new RemoteWebDriver(new Uri(remoteDriverAccessModel.RemoteHubUrl),
                capabilities.ToCapabilities(),
            TimeSpan.FromSeconds(remoteDriverAccessModel.CommandTimeoutInSeconds));
            return driver;
        }
    }
}
