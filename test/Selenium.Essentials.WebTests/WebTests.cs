using FluentAssertions;
using NUnit.Framework;
using Selenium.Essentials.Web;
using Selenium.Essentials.WebTests.PageObjects.Google;
using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials.WebTests
{
    public class WebTests : TestBase
    {
        [SetUp]
        public void Setup()
        {
            base.InitlializeWebTest();
        }

        [TearDown]
        public void TearDown()
        {
            base.TearDownTest();
        }

        [Test]
        [TestCase("Microsoft Support", "Microsoft Software Support and Product Help")]
        [TestCase("nuget package docs", "NuGet Documentation | Microsoft Docs")]
        public void GoogleSearch(string searchText, string searchResultExpectedHeader)
        {
            GoogleSearchHomePage googleSearchHomePage = new GoogleSearchHomePage(TestContextHelper.Driver);
            googleSearchHomePage.Navigate();
            googleSearchHomePage.SetTextToSearch(searchText);
            googleSearchHomePage.ClickToSearch();

            GoogleSearchResultPage googleSearchResultPage = new GoogleSearchResultPage(TestContextHelper.Driver);
            var contextToClickPosition = googleSearchResultPage.GetResultHeaderPosition(searchResultExpectedHeader);
            contextToClickPosition.Should().BeGreaterOrEqualTo(1, $"Cannot find the {searchResultExpectedHeader} in google search");
            googleSearchResultPage.ClickOnResult(searchResultExpectedHeader);
        }

    }
}
