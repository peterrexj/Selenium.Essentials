using OpenQA.Selenium;
using Selenium.Essentials.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Essentials.Web
{
    public enum BrowserType
    {
        Chrome,
        FireFox,
        InternetExplorer,
        Edge,
        Safari,
    }


    public class BrowserHelper
    {
        private string _driverPath => Path.Combine(Utility.ExecutingFolder, "UI", "Drivers");

        public IWebDriver GetChromeBrowser() { return null; }
        public IWebDriver GetEdgeBrowser() { return null; }
        public IWebDriver GetFirefoxBrowser() { return null; }
        public IWebDriver GetSafariBrowser() { return null; }
        public IWebDriver GetInternetExplorerBrowser() { return null; }
    }
}
