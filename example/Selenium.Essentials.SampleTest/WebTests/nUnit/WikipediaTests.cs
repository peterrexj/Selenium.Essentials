using NUnit.Framework;
using NUnit.Framework.Interfaces;
using Selenium.Essentials.SampleTest.Core;
using Selenium.Essentials.SampleTest.WebTests.PageObjects.Wikipedia;
using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials.SampleTest.WebTests.nUnit
{
    public class WikipediaTests : WebUnitTestBase
    {
        [TestCaseSource(typeof(CaseCommonDataSource), "BrowserCapabilities")]
        public void NavigateToWikipedia(string browserType)
        {
            TestUtility.InitializeDriver(browserType);
            var _wikiMainPage = new MainPage(TestContextHelper.Driver);
            _wikiMainPage.Navigate();
        }
    }
}
