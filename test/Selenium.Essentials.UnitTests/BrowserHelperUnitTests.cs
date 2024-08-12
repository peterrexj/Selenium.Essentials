using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;

namespace Selenium.Essentials.UnitTests
{
    public class BrowserHelperUnitTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_InstalledBrowser()
        {
            Assert.GreaterOrEqual(BrowserHelper.InstalledBrowsers.Count, 0, "Expecting at least one browser is installed on the host computer");
        }

        [Test]
        [TestCase("", ExpectedResult = BrowserType.Chrome)]
        [TestCase("chrome", ExpectedResult = BrowserType.Chrome)]
        [TestCase("Chrome", ExpectedResult = BrowserType.Chrome)]
        [TestCase("CHROME", ExpectedResult = BrowserType.Chrome)]
        [TestCase("incorrect", ExpectedResult = BrowserType.Chrome)]
        [TestCase("ie", ExpectedResult = BrowserType.InternetExplorer)]
        [TestCase("IE", ExpectedResult = BrowserType.InternetExplorer)]
        [TestCase("internet explorer", ExpectedResult = BrowserType.InternetExplorer)]
        [TestCase("Internet Explorer", ExpectedResult = BrowserType.InternetExplorer)]
        [TestCase("internetexplorer", ExpectedResult = BrowserType.InternetExplorer)]
        [TestCase("Safari", ExpectedResult = BrowserType.Safari)]
        [TestCase("safari", ExpectedResult = BrowserType.Safari)]
        [TestCase("edge", ExpectedResult = BrowserType.Edge)]
        [TestCase("Edge", ExpectedResult = BrowserType.Edge)]
        [TestCase("EDGE", ExpectedResult = BrowserType.Edge)]
        [TestCase("ms edge", ExpectedResult = BrowserType.Edge)]
        [TestCase("msedge", ExpectedResult = BrowserType.Edge)]
        public BrowserType Test_BrowserTypes(string browserType) 
            => BrowserHelper.GetBrowserType(browserType);


        [Test]
        public void CustomChromeCapabilitiesTest()
        {
            BrowserHelper.GetDriver(BrowserType.Chrome)
            SeleniumDriverCapabilitiesProvider caps = new()
            {
                ChromeOptions = new ChromeOptions()
            };

            var options = caps.GetChrome(isRemote: false, null);

            Assert.IsNotNull(options);
            Assert.LessOrEqual(options.Arguments.Count, 0); //arguments will be empty as the chromeoption is custom
        }

        [Test]
        public void CustomFirefoxCapabilitiesTest()
        {
            SeleniumDriverCapabilitiesProvider caps = new()
            {
                FirefoxOptions= new FirefoxOptions()
            };

            var options = caps.GetFirefox(isRemote: false, null);

            Assert.IsNotNull(options);
            Assert.IsNull(options.AcceptInsecureCertificates);
        }

        [Test]
        public void CustomEdgeCapabilitiesTest()
        {
            SeleniumDriverCapabilitiesProvider caps = new()
            {
                EdgeOptions = new EdgeOptions()
            };

            var options = caps.GetEdge(isRemote: false, null);

            Assert.IsNotNull(options);
            Assert.IsNull(options.AcceptInsecureCertificates);
        }

        [Test]
        public void CustomSafariCapabilitiesTest()
        {
            SeleniumDriverCapabilitiesProvider caps = new()
            {
                SafariOptions = new SafariOptions()
            };

            var options = caps.GetSafari(isRemote: false, null);

            Assert.IsNotNull(options);
            Assert.IsNull(options.AcceptInsecureCertificates);
        }

        [Test]
        public void CustomInternetExplorerCapabilitiesTest()
        {
            SeleniumDriverCapabilitiesProvider caps = new()
            {
                InternetExplorerOptions = new InternetExplorerOptions()
            };

            var options = caps.GetInternetExplorer(isRemote: false, null);

            Assert.IsNotNull(options);
            Assert.IsNull(options.AcceptInsecureCertificates);
        }
    }
}
