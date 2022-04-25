using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Selenium.Essentials.SampleTest.WebTests.PageObjects.Wikipedia
{
    public class MainPage : PageBase
    {
        private string _path => $"{TestUtility.EnvData["WikipediaDomain"]}/wiki/Main_Page";

        public MainPage(IWebDriver driver) : base(driver) { }

        private UnorderedListControl _tabNavigation => new UnorderedListControl(_driver, By.CssSelector("nav[id$='p-namespaces'] ul.vector-menu-content-list"));
        private TableControl _tableMainContent => new TableControl(_driver, By.Id("mp-upper"));

        public void Navigate()
        {
            base.Navigate(_path);
        }

        public void SelectMainPageTab(string tabText)
        {
            Assert.Greater(_tabNavigation.TotalItems, 0, "The tab on the wikipedia main page is missing");

            Assert.IsTrue(_tabNavigation.List.Any(p => p.Text.Contains(tabText)), $"The Wikipedia main page is missing with a tab text '{tabText}'");

            _tabNavigation.List
                .Where(p => p.Text.Contains(tabText))
                .FirstOrDefault()
                .Click();
        }
    }
}
