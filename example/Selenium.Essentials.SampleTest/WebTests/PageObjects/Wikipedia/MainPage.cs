using FluentAssertions;
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

        private UnorderedListControl _tabNavigation => new UnorderedListControl(_driver, By.XPath("//div[@id='p-namespaces']/ul"));
        private TableControl _tableMainContent => new TableControl(_driver, By.Id("mp-upper"));

        public void Navigate()
        {
            base.Navigate(_path);
        }

        public void SelectMainPageTab(string tabText)
        {
            _tabNavigation.TotalItems
                .Should()
                .BeGreaterThan(0, "The tab on the wikipedia main page is missing");

            _tabNavigation.List
                .Any(p => p.Text.Contains(tabText))
                .Should()
                .BeTrue($"The Wikipedia main page is missing with a tab text '{tabText}'");

            _tabNavigation.List
                .Where(p => p.Text.Contains(tabText))
                .FirstOrDefault()
                .Click();
        }
    }
}
