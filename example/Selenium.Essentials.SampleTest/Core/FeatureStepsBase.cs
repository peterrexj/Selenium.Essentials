using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials.SampleTest.Core
{
    internal class FeatureStepsBase
    {
        private IWebDriver _driver;

        public FeatureStepsBase()
        {
            _driver = TestContextHelper.Driver;
        }

        public FeatureStepsBase(IWebDriver driver)
        {
            _driver = driver;
        }

        private WebTests.PageObjects.Wikipedia.MainPage _wikiMainPage;

        protected WebTests.PageObjects.Wikipedia.MainPage WikipediaMainPage => _wikiMainPage ?? (_wikiMainPage = new WebTests.PageObjects.Wikipedia.MainPage(_driver));
    }
}
