using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials
{
    public class DateTimeControl : BaseControl, IEditableControl
    {
        public DateTimeControl(IWebDriver driver, By by, BaseControl parentControl = null, string description = null)
            : base(driver, by, parentControl, description)
        {
        }

        private string _dateTimeFormat => RawElement.GetAttribute("data-format");

        public string Get()
        {
            return Value;
        }

        public void Set(string value)
        {
            Clear();
            Click();
            SendKeys(value);
        }

        public void Set(DateTime value)
        {
            Set(value.ToString(_dateTimeFormat));
        }
    }
}
