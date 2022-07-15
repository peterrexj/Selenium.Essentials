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
        private object _lock = new();

        [SetUp]
        public void Setup()
        {
            TestAnyAppConfig.InitializeFramework(new CutomLogger());
            lock (_lock)
            {
                if (TestAnyTestContextHelper.ExistsGlobalContext("BuildId") == false)
                {
                    TestAnyTestContextHelper.SetGlobalContext("BuildId", Guid.NewGuid().ToString());
                }
            }
        }

        [TearDown]
        public void TearDown()
        {
            var passed = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed;

            if (TestUtility.SessionDrivers.ContainsKey(TestContext.CurrentContext.TestName()))
            {
                var driver = TestUtility.SessionDrivers[TestContext.CurrentContext.TestName()];
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
                TestUtility.SessionDrivers.TryRemove(TestContext.CurrentContext.TestName(), out driver);
            }
        }
    }
}
