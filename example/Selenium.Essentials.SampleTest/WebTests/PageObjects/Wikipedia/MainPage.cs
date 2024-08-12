using NUnit.Framework;
using OpenQA.Selenium;
using Selenium.Essentials.SampleTest.Core;
using System.Linq;

namespace Selenium.Essentials.SampleTest.WebTests.PageObjects.Wikipedia
{
    public class MainPage : PageBase
    {
        private static string Path => $"{TestUtility.EnvData["WikipediaDomain"]}/wiki/Main_Page";

        public MainPage(IWebDriver driver) : base(driver) { }

        private UnorderedListControl _tabNavigation => new UnorderedListControl(_driver, By.CssSelector("nav[aria-label$='Namespaces'] ul.vector-menu-content-list"));
        private TableControl _tableMainContent => new TableControl(_driver, By.Id("mp-upper"));
        private WebControl OtherAreaOfWikipediaContainer => new WebControl(_driver, By.Id("mp-other-content"));
        public CollectionControl OtherAreaOfWikipediaItems => new CollectionControl(_driver, By.TagName("li"), parentControl: OtherAreaOfWikipediaContainer);

        public void Navigate()
        {
            base.Navigate(Path);
        }

        public void SelectMainPageTab(string tabText)
        {
            Assert.Greater(_tabNavigation.TotalItems, 0, "The tab on the wikipedia main page is missing");

            Assert.IsTrue(_tabNavigation.List.Any(p => p.Text.Contains(tabText)), $"The Wikipedia main page is missing with a tab text '{tabText}'");

            _tabNavigation.List
                .FirstOrDefault(p => p.Text.Contains(tabText))
                ?.Click();
        }
    }
}
