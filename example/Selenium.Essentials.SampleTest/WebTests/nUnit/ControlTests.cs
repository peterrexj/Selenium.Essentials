using NUnit.Framework;
using Pj.Library;
using Selenium.Essentials.SampleTest.Core;
using Selenium.Essentials.SampleTest.WebTests.PageObjects.Wikipedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Essentials.SampleTest.WebTests.nUnit
{
    [TestFixture, Order(1)]
    [Parallelizable(ParallelScope.Self)]
    public class ControlTests : WebUnitTestBase
    {
        [TestCase]
        public void CollectionControl_ShouldVerify_InternalComputedProperties()
        {
            _driver = TestUtility.InitializeDriver("Chrome_Win10_latest");
            var wikiMainPage = new MainPage(_driver);
            wikiMainPage.Navigate();
            int position = 2;

            var total = wikiMainPage.OtherAreaOfWikipediaItems.Total;
            var totalRaw = wikiMainPage.OtherAreaOfWikipediaItems.TotalRaw;
            var firstControl = wikiMainPage.OtherAreaOfWikipediaItems.FirstVisibleElement;
            var visibleItem2Text = wikiMainPage.OtherAreaOfWikipediaItems.VisibleItem<WebControl>(position).Text;
            var itemPositionText = wikiMainPage.OtherAreaOfWikipediaItems.Item(position).Text;
            var itemPositionPosition = wikiMainPage.OtherAreaOfWikipediaItems.FindPositionByText(itemPositionText);
            var getTexts = wikiMainPage.OtherAreaOfWikipediaItems.Get().ToList();
            var getPositionText = wikiMainPage.OtherAreaOfWikipediaItems.Get(position);

            Assert.IsTrue(total > position && total < 20, $"Total items should fall between {position} and 20 for now");
            Assert.IsTrue(totalRaw > position && total < 20, $"Total Raw items should fall between {position} and 20 for now");
            Assert.IsNotNull(firstControl, "First element should not be null");
            Assert.IsTrue(visibleItem2Text.HasValue(), $"Visible Item at position {position} should have value");
            Assert.IsTrue(itemPositionText.HasValue(), $"Item at position {position} should have value");
            Assert.IsTrue(itemPositionText.HasValue(), $"Item at position {position} should have value");
            Assert.IsTrue(itemPositionPosition == position, $"FindPositionByText should match {position}");
            Assert.IsTrue(getTexts.Count == total, $"Get() total should be equal to the items total element");
            Assert.IsTrue(getPositionText == itemPositionText, $"Get(position) should be equal to the text found early at position");
        }
    }
}
