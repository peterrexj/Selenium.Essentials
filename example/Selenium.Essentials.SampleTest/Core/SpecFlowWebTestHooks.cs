using System;
using TechTalk.SpecFlow;
using TestAny.Essentials.Core;

namespace Selenium.Essentials.SampleTest.Core
{
    [Binding]
    public class SpecFlowWebTestHooks : WebUnitTestBase
    {
        private WebTests.PageObjects.Wikipedia.MainPage _wikiMainPage;
        protected WebTests.PageObjects.Wikipedia.MainPage WikipediaMainPage =>
            _wikiMainPage ?? (_wikiMainPage = new WebTests.PageObjects.Wikipedia.MainPage(_driver));

        [BeforeFeature]
        public static void BeforeFeature()
        {
            if (TestAnyTestContextHelper.ExistsGlobalContext("BuildId") == false)
            {
                TestAnyTestContextHelper.SetGlobalContext("BuildId", Guid.NewGuid().ToString());
            }
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            Setup();
        }

        [AfterScenario]
        public void AfterScenario()
        {
            TearDown();
        }
    }
}
