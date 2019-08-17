using TechTalk.SpecFlow;

namespace Selenium.Essentials.SampleTest.Core
{
    [Binding]
    public class SpecFlowWebTestHooks
    {
        [BeforeScenario]
        public void BeforeScenario()
        {
            WebUnitTestBase.Setup();
        }

        [AfterScenario]
        public void AfterScenario()
        {
            WebUnitTestBase.TearDown();
        }

        [Given(@"I open a new browser of type (.*)")]
        public static void GivenIOpenANewBrowserOfTypeChrome(string browserType)
        {
            TestUtility.InitializeDriver(browserType);
        }
    }
}
