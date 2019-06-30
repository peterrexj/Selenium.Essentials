using FluentAssertions;
using NUnit.Framework;
using Selenium.Essentials.Web;
using System;
using System.Collections.Generic;
using System.Text;

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
            BrowserHelper.InstalledBrowsers.Count
                .Should()
                .BeGreaterThan(0, because: "Expecting at least one browser is installed on the host computer");
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
    }
}
