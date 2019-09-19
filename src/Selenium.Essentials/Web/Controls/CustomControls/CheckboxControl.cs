using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Essentials
{
    /// <summary>
    /// Checkbox control to use in page object.
    /// </summary>
    public class CheckboxControl : BaseControl, IEditableControl
    {
        public CheckboxControl(IWebDriver driver, By by, BaseControl parentControl = null, string description = null)
            : base(driver, by, parentControl, description)
        {
        }

        /// <summary>
        /// Check the checkbox in the UI.
        /// If the checkbox is not already checked, it will try to click on the [ClickableElement] of this control, which make it Check on the UI
        /// </summary>
        public void Check()
        {
            if (!IsChecked)
            {
                ClickableElement.WaitUntilElementCssDisplayed();
                ClickableElement.Click();
            }
        }

        /// <summary>
        /// UnCheck the checkbox in the UI.
        /// If the checkbox is already checked, it will try to click on the [ClickableElement] of this control, which make it UnCheck on the UI
        /// </summary>
        public void Uncheck()
        {
            if (IsChecked)
            {
                ClickableElement.WaitUntilElementCssDisplayed();
                ClickableElement.Click();
            }
        }

        /// <summary>
        /// Returns true if the checkbox is checked else false
        /// </summary>
        public bool IsChecked => RawElement.Selected;

        /// <summary>
        /// Returns the label element associated with the checkbox if there is one or else return the checkbox
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

        /// <summary>
        /// Tries to return the label by getting the parent element. You can also try [ClickableElement] to get the associated label
        /// </summary>
        private WebControl CheckBoxLabel => new WebControl(Driver, By.XPath(".."), this); 

        /// <summary>
        /// Check or uncheck based on the value passed
        /// </summary>
        /// <param name="value">pass 'check' or 'checked' to check the checkbox</param>
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

        /// <summary>
        /// Returns the current state of the checkbox value
        /// </summary>
        /// <returns></returns>
        public string Get() => IsChecked.ToString();

        /// <summary>
        /// Highlights the checkbox label
        /// </summary>
        public override void Highlight() => CheckBoxLabel.Highlight();
    }
}
