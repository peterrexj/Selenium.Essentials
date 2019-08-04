using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Essentials
{
    /// <summary>
    /// Abstract Base control which provides default properties and methods to all custom controls.
    /// The initalization process only sets the properties to find the control.
    /// The control will be fetched each time from the UI when it is accessed for any operations, that is when any 
    /// property or method is called on this control. The methods are all overridable by the custom controls which extends or inherits.
    /// For every new custom control which is created should have this class inherited and also the Control Factory should have
    /// a case to return the new custom control.
    /// </summary>
    public abstract partial class BaseControl : IBaseControl
    {
        /// <summary>
        /// Constructor for initializing the base control. 
        /// </summary>
        /// <param name="driver">The Selenium web driver object for the current execution</param>
        /// <param name="by">The selector by which the element will be fetch from the browser</param>
        /// <param name="parentControl">The parent control on which the element will be fetched using the selector</param>
        /// <param name="description">Description of the element.This description will be used when ToString called on this control</param>
        /// <param name="findFirstAvailable">Finds and returns the first available (displayed) element from the browser</param>
        protected BaseControl(IWebDriver driver, By by = null, BaseControl parentControl = null, string description = null, bool findFirstAvailable = false)
        {
            Driver = driver;
            FindFirstAvailable = findFirstAvailable;
            ParentControl = parentControl;
            Description = description;

            By = by;

            if (findFirstAvailable)
            {
                By = By.XPath(XpathSelector);
            }
        }

        /// <summary>
        /// Finds the element from the browser using retry logic
        /// </summary>
        /// <param name="waitTimeSec">Time to wait in between each retry logic</param>
        /// <param name="retryCount">Number of times to retry the element from the browser</param>
        /// <returns>IWebElement</returns>
        private IWebElement RetryFindElement(int waitTimeSec = 1, int retryCount = -1)
        {
            if (retryCount == -1) retryCount = SeAppConfig.DefaultRetryElementCount;

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
        /// <summary>
        /// Current web driver associated with the element
        /// </summary>
        public IWebDriver Driver { get; set; }

        /// <summary>
        /// Selenium's IWebElement associated against the control
        /// </summary>
        public IWebElement RawElement => RetryFindElement();

        /// <summary>
        /// Returns the parent control of the current selector from the broswer as type of BaseControl
        /// </summary>
        public IBaseControl ParentControl { get; protected set; }

        /// <summary>
        /// Parent control of the current selector from the browser as type of IWebElement
        /// </summary>
        public IWebElement ParentRawElement => RawElement.FindElement(By.XPath(".."));

        /// <summary>
        /// Element's Id attribute
        /// </summary>
        public string ElementId => RawElement.Id();

        /// <summary>
        /// Selector which is used for selecting the control from the browser
        /// </summary>
        public By By { get; protected set; }

        /// <summary>
        /// Description that was set while initializing the control
        /// </summary>
        public string Description { get; protected set; }

        /// <summary>
        /// Find First availble element in the browser. This value is set during the control initialization
        /// </summary>
        public bool FindFirstAvailable { get; protected set; }

        /// <summary>
        /// Whether element is readonly
        /// </summary>
        public bool IsReadonly => RawElement.IsReadonly();

        /// <summary>
        /// Whether element is disabled
        /// </summary>
        public bool IsDisabled => RawElement.IsDisabled();

        /// <summary>
        /// Whether element is enabled
        /// </summary>
        public bool IsEnabled => RawElement.IsEnabled();

        /// <summary>
        /// Whether element is visible
        /// </summary>
        public bool IsVisible => RawElement.IsVisible();

        /// <summary>
        /// Whether element is Css Displayed (display: none is not applied to the element's style)
        /// </summary>
        public bool CssDisplayed => RawElement.IsCssDisplayed();

        /// <summary>
        /// Whether element is displayed (is same as Visible)
        /// </summary>
        public bool IsDisplayed => IsVisible;

        /// <summary>
        /// Whether element is hidden
        /// </summary>
        public bool IsHidden => !IsVisible;

        /// <summary>
        /// Whether element exists
        /// </summary>
        public bool Exists => RawElement.Exists();

        /// <summary>
        /// Returns the value in the attribute [value]
        /// </summary>
        public string Value => RawElement.Value();

        /// <summary>
        /// Returns either Text or Value of the element which ever is available
        /// </summary>
        public string Text => RawElement.Text();

        /// <summary>
        /// Returns all the class as a single string
        /// </summary>
        public string Class => RawElement.Class();

        /// <summary>
        /// Returns the class applied to the element as a string array
        /// </summary>
        public string[] Classes => RawElement.Classes();

        /// <summary>
        /// Return the attribute value of the element
        /// </summary>
        /// <param name="attributeName">attribute that needs to be read</param>
        /// <returns>value of the attribute</returns>
        public string GetAttribute(string attributeName) => RawElement.GetAttribute(attributeName);
        private string _computedXpath;

        /// <summary>
        /// Returns the XPath selector of the element.
        /// </summary>
        public string XpathSelector
        {
            get
            {
                if (_computedXpath.IsEmpty() && FindFirstAvailable)
                {
                    _computedXpath = ParentControl != null ?
                        ParentControl.RawElement.FindElements(By).FirstOrDefault(d => d.Displayed)?.GetElementXPath(Driver) :
                        Driver.FindElements(By).FirstOrDefault(d => d.Displayed)?.GetElementXPath(Driver);
                }
                else
                {
                    _computedXpath = Driver.FindElements(By).FirstOrDefault(d => d.Displayed)?.GetElementXPath(Driver);
                }
                return _computedXpath;
            }
        }
        #endregion

        #region Control Click Methods
        /// <summary>
        /// Click on the element
        /// </summary>
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

        /// <summary>
        /// Waits for an amount of time and performs the click
        /// </summary>
        /// <param name="timeToWaitInSeconds">Time to wait in seconds before performing the click</param>
        public virtual void WaitAndClick(int timeToWaitInSeconds = 0)
        {
            if (timeToWaitInSeconds != 0)
            {
                WaitForElementInvisible(timeToWaitInSeconds, throwExceptionWhenNotFound: false);
            }
            WaitForElementClickable();
            Click();
        }

        /// <summary>
        /// Scroll to the element in the browser and perform click
        /// </summary>
        public virtual void ScrollAndClick()
        {
            ScrollTo();
            Click();
        }

        /// <summary>
        /// Perform click on the element using Javascript ".click()"
        /// </summary>
        public virtual void ClickByJsScript() => Driver.ExecuteJavaScript("arguments[0].click();", RawElement);

        /// <summary>
        /// Wait for the element to be visible and performs click and then wait until the element goes invisible
        /// </summary>
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

        /// <summary>
        /// Waits for the element to be visible and performs click and ignore any exception if encountered
        /// </summary>
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
        /// <summary>
        /// Sends the text to the element
        /// </summary>
        /// <param name="textToSet">Text that needs to be sent</param>
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

        /// <summary>
        /// Sends keystroke "Enter" to the element
        /// </summary>
        public virtual void SendEnter() => SendKeys(Keys.Enter);

        /// <summary>
        /// Sends keystroke "Tab" to the element
        /// </summary>
        public virtual void SendTab() => SendKeys(Keys.Tab);
        #endregion

        /// <summary>
        /// Scroll to the element in the browser
        /// </summary>
        public virtual void ScrollTo()
        {
            Driver.ExecuteJavaScript("arguments[0].scrollIntoView(true);", RawElement);
        }

        /// <summary>
        /// Clear the element content
        /// </summary>
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

        /// <summary>
        /// Highlights the element in the UI
        /// </summary>
        public virtual void Highlight() => RawElement.Highlight(Driver);

        /// <summary>
        /// Tries to set the focus to the element using Javascript
        /// </summary>
        public virtual void SetFocusByJavascript()
        {
            var findElementCode = $"(document.evaluate('{RawElement.GetElementXPath(Driver)}', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue).focus();";
            Driver.ExecuteJavaScript(findElementCode);
        }

        /// <summary>
        /// ToString format of the element
        /// </summary>
        /// <returns>Type of the element, selector used to find the element, description of the element</returns>
        public override string ToString()
        {
            return $"Type: [{GetType().Name}], Selector: [{By}], Description: [{Description}]";
        }
    }
}