using NUnit.Framework;
using Selenium.Essentials.SampleTest.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Selenium.Essentials.SampleTest
{
    public static class TestUtility
    {
        private static Dictionary<string, string> _envData;

        /// <summary>
        /// Loads the environment information from the json and stores as Dictionary
        /// </summary>
        public static Dictionary<string, string> EnvData
        {
            get
            {
                if (_envData == null)
                {
                    var currentEnv = Utility.AppConfig.AppSettingsCallerAssembly
                        .Where(k => k.Key.EqualsIgnoreCase("Environment"))
                        .FirstOrDefault().Value;
                    var envDataFilePath = Path.Combine(Utility.Runtime.ExecutingFolder, "DataSource", "EnvironmentData", currentEnv, "EnvData.json");
                    if (File.Exists(envDataFilePath))
                    {
                        _envData = SerializationHelper.JsonToDictionary(new PayloadDataJsonAttribute(envDataFilePath).FileContent);
                    }
                    else
                    {
                        _envData = new Dictionary<string, string>();
                    }
                    _envData.Add("Environment", currentEnv);
                }
                return _envData;
            }
        }

        /// <summary>
        /// Opens a Webdriver
        /// On Debug mode - will always open the browser on the local computer
        /// On Release mode - Check if the browser capability json is available and contains the 
        /// browserType defined in that file, else will fall back to the local browser
        /// </summary>
        /// <param name="browserType"></param>
        public static void InitializeDriver(string browserType)
        {
            var openLocalBrowser = true; //variable to determine local browser
            BrowserCapabilitiesModal browserCapability = null;

            var pathToBrowserCapabilities = EnvData["PathToBrowserCapabilities"];

            if (Utility.Runtime.IsInDebugMode)
            {
                openLocalBrowser = true;
            }
            //not in debug and contains browser capabilities
            else if (StorageHelper.Exists(StorageHelper.GetAbsolutePath(pathToBrowserCapabilities)))
            {
                var capabilities = SerializationHelper.DeSerializeFromJsonFile<List<BrowserCapabilitiesModal>>(pathToBrowserCapabilities);
                browserCapability = capabilities.FirstOrDefault(c => c.CapabilityName.EqualsIgnoreCase(browserType));
                if (browserCapability != null)
                {
                    openLocalBrowser = false;
                }
            }

            if (openLocalBrowser || browserCapability == null)
            {
                TestContextHelper.Set("Driver", BrowserHelper.GetChromeBrowser());
                TestContextHelper.Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(EnvData["PageLoadTimeoutInSeconds"].ToInteger());
            }
            else
            {
                if (browserCapability != null)
                {
                    var remoteDriverModel = new RemoteDriverAccessModel
                    {
                        RemoteHubUrl = EnvData["SauceLabsRemoteHubUrl"],
                        CommandTimeoutInSeconds = EnvData["PageLoadTimeoutInSeconds"].ToInteger(),
                        Capabilities = new Dictionary<string, string>
                        {
                            { "username", EnvData["SauceLabsUsername"] },
                            { "accessKey", EnvData["SauceLabsAccessKey"] },
                            { "browserName", browserCapability.BrowserName },
                            { "platform", browserCapability.Platform },
                            { "version", browserCapability.Version },
                            { "screenResolution", browserCapability.ScreenResolution },
                            { "name", TestContext.CurrentContext.Test.Name }
                        }
                    };
                    TestContextHelper.Set("Driver", BrowserHelper.GetRemoteDriver(remoteDriverModel));
                }
                else
                {
                    Assert.IsTrue(false, "The browser initialization failed as was not to determine which browser to open");
                }
            }
        }
    }
}
