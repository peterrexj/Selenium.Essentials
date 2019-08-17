using FluentAssertions;
using NUnit.Framework;
using Selenium.Essentials.SampleTest.Core;
using Selenium.Essentials.SampleTest.WebTests.PageObjects.Google;
using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials.SampleTest.WebTests.nUnit
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class GoogleSearchPageTests : WebUnitTestBase
    {
        [TestCaseSource(typeof(CaseCommonDataSource), "BrowserCapabilitiesWithAdditionalParams", 
            new object[] { "Microsoft Support, Microsoft Software Support and Product Help" })]
        [TestCaseSource(typeof(CaseCommonDataSource), "BrowserCapabilitiesWithAdditionalParams", 
            new object[] { "nuget package docs, NuGet Documentation | Microsoft Docs" })]
        public void GoogleSearch(string browserType, string searchText, string searchResultExpectedHeader)
        {
            _driver = TestUtility.InitializeDriver(browserType);
            GoogleSearchHomePage googleSearchHomePage = new GoogleSearchHomePage(_driver);
            googleSearchHomePage.Navigate();
            googleSearchHomePage.SetTextToSearch(searchText);
            googleSearchHomePage.ClickToSearch();
        }

        public void GooglewSearchWithNavigate(string browserType, string searchText, string searchResultExpectedHeader)
        {
            _driver = TestUtility.InitializeDriver(browserType);
            GoogleSearchHomePage googleSearchHomePage = new GoogleSearchHomePage(_driver);
            googleSearchHomePage.Navigate();
            googleSearchHomePage.SetTextToSearch(searchText);
            googleSearchHomePage.ClickToSearch();

            GoogleSearchResultPage googleSearchResultPage = new GoogleSearchResultPage(_driver);
            var contextToClickPosition = googleSearchResultPage.GetResultHeaderPosition(searchResultExpectedHeader);
            contextToClickPosition.Should().BeGreaterOrEqualTo(1, $"Cannot find the {searchResultExpectedHeader} in google search");
            googleSearchResultPage.ClickOnResult(contextToClickPosition);
        }
    }
}
