using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using Pj.Library;
using Selenium.Essentials.SampleTest.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TestAny.Essentials.Core.Attributes;
using static Pj.Library.PjUtility;

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
                    var currentEnv = AppSettingsConfig.AppSettingsCallerAssembly
                        .Where(k => k.Key.EqualsIgnoreCase("Environment"))
                        .FirstOrDefault().Value;
                    var envDataFilePath = Path.Combine(Runtime.ExecutingFolder, "DataSource", "EnvironmentData", currentEnv, "EnvData.json");
                    if (File.Exists(envDataFilePath))
                    {
                        _envData = JsonHelper.ConvertComplexJsonDataToDictionary(new PayloadDataJsonAttribute(envDataFilePath).FileContent)
                            .ToDictionary(d => d.Key, d => d.Value);
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
            var openLocalBrowser = Runtime.IsInDebugMode; //variable to determine local browser
            BrowserCapabilitiesModal browserCapability = null;
            IWebDriver driver = null;

            if (!openLocalBrowser && BrowserCapabilityHelper.CurrentBrowserCapabilities.Any())
            {
                browserCapability = BrowserCapabilityHelper.CurrentBrowserCapabilities.FirstOrDefault(c => c.CapabilityName.EqualsIgnoreCase(browserType));
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

                    Runtime.Logger.Log($"Travis CI build number: {buildNumber}");
                    Runtime.Logger.Log($"Travis CI job number: {travisJobNumber}");
                    Runtime.Logger.Log($"Travis CI username (sauce): {sauceUsername}");

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
                            { "name", TestContext.CurrentContext.Test.Name }
                        }
                    };
                    remoteDriverModel.Capabilities.AddOrUpdate(browserCapability.ToCustomDictionary());
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

                if (SessionDrivers.ContainsKey(TestContext.CurrentContext.Test.Name))
                    throw new Exception($"Driver already initiated for test: [{TestContext.CurrentContext.Test.Name}] with session id [{(driver as RemoteWebDriver).SessionId}]");
            }
            else
            {
                SessionDrivers.TryAdd(TestContext.CurrentContext.Test.Name, driver);
            }
            return driver;
        }
    }
}
