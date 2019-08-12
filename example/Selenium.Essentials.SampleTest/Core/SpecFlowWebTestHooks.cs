using TechTalk.SpecFlow;

namespace Selenium.Essentials.SampleTest.Core
{
    [Binding]
    public class SpecFlowWebTestHooks
    {
        [BeforeScenario]
        public void BeforeScenario()
        {
            new WebUnitTestBase().Setup();
        }

        [AfterScenario]
        public void AfterScenario()
        {
            new WebUnitTestBase().TearDown();
        }

        [Given(@"I open a new browser of type (.*)")]
        public void GivenIOpenANewBrowserOfTypeChrome(string browserType)
        {
            TestUtility.InitializeDriver(browserType);
        }
    }
}
