using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Essentials
{
    public class CheckboxControl : BaseControl, IEditableControl
    {
        public CheckboxControl(IWebDriver driver, By by, BaseControl parentControl = null, string description = null)
            : base(driver, by, parentControl, description)
        {
        }

        public void Check()
        {
            if (!IsChecked)
            {
                ClickableElement.WaitForElementCssDisplayed();
                ClickableElement.Click();
            }
        }

        public void Uncheck()
        {
            if (IsChecked)
            {
                ClickableElement.WaitForElementCssDisplayed();
                ClickableElement.Click();
            }
        }

        public bool IsChecked => RawElement.Selected;

        public BaseControl ClickableElement
        {
            get
            {
                if (CheckBoxLabel.RawElement.TagName.Equals("label"))
                {
                    return CheckBoxLabel;
                }

                return this;
            }
        }

        private WebControl CheckBoxLabel => new WebControl(Driver, By.XPath(".."), this); 

        public void Set(string value)
        {
            if (value.Equals("checked", StringComparison.InvariantCultureIgnoreCase) || value.Equals("check", StringComparison.InvariantCultureIgnoreCase))
            {
                value = "true";
            }
            if (!string.IsNullOrEmpty(value))
            {
                bool result = false;
                bool.TryParse(value, out result);

                if (result)
                    this.Check();
                else
                    this.Uncheck();
            }
        }
        public string Get() => IsChecked.ToString();

        public override void Highlight() => CheckBoxLabel.Highlight();
    }
}
