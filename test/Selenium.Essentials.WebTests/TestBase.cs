using Selenium.Essentials.Core;
using Selenium.Essentials.Web;
using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials.WebTests
{
    public class TestBase
    {
        public virtual void InitlializeWebTest()
        {
            Utility.InitializeFramework();

            TestContextHelper.Set("Driver", new BrowserHelper().GetChromeBrowser());
            TestContextHelper.Driver.Manage().Timeouts().PageLoad = (TimeSpan.FromSeconds(120));

        }

        public virtual void TearDownTest()
        {
            if (TestContextHelper.Driver != null)
            {
                TestContextHelper.Driver.Quit();
            }
        }
    }
}
