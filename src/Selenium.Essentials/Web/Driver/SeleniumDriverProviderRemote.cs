using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Selenium.Essentials
{
    [Serializable]
    public class SeleniumDriverProviderRemote
    {
        private readonly SeleniumDriverCapabilitiesProvider _browserCapabilitiesProvider = new();

        public IWebDriver GetDriver(string driverType, RemoteDriverAccessModel remoteDriverAccessModel)
        {
            Enum.TryParse(driverType, true, out BrowserType browserType);

            return GetDriver(browserType, remoteDriverAccessModel);
        }

        public IWebDriver GetDriver(BrowserType driverType, RemoteDriverAccessModel? remoteDriverAccessModel)
        {
            if (remoteDriverAccessModel == null)
            {
                throw new ArgumentNullException(nameof(remoteDriverAccessModel));
            }

            if (!Enum.IsDefined(typeof(BrowserType), driverType))
                throw new InvalidEnumArgumentException(nameof(driverType), (int)driverType, typeof(BrowserType));

            var capabilities = _browserCapabilitiesProvider.GetCapability(driverType, isRemote: true, remoteDriverAccessModel).ToCapabilities();

            var driver = new RemoteWebDriver(new Uri(remoteDriverAccessModel.RemoteHubUrl),
                capabilities,
                TimeSpan.FromSeconds(remoteDriverAccessModel.CommandTimeoutInSeconds));
            return driver;
        }

        //public IWebDriver GetDriver(RemoteDriverAccessModel? remoteDriverAccessModel)
        //{
        //    if (remoteDriverAccessModel == null)
        //    {
        //        throw new ArgumentNullException(nameof(remoteDriverAccessModel));
        //    }

        //    var capabilities = new DriverOptions();

        //    if (remoteDriverAccessModel.Capabilities != null)
        //    {
        //        foreach (var capability in remoteDriverAccessModel.Capabilities)
        //        {
        //            capabilities.SetCapability(capability.Key, capability.Value);
        //        }
        //    }
        //    var driver = new RemoteWebDriver(new Uri(remoteDriverAccessModel.RemoteHubUrl),
        //        capabilities,
        //        TimeSpan.FromSeconds(remoteDriverAccessModel.CommandTimeoutInSeconds));
        //    return driver;

        //    var capabilities = _browserCapabilitiesProvider.GetCapability(driverType, isRemote: true, remoteDriverAccessModel).ToCapabilities();

        //    var driver = new RemoteWebDriver(new Uri(remoteDriverAccessModel.RemoteHubUrl),
        //        capabilities,
        //        TimeSpan.FromSeconds(remoteDriverAccessModel.CommandTimeoutInSeconds));
        //    return driver;
        //}

    }
}
