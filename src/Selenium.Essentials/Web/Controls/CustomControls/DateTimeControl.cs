using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials
{
    public class DateTimeControl : BaseControl, IEditableControl
    {
        private readonly string _dateFormat = "";
        private string _dateTimeFormat => RawElement.GetAttribute("_dateFormat");

        public DateTimeControl(IWebDriver driver, By by, BaseControl parentControl = null, string description = null, string dateFormat = "data-format")
            : base(driver, by, parentControl, description)
        {
            _dateFormat = dateFormat;
        }


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
