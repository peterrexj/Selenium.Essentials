using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using Selenium.Essentials.SampleTest.Core;
using Selenium.Essentials.SampleTest.WebTests.PageObjects.Wikipedia;
using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials.SampleTest.WebTests.nUnit
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class WikipediaTests : WebUnitTestBase
    {
        [TestCaseSource(typeof(CaseCommonDataSource), "BrowserCapabilities")]
        public void NavigateToWikipedia(string browserType)
        {
            _driver = TestUtility.InitializeDriver(browserType);
            var _wikiMainPage = new MainPage(_driver);
            _wikiMainPage.Navigate();
        }
    }
}
