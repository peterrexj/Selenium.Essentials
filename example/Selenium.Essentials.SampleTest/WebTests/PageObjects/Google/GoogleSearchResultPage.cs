using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials.SampleTest.WebTests.PageObjects.Google
{
    public class GoogleSearchResultPage : PageBase
    {
        public GoogleSearchResultPage(IWebDriver driver) : base(driver) { }

        private CollectionControl _searchResultsHeader => new CollectionControl(_driver,
            By.CssSelector("div.g div.rc div.r > a > h3"));

        private CollectionControl _searchResultLink => new CollectionControl(_driver,
            By.CssSelector("div.g div.rc div.r > a"));

        public int GetResultHeaderPosition(string headerToFindInPage)
        {
            _searchResultsHeader.WaitForElementVisible(position: 1);
            return _searchResultsHeader.FindPositionByText(headerToFindInPage);
        }

        public void ClickOnResult(int position)
        {
            _searchResultLink.Item<LinkControl>(position).Href.Should().NotBeEmpty("The hyperlink on the result is empty");
            _searchResultLink.Item<LinkControl>(position).Click();
        }
    }
}
