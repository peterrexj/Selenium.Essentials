using NUnit.Framework;
using Selenium.Essentials.SampleTest.Core;

namespace Selenium.Essentials.SampleTest.IntegrationTests.Tests
{
    public class SampleTest
    {
        [TestCase]
        public void Test()
        {
            var envData = TestUtility.EnvData;
            Assert.IsNotEmpty(envData);
        }
    }
}
