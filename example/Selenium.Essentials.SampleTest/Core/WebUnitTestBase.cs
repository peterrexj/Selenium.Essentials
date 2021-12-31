using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using TestAny.Essentials.Api;
using TestAny.Essentials.Core;
using static Pj.Library.PjUtility;

namespace Selenium.Essentials.SampleTest.Core
{
    public class WebUnitTestBase : TestApiBase
    {
        protected IWebDriver _driver;

        [SetUp]
        public void Setup()
        {
            TestAnyAppConfig.InitializeFramework(new CutomLogger());
        }

        [TearDown]
        public void TearDown()
        {
            var passed = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed;

            if (TestUtility.SessionDrivers.ContainsKey(TestContext.CurrentContext.Test.Name))
            {
                var driver = TestUtility.SessionDrivers[TestContext.CurrentContext.Test.Name];
                if (driver != null)
                {
                    driver.ExecuteJavaScript("sauce:job-result=" + (passed ? "passed" : "failed"), supressErrors: true);
                    try
                    {
                        driver.CloseDriver();
                    }
                    catch (Exception ex)
                    {
                        Runtime.Logger.Log($"Unable to Close driver gracefully due to: {ex.ToString()}");
                    }
                }
                TestUtility.SessionDrivers.TryRemove(TestContext.CurrentContext.Test.Name, out driver);
            }
        }
    }
}
