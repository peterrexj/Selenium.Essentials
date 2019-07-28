using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Essentials.Web.Controls
{
    public class Checkbox : BaseControl, IEditableControl
    {
        public Checkbox(IWebDriver driver, By by, BaseControl parentControl = null, string description = null)
            : base(driver, by, parentControl, description)
        {
        }

        public bool IsChecked => RawElement.Selected;

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

        /// <summary>
        /// This is introduced with Selenium version 2.48.2 where the element inside another element cannot be clicked
        /// For example:
        /// <lable><input type='checkbox'></input></lable>
        /// Require since the clickable element will be used to check the availabilty of the control in the UI, since the actual input field will be hidden
        /// </summary>
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

        private WebControl CheckBoxLabel
        {
            get { return new WebControl(Driver, By.XPath(".."), this); }
        }

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

        public override void Highlight()
        {
            CheckBoxLabel.Highlight();
        }

        public string Get()
        {
            return this.IsChecked.ToString();
        }
    }
}
