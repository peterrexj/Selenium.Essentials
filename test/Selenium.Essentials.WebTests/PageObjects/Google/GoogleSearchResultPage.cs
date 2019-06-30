using FluentAssertions;
using OpenQA.Selenium;
using Selenium.Essentials.Web.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials.WebTests.PageObjects.Google
{
    public class GoogleSearchResultPage : PageBase
    {
        public GoogleSearchResultPage(IWebDriver driver)
        {

        }

        private CollectionControl _searchResultsHeader => new CollectionControl(_driver,
            By.CssSelector("div.g div.rc div.r > a > h3"));

        private CollectionControl _searchResultLink => new CollectionControl(_driver,
            By.CssSelector("div.g div.rc div.r > a"));

        public int GetResultHeaderPosition(string headerToFindInPage)
        {
            _searchResultsHeader.WaitForElementVisible();
            return _searchResultsHeader.FindPositionByText(headerToFindInPage);
        }

        public void ClickOnResult(string headerToFindInPage)
        {
            _searchResultsHeader.WaitForElementVisible();
            var position = _searchResultsHeader.FindPositionByText(headerToFindInPage);
            _searchResultLink.Item<LinkControl>(position).Href.Should().NotBeEmpty("The hyperlink on the result is empty");
            _searchResultLink.Item<LinkControl>(position).Click();
        }

    }
}
