using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials.Web.Controls.Controls
{
    public class RadioControl : BaseControl
    {
        public RadioControl(IWebDriver driver, By by, BaseControl parentControl = null, string description = null)
            : base(driver, by, parentControl, description)
        {
        }
    }
}
