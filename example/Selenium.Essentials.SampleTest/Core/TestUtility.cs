using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using Selenium.Essentials.SampleTest.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Selenium.Essentials.SampleTest
{
    public static class TestUtility
    {
        private static ConcurrentDictionary<string, IWebDriver> _sessionDrivers;
        public static ConcurrentDictionary<string, IWebDriver> SessionDrivers
        {
            get
            {
                if (_sessionDrivers == null)
                {
                    _sessionDrivers = new ConcurrentDictionary<string, IWebDriver>();
                }
                return _sessionDrivers;
            }
        }

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
        public static IWebDriver InitializeDriver(string browserType)
        {
            var openLocalBrowser = Utility.Runtime.IsInDebugMode; //variable to determine local browser
            BrowserCapabilitiesModal browserCapability = null;
            IWebDriver driver = null;

            var pathToBrowserCapabilities = EnvData["PathToBrowserCapabilities"];
            if (!openLocalBrowser && StorageHelper.Exists(StorageHelper.GetAbsolutePath(pathToBrowserCapabilities)))
            {
                var capabilities = SerializationHelper.DeSerializeFromJsonFile<List<BrowserCapabilitiesModal>>(pathToBrowserCapabilities);
                browserCapability = capabilities.FirstOrDefault(c => c.CapabilityName.EqualsIgnoreCase(browserType));
                if (browserCapability != null)
                {
                    openLocalBrowser = false;
                }
            }

            if (openLocalBrowser)
            {
                driver = BrowserHelper.GetChromeBrowser();
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(EnvData["PageLoadTimeoutInSeconds"].ToInteger());
            }
            else
            {
                if (browserCapability != null)
                {
                    var buildNumber = Environment.GetEnvironmentVariable("TRAVIS_BUILD_NUMBER") ?? string.Empty;
                    var travisJobNumber = Environment.GetEnvironmentVariable("TRAVIS_JOB_NUMBER") ?? string.Empty;
                    var sauceUsername = Environment.GetEnvironmentVariable("SAUCE_USERNAME") ?? EnvData["SauceLabsUsername"];
                    var sauceAccessKey = Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY") ?? EnvData["SauceLabsAccessKey"];

                    Console.WriteLine($"Travis CI build number: {buildNumber}");
                    Console.WriteLine($"Travis CI job number: {travisJobNumber}");
                    Console.WriteLine($"Travis CI username (sauce): {sauceUsername}");

                    var remoteDriverModel = new RemoteDriverAccessModel
                    {
                        RemoteHubUrl = EnvData["SauceLabsRemoteHubUrl"],
                        CommandTimeoutInSeconds = EnvData["PageLoadTimeoutInSeconds"].ToInteger(),
                        Capabilities = new Dictionary<string, string>
                        {
                            { "build", buildNumber },
                            { "tunnel-identifier", travisJobNumber },
                            { "username", sauceUsername },
                            { "accessKey", sauceAccessKey },
                            { "browserName", browserCapability.BrowserName },
                            { "platform", browserCapability.Platform },
                            { "version", browserCapability.Version },
                            { "screenResolution", browserCapability.ScreenResolution },
                            { "name", TestContext.CurrentContext.Test.Name }
                        }
                    };
                    driver = BrowserHelper.GetRemoteDriver(remoteDriverModel);
                }
                else
                {
                    Assert.IsTrue(false, "The browser initialization failed as was not to determine which browser to open");
                }
            }
            if (SessionDrivers.ContainsKey(TestContext.CurrentContext.Test.Name))
            {
                driver?.CloseDriver();
                SessionDrivers.ContainsKey(TestContext.CurrentContext.Test.Name)
                    .Should().BeFalse($"Driver already initiated for test: [{TestContext.CurrentContext.Test.Name}] with session id [{(driver as RemoteWebDriver).SessionId.ToString()}]");
            }
            else
            {
                SessionDrivers.TryAdd(TestContext.CurrentContext.Test.Name, driver);
            }
            return driver;
        }
    }
}
