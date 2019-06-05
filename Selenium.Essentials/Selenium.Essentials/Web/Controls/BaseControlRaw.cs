using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Selenium.Essentials.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Essentials.Web.Controls
{
    public abstract partial class BaseControl : IBaseControl
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="by"></param>
        /// <param name="parentControl"></param>
        /// <param name="description"></param>
        /// <param name="firstAvailable">Finds the first element from the DOM</param>
        protected BaseControl(IWebDriver driver, By by = null, BaseControl parentControl = null, string description = null, bool firstAvailable = false)
        {
            Driver = driver;
            var computedXpath = string.Empty;
            if (firstAvailable)
            {
                computedXpath = parentControl != null ?
                    parentControl.RawElement.FindElements(by).FirstOrDefault(d => d.Displayed)?.GetElementXPath(Driver) :
                    Driver.FindElements(by).FirstOrDefault(d => d.Displayed)?.GetElementXPath(Driver);
            }
            if (computedXpath.HasValue())
            {
                By = By.XPath(computedXpath);
            }
            else
            {
                By = by;
                ParentControl = parentControl;
            }
        }

        private IWebElement RetryFindElement(int waitTimeSec = 1, int retryCount = -1)
        {
            if (retryCount == -1) retryCount = AppConfig.DefaultRetryElementCount;

            for (var i = 0; i < retryCount + 1; i++)
            {
                try
                {
                    return ParentControl != null ? ParentControl.RawElement.FindElement(By) : Driver.FindElement(By);
                }
                catch (System.Net.WebException ex)
                {
                    if (i == retryCount)
                    {
                        throw new WebControlException(Driver, ex, uiControl: this);
                    }
                }
                catch (Exception ex)
                {
                    if (i == retryCount)
                    {
                        throw new Exception(ex.Message, ex);
                    }
                }

                // Short interval before a retry occurs
                System.Threading.Thread.Sleep(waitTimeSec * 200);
            }

            throw new WebControlException(Driver, $"Retry failed: Not able to find the element [{ToString()}] from the UI", uiControl: this);
        }


        #region Control Properties
        public IWebDriver Driver { get; set; }
        public IWebElement RawElement => RetryFindElement();
        public IBaseControl ParentControl { get; protected set; }
        public IWebElement ParentRawElement => RawElement.FindElement(By.XPath(".."));
        public string ElementId => RawElement.GetAttribute("id");
        public By By { get; protected set; }
        public string Description { get; protected set; }

        public bool IsReadonly => RawElement.IsReadonly();
        public bool IsDisabled => RawElement.IsDisabled();
        public bool IsEnabled => RawElement.IsEnabled();
        public bool IsVisible => RawElement.IsVisible();
        /// <summary>
        /// If this element exists and displayed:none; is set on the CSS for that element then this method return false.
        /// </summary>
        public bool CssDisplayed => RawElement.IsCssDisplayed();

        /// <summary>
        /// Element is displayed in the page. Uses the IWebElement.Displayed property and checks the css attribute for display:none is not applied
        /// </summary>
        public bool IsDisplayed => IsVisible;

        /// <summary>
        /// Element is not shown in the page.
        /// </summary>
        public bool IsHidden => !IsVisible;

        /// <summary>
        /// Elements exists in the DOM. This element may or may not be visible.
        /// </summary>
        public bool Exists => RawElement.Exists();

        /// <summary>
        /// Returns value for the HTML attribute [Value] of the control. Mainly used with INPUT controls
        /// </summary>
        public string Value => RawElement.Value();

        /// <summary>
        /// Returns the raw text available within the control
        /// </summary>
        public string Text => RawElement.Text(Driver);

        #endregion

        #region Control Click Methods
        public virtual void Click()
        {
            if (IsDisabled)
            {
                Highlight();
                throw new WebControlException(Driver, "Control is disabled.", this);
            }

            if (!Exists)
            {
                throw new WebControlException(Driver, "Control does not exist.", this);
            }

            try
            {
                RawElement.Click();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("is not clickable at point") && ex.Message.Contains("Other element would receive the click"))
                {
                    //LOG: The element could not be clicked as there is another element which would receive the click

                    ParentRawElement.Click();
                }
                else
                {
                    Highlight();
                    throw new WebControlException(Driver, ex, "Control could not be clicked on", this);
                }
            }
        }
        public virtual void WaitAndClick(int timeToWaitInSeconds = 0)
        {
            if (timeToWaitInSeconds != 0)
            {
                WaitForElementInvisible(timeToWaitInSeconds, throwExceptionWhenNotFound: false);
            }
            WaitForElementClickable();
            Click();
        }
        public virtual void ScrollAndClick()
        {
            ScrollTo();
            Click();
        }
        public virtual void ClickByJsScript() => Driver.ExecuteJavaScript("arguments[0].click();", RawElement);
        public virtual void WaitClickTillElementGoesInvisible()
        {
            WaitAndClick();

            if (!WaitForElementInvisible(5, throwExceptionWhenNotFound: false))
            {
                if (WaitForElementClickable(waitTimeSec: 5, throwExceptionWhenNotFound: false))
                {
                    WaitAndClick();
                }
            }
            WaitForElementInvisible();
        }
        public virtual void WaitClickAndIgnoreError()
        {
            try
            {
                WaitAndClick();
            }
            catch (Exception)
            {
                // ignored
            }
        }
        #endregion

        #region Control Set Methods

        public virtual void SendKeys(string textToSet)
        {
            if (IsDisabled)
            {
                Highlight();
                throw new WebControlException(Driver, $"{ToString()} is disabled.", this);
            }

            try
            {
                RawElement.SendKeys(textToSet);
            }
            catch (Exception ex)
            {
                Highlight();
                throw new WebControlException(Driver, ex, $"Failed to do SendKeys on control {ToString()}.", this);
            }
        }
        public virtual void SendEnter() => SendKeys(Keys.Enter);
        public virtual void SendTab() => SendKeys(Keys.Tab);

        #endregion

        public virtual void ScrollTo()
        {
            Driver.ExecuteJavaScript("arguments[0].scrollIntoView(true);", RawElement);
        }
        public virtual void Clear()
        {
            try
            {
                RawElement.Clear();

                if (Value.HasValue())
                {
                    RawElement.SendKeys(Keys.Control + "a");
                    RawElement.SendKeys(Keys.Delete);
                }

                Value.Should().BeEmpty("Control was not cleared");
            }
            catch (Exception ex)
            {
                Highlight();
                throw new WebControlException(Driver, ex, "Control could not be cleared.", this);
            }
        }
        public virtual void Highlight() => RawElement.Highlight(Driver);
        public virtual void SetFocusByJavascript()
        {
            var findElementCode = $"(document.evaluate('{RawElement.GetElementXPath(Driver)}', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue).focus();";
            Driver.ExecuteJavaScript(findElementCode);
        }




        public override string ToString()
        {
            return $"Type: [{GetType().Name}], Selector: [{By}], Description: [{Description}]";
        }
    }
}
