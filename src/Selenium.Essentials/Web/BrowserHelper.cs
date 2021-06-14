using FluentAssertions;
using Microsoft.Win32;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using Pj.Library;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using static Pj.Library.PjUtility;

namespace Selenium.Essentials
{
    public enum BrowserType
    {
        Chrome,
        FireFox,
        InternetExplorer,
        Edge,
        Safari,
    }

    public static class BrowserHelper
    {
        public static string DriverFolder { get; set; } = Runtime.ExecutingFolder;

        private static ChromeOptions _chromeOptions;
        public static ChromeOptions ChromeOptions
        {
            get
            {
                if (_chromeOptions == null)
                {
                    _chromeOptions = new ChromeOptions();
                    _chromeOptions.AddUserProfilePreference("download.default_directory", Runtime.ExecutingFolder);
                    _chromeOptions.AddUserProfilePreference("download.prompt_for_download", false);
                    _chromeOptions.AddUserProfilePreference("profile.content_settings.exceptions.automatic_downloads.*.setting", 1);
                    _chromeOptions.AddArgument("--disable-extensions");
                    _chromeOptions.AddArgument("no-sandbox");
                }
                return _chromeOptions;
            }
            set
            {
                _chromeOptions = value;
            }
        }

        private static FirefoxOptions _firefoxOptions;
        public static FirefoxOptions FirefoxOptions
        {
            get
            {
                if (_firefoxOptions == null)
                {
                    _firefoxOptions = new FirefoxOptions();
                    _firefoxOptions.AddArgument("--marionette");
                    _firefoxOptions.AcceptInsecureCertificates = true;
                }
                return _firefoxOptions;
            }
            set
            {
                _firefoxOptions = value;
            }

        }

        private static InternetExplorerOptions _internetExplorerOptions;
        public static InternetExplorerOptions InternetExplorerOptions
        {
            get
            {
                if (_internetExplorerOptions == null)
                {
                    _internetExplorerOptions = new InternetExplorerOptions
                    {
                        IgnoreZoomLevel = false,
                        IntroduceInstabilityByIgnoringProtectedModeSettings = true
                    };
                }
                return _internetExplorerOptions;
            }
            set
            {
                _internetExplorerOptions = value;
            }
        }

        public static IWebDriver GetDriver(BrowserType browserType)
        {
            switch (browserType)
            {
                case BrowserType.Chrome:
                    return GetChromeBrowser();
                case BrowserType.FireFox:
                    return GetFirefoxBrowser();
                case BrowserType.InternetExplorer:
                    return GetInternetExplorerBrowser();
                case BrowserType.Edge:
                    return GetEdgeBrowser();
                case BrowserType.Safari:
                    return GetSafariBrowser();
                default:
                    return GetChromeBrowser();
            }
        }
        public static IWebDriver GetChromeBrowser()
        {
            var chromeService = ChromeDriverService.CreateDefaultService(DriverFolder);
            chromeService.HideCommandPromptWindow = true;

            try
            {
                return new ChromeDriver(chromeService, ChromeOptions);
            }
            catch (Exception)
            {
                return new ChromeDriver(chromeService, ChromeOptions);
            }
        }
        public static IWebDriver GetEdgeBrowser() { return null; }
        public static IWebDriver GetFirefoxBrowser()
        {
            InstalledBrowsers
                .Any(d =>
                    d.Name.ContainsIgnoreCase("firefox") &&
                    d.InstallationPath.HasValue())
                .Should()
                .BeTrue(
                "Firefox is not installed in your computer. Make sure you installed firefox in your machine and InstalledBrowsers is listing the browser");

            FirefoxDriverService service = FirefoxDriverService.CreateDefaultService(DriverFolder);
            service.FirefoxBinaryPath = InstalledBrowsers
                .FirstOrDefault(d => d.Name.ContainsIgnoreCase("firefox"))?.InstallationPath;
            service.HideCommandPromptWindow = true;

            return new FirefoxDriver(service, FirefoxOptions);
        }
        public static IWebDriver GetSafariBrowser() { return null; }
        public static IWebDriver GetInternetExplorerBrowser()
        {
            var service = InternetExplorerDriverService.CreateDefaultService(DriverFolder);
            service.HideCommandPromptWindow = true;

            return new InternetExplorerDriver(service, InternetExplorerOptions);
        }
        public static IWebDriver GetRemoteDriver(RemoteDriverAccessModel remoteDriverAccessModel)
        {
            if (remoteDriverAccessModel == null)
            {
                return null;
            }

            var capabilities = new DesiredCapabilities();

            if (remoteDriverAccessModel.Capabilities != null)
            {
                foreach (var capability in remoteDriverAccessModel.Capabilities)
                {
                    capabilities.SetCapability(capability.Key, capability.Value);
                }
            }
            var driver = new RemoteWebDriver(new Uri(remoteDriverAccessModel.RemoteHubUrl), 
                capabilities, 
                TimeSpan.FromSeconds(remoteDriverAccessModel.CommandTimeoutInSeconds));
            return driver;
        }

        public static BrowserType GetBrowserType(string browserName)
        {
            if (browserName.EqualsIgnoreCase("chrome")) return BrowserType.Chrome;
            else if (new[] { "firefox", "fire fox", "ff" }.ContainsIgnoreCase(browserName)) return BrowserType.FireFox;
            else if (new[] { "internet explorer", "internetexplorer", "ie" }.ContainsIgnoreCase(browserName)) return BrowserType.InternetExplorer;
            else if (new[] { "safari" }.ContainsIgnoreCase(browserName)) return BrowserType.Safari;
            else if (new[] { "edge", "ms edge", "msedge" }.ContainsIgnoreCase(browserName)) return BrowserType.Edge;
            else return BrowserType.Chrome;
        }

        private static List<BrowserInformationModel> _installedBrowsers;
        public static List<BrowserInformationModel> InstalledBrowsers
        {
            get
            {
                if (_installedBrowsers == null || _installedBrowsers.Count == 0)
                {
                    _installedBrowsers = new List<BrowserInformationModel>();
                    try
                    {
                        RegistryKey browserKeys = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Clients\StartMenuInternet") ??
                                      Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet");
                        var browserNames = browserKeys.GetSubKeyNames();
                        foreach (var bName in browserNames)
                        {
                            BrowserInformationModel browser = new BrowserInformationModel();
                            RegistryKey browserKey = browserKeys.OpenSubKey(bName);
                            browser.Name = (string)browserKey.GetValue(null);
                            RegistryKey browserKeyPath = browserKey.OpenSubKey(@"shell\open\command");
                            browser.InstallationPath = (string)browserKeyPath.GetValue(null).ToString().StripQuotes();
                            RegistryKey browserIconPath = browserKey.OpenSubKey(@"DefaultIcon");
                            browser.IconPath = (string)browserIconPath.GetValue(null).ToString().StripQuotes();
                            browser.BrowserVersion = browser.InstallationPath.HasValue()
                                ? FileVersionInfo.GetVersionInfo(browser.InstallationPath).FileVersion
                                : "unknown";
                            _installedBrowsers.Add(browser);
                        }
                    }
                    catch (SecurityException e)
                    {
                        Runtime.Logger.Log($"Unable to get browser info due to access issues. {e.Message}", e);
                    }
                    catch (Exception e)
                    {
                        Runtime.Logger.Log($"Unable to get browser info due to exception : {e.Message}", e);
                    }
                }
                return _installedBrowsers;
            }
        }
    }
}
