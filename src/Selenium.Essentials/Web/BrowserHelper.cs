using Microsoft.Win32;
using OpenQA.Selenium;
using Pj.Library;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security;

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
        public static IWebDriver GetDriver(string browserName, bool isRemote = false, RemoteDriverAccessModel? remoteDriverAccessModel = null)
        {
            return GetDriver(GetBrowserType(browserName), isRemote, remoteDriverAccessModel);
        }
        public static IWebDriver GetDriver(BrowserType browserType, bool isRemote = false, RemoteDriverAccessModel? remoteDriverAccessModel = null)
        {
            return browserType switch
            {
                BrowserType.Chrome => GetChromeBrowser(isRemote, remoteDriverAccessModel),
                BrowserType.FireFox => GetFirefoxBrowser(isRemote, remoteDriverAccessModel),
                BrowserType.InternetExplorer => GetInternetExplorerBrowser(isRemote, remoteDriverAccessModel),
                BrowserType.Edge => GetEdgeBrowser(isRemote, remoteDriverAccessModel),
                BrowserType.Safari => GetSafariBrowser(isRemote, remoteDriverAccessModel),
                _ => GetChromeBrowser(isRemote, remoteDriverAccessModel),
            };
        }
        public static IWebDriver GetChromeBrowser(bool isRemote = false, RemoteDriverAccessModel? remoteDriverAccessModel = null) => 
            new SeleniumDriverMiddleware().GetDriver(BrowserType.Chrome, isRemote, remoteDriverAccessModel);

        public static IWebDriver GetEdgeBrowser(bool isRemote = false, RemoteDriverAccessModel? remoteDriverAccessModel = null) =>
            new SeleniumDriverMiddleware().GetDriver(BrowserType.Edge, isRemote, remoteDriverAccessModel);

        public static IWebDriver GetFirefoxBrowser(bool isRemote = false, RemoteDriverAccessModel? remoteDriverAccessModel = null) =>
            new SeleniumDriverMiddleware().GetDriver(BrowserType.FireFox, isRemote, remoteDriverAccessModel);
        
        public static IWebDriver GetSafariBrowser(bool isRemote = false, RemoteDriverAccessModel? remoteDriverAccessModel = null) => 
            new SeleniumDriverMiddleware().GetDriver(BrowserType.Safari, isRemote, remoteDriverAccessModel);

        public static IWebDriver GetInternetExplorerBrowser(bool isRemote = false, RemoteDriverAccessModel? remoteDriverAccessModel = null) =>
            new SeleniumDriverMiddleware().GetDriver(BrowserType.InternetExplorer, isRemote, remoteDriverAccessModel);

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
                        PjUtility.Runtime.Logger.Log($"Unable to get browser info due to access issues. {e.Message}", e);
                    }
                    catch (Exception e)
                    {
                        PjUtility.Runtime.Logger.Log($"Unable to get browser info due to exception : {e.Message}", e);
                    }
                }
                return _installedBrowsers;
            }
        }
    }
}
