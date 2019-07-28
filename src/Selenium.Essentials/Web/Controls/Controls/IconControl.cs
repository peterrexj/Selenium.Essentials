using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials
{
    public class IconControl : BaseControl
    {
        public IconControl(IWebDriver driver, By by, BaseControl parentControl = null, string description = null)
            : base(driver, by, parentControl, description)
        {
        }
    }
}
