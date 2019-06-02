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
        public readonly IWebDriver _driver;

        /// <summary>
        /// Returns the selector how the element is used to access from the UI
        /// </summary>
        public By By { get; set; }
        public string ElementId => RawElement.GetAttribute("id");
        public IWebElement RawElement => RetryFindElement();
        protected BaseControl ParentControl { get; set; }
        public IWebElement ParentElement => RawElement.FindElement(By.XPath(".."));
        public string Description { get; protected set; }

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
            _driver = driver;
            var computedXpath = string.Empty;
            if (firstAvailable)
            {
                computedXpath = parentControl != null ?
                    parentControl.RawElement.FindElements(by).FirstOrDefault(d => d.Displayed)?.GetElementXPath(_driver) :
                    _driver.FindElements(by).FirstOrDefault(d => d.Displayed)?.GetElementXPath(_driver);
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

        private IWebElement RetryFindElement(int waitTimeSec = 1, int retryCount = 2)
        {
            for (var i = 0; i < retryCount + 1; i++)
            {
                try
                {
                    return ParentControl != null ? ParentControl.RawElement.FindElement(By) : _driver.FindElement(By);
                }
                catch (System.Net.WebException ex)
                {
                    if (i == retryCount)
                    {
                        throw new WebControlException(_driver, ex, uiControl: this);
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

            throw new WebControlException(_driver, "Retry failed: this should never go here", uiControl: this);
        }

        #region Find and Wait
        /// <summary>
        /// This element is useful if you have two elements 
        /// </summary>
        public static IBaseControl FindEitherElement(params IBaseControl[] controlsToFind)
        {
            foreach (IBaseControl ctrl in controlsToFind)
            {
                try
                {
                    // This will try to do FindElement. If it throws an exception we assume it doesn't exist.
                    var el = ctrl.RawElement;
                    if (ctrl.Exists && ctrl.CssDisplayed) return ctrl;
                }
                catch
                {
                    // Continue looping through all the locators
                }
            }

            return null;
        }

        

        

        

        

        

        

        

        public bool WaitForElementTextTrimEquals(string str)
        {
            return WaitForElementTextTrimEquals(str, AppConfig.DefaultTimeoutWaitPeriodInSeconds);
        }

        public bool WaitForElementTextTrimEquals(string str, int waitTimeSec)
        {
            return WaitForElementTextTrimEquals(str, waitTimeSec, true);
        }

        public bool WaitForElementTextTrimEquals(string str, int waitTimeSec, bool throwExceptionWhenNotFound = true)
        {
            return WaitGeneric(waitTimeSec, throwExceptionWhenNotFound, "", () => RawElement.Text.Trim() == str, $"not contain the string '{str}'");
        }

        /// <summary>
        /// This function will search for a string that starts with the passed parameter (str) in the element specified.
        /// </summary>
        public bool WaitForElementTextStartsWith(string str)
        {
            return WaitForElementTextStartsWith(str, AppConfig.DefaultTimeoutWaitPeriodInSeconds);
        }

        /// <summary>
        /// This function will search for a string that starts with the passed parameter (str) in the element specified.
        /// </summary>
        /// <param name="str">String to find</param>
        /// <param name="waitTimeSec">Will wait for specific period of time for the element to appear in the page</param>
        public bool WaitForElementTextStartsWith(string str, int waitTimeSec, bool throwExceptionWhenNotFound = true)
        {
            return WaitGeneric(waitTimeSec, throwExceptionWhenNotFound, "", () => RawElement.Text.Trim().StartsWith(str), $"not start with the string '{str}'");
        }

        /// <summary>
        /// This function will search for any string that starts with the passed parameter (str) in the element specified.
        /// </summary>
        public bool WaitForElementTextStartsWith(string[] str)
        {
            return WaitForElementTextStartsWith(str, AppConfig.DefaultTimeoutWaitPeriodInSeconds);
        }

        /// <summary>
        /// This function will search for any string that starts with the passed parameter (str) in the element specified.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="waitTimeSec"></param>
        public bool WaitForElementTextStartsWith(string[] str, int waitTimeSec, bool throwExceptionWhenNotFound = true)
        {
            return WaitGeneric(waitTimeSec, throwExceptionWhenNotFound, "", () => str.Any(s => RawElement.Text.Trim().StartsWith(s)), $"not contain any of the string '{string.Join(",", str)}'");
        }


        /// <summary>
        /// This function will search for a string that contains the passed parameter (str) in the element specified.
        /// </summary>
        /// <param name="str">String to find</param>
        public bool WaitForElementTextContains(string str)
        {
            return WaitForElementTextContains(str, AppConfig.DefaultTimeoutWaitPeriodInSeconds);
        }

        /// <summary>
        /// This function will search for a string that contains the passed parameter (str) in the element specified.
        /// </summary>
        /// <param name="str">String to find</param>
        /// <param name="waitTimeSec">Will wait for specific period of time for the element to appear in the page</param>
        public bool WaitForElementTextContains(string str, int waitTimeSec)
        {
            return WaitForElementTextContains(str, waitTimeSec, true);
        }

        public bool WaitForElementTextContains(string str, int waitTimeSec, bool throwExceptionWhenNotFound = true)
        {
            return WaitGeneric(waitTimeSec, throwExceptionWhenNotFound, "", () => RawElement.Text.Contains(str), $"not contain the string '{str}'");
        }

        public bool WaitForElementHasSomeText()
        {
            return WaitForElementHasSomeText(AppConfig.DefaultTimeoutWaitPeriodInSeconds);
        }

        public bool WaitForElementHasSomeText(int waitTimeSec)
        {
            return WaitForElementHasSomeText(waitTimeSec, true);
        }

        public bool WaitForElementHasSomeText(int waitTimeSec, bool throwExceptionWhenNotFound = true)
        {
            return WaitGeneric(waitTimeSec, throwExceptionWhenNotFound, "", () => RawElement.Text.HasValue(), $"not contain any value yet");
        }
        #endregion

        #region Control Properties

        public bool IsReadonly => RawElement.IsReadonly();
        public bool IsDisabled => RawElement.IsDisabled();
        public bool IsEnabled => RawElement.IsEnabled();

        /// <summary>
        /// Returns the raw text available within the control
        /// </summary>
        public string Text
        {
            get
            {
                try
                {
                    if (RawElement.Text.HasValue())
                    {
                        return RawElement.Text;
                    }

                    try
                    {
                        if (RawElement.GetAttribute("value").HasValue())
                        {
                            return RawElement.GetAttribute("value");
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }

                    return "";
                }
                catch (Exception ex)
                {
                    Highlight();
                    throw new WebControlException(_driver, ex, "Cannot get Text from the control.", this);
                }
            }
        }

        /// <summary>
        /// Returns value for the HTML attribute [Value] of the control. Mainly used with INPUT controls
        /// </summary>
        public string Value => RawElement.GetAttribute("value");

        /// <summary>
        /// Elements exists in the DOM. This element may or may not be visible.
        /// </summary>
        public bool Exists
        {
            get
            {
                try
                {
                    return RawElement != null; //RawElement.Displayed || RawElement.GetAttribute("displa")
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// If this element exists and displayed:none; is set on the CSS for that element then this method return false.
        /// </summary>
        public bool CssDisplayed
        {
            get
            {
                if (Exists)
                {
                    return !RawElement.GetCssValue("display").Equals("none", StringComparison.InvariantCultureIgnoreCase);
                }

                return false;
            }
        }

        /// <summary>
        /// Element is displayed in the page. Uses the IWebElement.Displayed property and checks the css attribute for display:none is not applied
        /// </summary>
        public bool IsDisplayed => RawElement.Displayed && CssDisplayed;

        /// <summary>
        /// Element is not shown in the page.
        /// </summary>
        public bool IsHidden => !IsDisplayed;
        #endregion

        #region Control Methods

        public void ClickByJsScript() => _driver.ExecuteJavaScript("arguments[0].click();", RawElement);

        public virtual void WaitAndClick(int wait = 0)
        {
            if (wait != 0)
            {
                WaitForElementInvisible(wait, throwExceptionWhenNotFound: false);
            }
            WaitForElementClickable();
            Click();
        }

        public virtual void WaitTillInvisible()
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
        public virtual void Click()
        {
            if (IsDisabled)
            {
                Highlight();
                throw new WebControlException(_driver, "Control is disabled.", this);
            }

            if (!Exists)
            {
                throw new WebControlException(_driver, "Control does not exist.", this);
            }

            try
            {
                RawElement.Click();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("is not clickable at point") && ex.Message.Contains("Other element would receive the click"))
                {
                    //ContextHelper.Log($"Element could not be clicked due to: {ex.Message}");
                    ParentElement.Click();
                }
                else
                {
                    Highlight();
                    throw new WebControlException(_driver, ex, "Control could not be clicked on", this);
                }
            }
        }

        public void ScrollAndClick()
        {
            ScrollTo();
            Click();
        }

        public void ScrollTo()
        {
            _driver.ExecuteJavaScript("arguments[0].scrollIntoView(true);", RawElement);
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
                throw new WebControlException(_driver, ex, "Control could not be cleared.", this);
            }
        }

        public virtual void Highlight() => RawElement.Highlight(_driver);

        public virtual void SendKeys(string text)
        {
            if (IsDisabled)
            {
                Highlight();
                throw new WebControlException(_driver, GetType().Name + " is disabled.", this);
            }

            try
            {
                RawElement.SendKeys(text);
            }
            catch (Exception ex)
            {
                Highlight();
                throw new WebControlException(_driver, ex, $"Failed to do SendKeys on control {GetType().Name}.", this);
            }
        }

        public virtual void SendEnter() => SendKeys(Keys.Enter);

        public virtual void SendTab() => SendKeys(Keys.Tab);

        public virtual void SetFocusByJavascript()
        {
            var findElementCode = $"(document.evaluate('{RawElement.GetElementXPath(_driver)}', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue).focus();";
            _driver.ExecuteJavaScript(findElementCode);
        }

        #endregion
    }
}
