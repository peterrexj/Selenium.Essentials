using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Essentials
{
    public class SelectControl : BaseControl, IEditableControl
    {
        public SelectControl(IWebDriver driver, By by, BaseControl parentControl = null, string description = null, bool firstAvailable = false)
            : base(driver, by, parentControl, description, firstAvailable)
        {
        }

        public void SelectByText(string textValue)
        {
            WaitUntilElementVisible();
            new SelectElement(RawElement).SelectByText(textValue);
        }
        public void SelectByTextPartial(string textValue, int preferredCompareLength = 12)
        {
            WaitUntilElementVisible();
            string textValuePartial = textValue.Length <= preferredCompareLength ? textValue : textValue.Substring(0, preferredCompareLength);
            new SelectElement(RawElement).SelectByText(textValuePartial, true);
        }
        public void SelectByValue(string textValue)
        {
            WaitUntilElementVisible();
            new SelectElement(RawElement).SelectByValue(textValue);
        }
        public void SelectByIndex(int index)
        {
            WaitUntilElementVisible();
            new SelectElement(RawElement).SelectByIndex(index);
        }

        public string GetCurrentSelectedOption()
        {
            WaitUntilElementVisible();
            return new SelectElement(RawElement).SelectedOption.Text;
        }

        #region IEditableControl
        public void Set(string value) => SelectByText(value);

        public string Get() => GetCurrentSelectedOption();
        #endregion

        public List<string> AvailableOptions => new SelectElement(RawElement).Options.Select(option => option.Text).ToList();
    }
}