using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials.SampleTest.WebTests.PageObjects.Wikipedia
{
    public class MainPage : PageBase
    {
        private string _path => $"{TestUtility.EnvData["WikipediaDomain"]}/wiki/Main_Page";

        public MainPage(IWebDriver driver)
        {

        }

        public void Navigate()
        {
            base.Navigate(_path);
        }
    }
}
