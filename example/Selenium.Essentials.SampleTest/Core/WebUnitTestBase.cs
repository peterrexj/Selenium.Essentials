using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials.SampleTest.Core
{
    public class WebUnitTestBase : TestApiBase
    {
        [SetUp]
        public void Setup()
        {
            Utility.InitializeFramework();
        }

        [TearDown]
        public void TearDown()
        {
            var passed = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed;
            TestContextHelper.Driver.ExecuteJavaScript("sauce:job-result=" + (passed ? "passed" : "failed"), supressErrors: true);

            if (TestContextHelper.Driver != null)
            {
                TestContextHelper.Driver.CloseDriver();
            }
        }
    }
}
