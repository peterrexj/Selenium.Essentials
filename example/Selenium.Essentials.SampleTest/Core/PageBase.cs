using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials.SampleTest
{
    public abstract class PageBase
    {
        protected IWebDriver _driver;

        public PageBase(IWebDriver driver)
        {
            _driver = driver;
        }

        public virtual void Navigate(string path)
        {
            _driver.Navigate().GoToUrl(path);
            _driver.WaitTillPageLoad();
        }
    }
}
