using TechTalk.SpecFlow;

namespace Selenium.Essentials.SampleTest.Core
{
    [Binding]
    public class SpecFlowWebTestHooks : WebUnitTestBase
    {
        private WebTests.PageObjects.Wikipedia.MainPage _wikiMainPage;
        protected WebTests.PageObjects.Wikipedia.MainPage WikipediaMainPage => 
            _wikiMainPage ?? (_wikiMainPage = new WebTests.PageObjects.Wikipedia.MainPage(_driver));

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
