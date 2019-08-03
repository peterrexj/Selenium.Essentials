using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;

namespace Selenium.Essentials.SampleTest.Core
{
    [Binding]
    public class SpecFlowWebTestHooks
    {
        [BeforeScenario]
        public void BeforeScenario()
        {
            Utility.InitializeFramework();

            TestContextHelper.Set("Driver", new BrowserHelper().GetChromeBrowser());
            TestContextHelper.Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(TestUtility.EnvData["PageLoadTimeoutInSeconds"].ToInteger());
        }

        [AfterScenario]
        public void AfterScenario()
        {
            if (TestContextHelper.Driver != null)
            {
                TestContextHelper.Driver.CloseDriver();
            }
        }
    }
}
