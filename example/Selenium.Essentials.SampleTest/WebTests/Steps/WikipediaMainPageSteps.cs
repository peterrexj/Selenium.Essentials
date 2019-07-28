using Selenium.Essentials.SampleTest.Core;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;

namespace Selenium.Essentials.SampleTest.WebTests.Steps
{
    [Binding]
    internal class WikipediaMainPageSteps : FeatureStepsBase
    {
        [Given(@"I have navigated to Wikipedia")]
        public void GivenIHaveNavigatedToWikipedia()
        {
            WikipediaMainPage.Navigate();
        }

        [When(@"I have selected Wikipedia Main Page tab")]
        public void WhenIHaveSelectedWikipediaMainPageTab()
        {
            WikipediaMainPage.SelectMainPageTab("Main Page");
        }

        [Then(@"I should see the Wikipedia Welcome content")]
        public void ThenIShouldSeeTheWikipediaWelcomeContent()
        {
        }

        [Then(@"I should see the Wikipedia today's feature")]
        public void ThenIShouldSeeTheWikipediaTodaySFeature()
        {
        }

    }
}
