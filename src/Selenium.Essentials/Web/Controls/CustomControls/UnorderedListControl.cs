using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials
{
    public class UnorderedListControl : BaseControl
    {
        public UnorderedListControl(IWebDriver driver, By by, BaseControl parentControl = null, string description = null)
           : base(driver, by, parentControl, description)
        {
        }

        public int TotalItems => Driver.FindElements(By.XPath($"{XpathSelector}/li")).GetAllVisibleElements().Count;

        public IEnumerable<WebControl> List
        {
            get
            {
                for (int index = 1; index <= TotalItems; index++)
                {
                    yield return new WebControl(Driver, By.XPath($"{XpathSelector}/li[{index}]"));
                }
            }
        }
    }
}
