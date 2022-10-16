using NUnit.Framework;
using Pj.Library;
using Selenium.Essentials.SampleTest.Core;
using Selenium.Essentials.SampleTest.WebTests.PageObjects.Wikipedia;
using System.Linq;

namespace Selenium.Essentials.SampleTest.WebTests.nUnit
{
    [TestFixture, Order(2)]
    [Parallelizable(ParallelScope.All)]
    public class WikipediaTests : WebUnitTestBase
    {
        [TestCaseSource(typeof(CaseCommonDataSource), "BrowserCapabilities")]
        public void NavigateToWikipedia(string browserType)
        {
            _driver = TestUtility.InitializeDriver(browserType);
            var _wikiMainPage = new MainPage(_driver);
            _wikiMainPage.Navigate();
        }
    }
}
