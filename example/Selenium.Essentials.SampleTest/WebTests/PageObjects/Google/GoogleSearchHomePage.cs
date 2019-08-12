using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials.SampleTest.WebTests.PageObjects.Google
{
    public class GoogleSearchHomePage : PageBase
    {
        private string _url = "https://www.google.com";

        public GoogleSearchHomePage(IWebDriver driver) : base(driver) { }

        public override void Navigate(string path = "")
        {
            base.Navigate(_url);
        }

        private TextboxControl _searchInputControl => new TextboxControl(_driver,
            By.XPath("//input[contains(@title, 'Search')]"),
            firstAvailable: true,
            useExtendedClear: true);

        private ButtonControl _googleSearchButton => new ButtonControl(_driver,
            By.XPath("//input[@value = 'Google Search']"));

        public void SetTextToSearch(string searchText)
        {
            _searchInputControl.WaitForElementVisible();
            _searchInputControl.Set(searchText);
        }

        public void ClickToSearch()
        {
            _googleSearchButton.WaitForElementVisible();
            _googleSearchButton.Click();
        }
    }
}
