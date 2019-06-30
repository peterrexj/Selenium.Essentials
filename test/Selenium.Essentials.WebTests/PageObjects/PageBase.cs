using OpenQA.Selenium;
using Selenium.Essentials.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials.WebTests.PageObjects
{
    public abstract class PageBase
    {
        protected IWebDriver _driver;

        public PageBase()
        {
            if (TestContextHelper.Driver != null)
            {
                _driver = TestContextHelper.Driver;
            }
        }

        public PageBase(IWebDriver driver)
        {
            _driver = driver;
        }

        public virtual void Navigate(string path)
        {
            TestContextHelper.Driver.Navigate().GoToUrl(path);
        }
    }
}
