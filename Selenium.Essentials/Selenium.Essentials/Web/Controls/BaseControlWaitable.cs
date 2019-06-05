using OpenQA.Selenium.Support.UI;
using Selenium.Essentials.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Essentials.Web.Controls
{
    public abstract partial class BaseControl
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="waitTimeSec">how long to wait to meet the condition</param>
        /// <param name="throwExceptionWhenNotFound">Should throw exception when encountered</param>
        /// <param name="errorMessage">Detailed error message to be used when condition not met and error bubbled up</param>
        /// <param name="process">Func condition whose value is tested to verify</param>
        /// <param name="reasonForFailedCondition">name the condition so that it can be printed when fails</param>
        /// <param name="whenConditionFailed">When the condition is tried on and fails with exception, what should be returned</param>
        /// <returns></returns>
        protected bool WaitGeneric(int waitTimeSec, bool throwExceptionWhenNotFound, string errorMessage, Func<bool> process, string reasonForFailedCondition, bool whenConditionFailed = false)
        {
            waitTimeSec = waitTimeSec == 0 ? AppConfig.DefaultTimeoutWaitPeriodInSeconds : waitTimeSec;
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(waitTimeSec));
            var conditionSatisfied = false;
            var messageOnFail = errorMessage.HasValue() ? errorMessage : $"Waiting for element failed with reason: {reasonForFailedCondition}";

            try
            {
                conditionSatisfied = wait.Until(d =>
                {
                    try
                    {
                        return process();
                    }
                    catch (Exception)
                    {
                        return whenConditionFailed;
                    }
                });
            }
            catch (Exception ex)
            {
                if (throwExceptionWhenNotFound)
                {
                    throw new WebControlException(Driver, ex, messageOnFail, uiControl: this);
                }
            }

            if (!conditionSatisfied && throwExceptionWhenNotFound)
            {
                throw new ElementUnavailableException(Driver, messageOnFail, this);
            }

            return conditionSatisfied;
        }
        
        #region Enabled

        /// <summary>
        /// Waits for element to appear in the page for specified period of time.
        /// The default timeout period is used.
        /// </summary>
        public bool WaitForElementEnabled(bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementEnabled(AppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

        /// <summary>
        /// Waits for element to appear in the page for specified period of time. 
        /// If element not found and throwExceptionWhenNotFound is true then this method will throw ElementNotVisibleException.
        /// </summary>
        /// <param name="waitTimeSec">Maximum amount of time to wait for</param>
        /// <param name="throwExceptionWhenNotFound">Whether error needs to be thrown out if not found</param>
        /// <param name="errorMessage"></param>
        public bool WaitForElementEnabled(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitGeneric(waitTimeSec, throwExceptionWhenNotFound, errorMessage, () => RawElement.Enabled, "Wait until enabled");

        #endregion

        #region Visible
        /// <summary>
        /// Waits for element to appear in the page for specified period of time.
        /// The default timeout period is used.
        /// </summary>
        public bool WaitForElementVisible(bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementVisible(AppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

        /// <summary>
        /// Waits for element to appear in the page for specified period of time. 
        /// If element not found and throwExceptionWhenNotFound is true then this method will throw ElementNotVisibleException.
        /// </summary>
        /// <param name="waitTimeSec">Maximum amount of time to wait for</param>
        /// <param name="throwExceptionWhenNotFound">Whether error needs to be thrown out if not found</param>
        public bool WaitForElementVisible(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitGeneric(waitTimeSec, throwExceptionWhenNotFound, errorMessage, () => RawElement.Displayed, "Wait until visible");
        #endregion

        #region Exists
        public bool WaitForElementExists(bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementExists(AppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

        public bool WaitForElementExists(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitGeneric(waitTimeSec, throwExceptionWhenNotFound, errorMessage, () => Exists, "Wait until exists");
        #endregion

        #region CssDisplayed
        /// <summary>
        /// Waits for element to get rendered in the page for specified period of time.
        /// The default timeout period is used.
        /// </summary>
        public bool WaitForElementCssDisplayed(bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementCssDisplayed(AppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

        /// <summary>
        /// Waits for element to get rendered (CSS Displayed) in the page for specified period of time. 
        /// If element not found and throwExceptionWhenNotFound is true then this method will throw ElementNotVisibleException.
        /// </summary>
        /// <param name="waitTimeSec">Maximum amount of time to wait for</param>
        /// <param name="throwExceptionWhenNotFound">Whether error needs to be thrown out if not found</param>
        public bool WaitForElementCssDisplayed(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitGeneric(waitTimeSec, throwExceptionWhenNotFound, errorMessage, () => CssDisplayed, "Wait until CssDisplayed");
        #endregion


        /// <summary>
        /// Waits for the element to be removed from the DOM or to be hidden.
        /// </summary>
        public bool WaitForElementInvisible(bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementInvisible(AppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

        /// <summary>
        /// Waits for the element to be removed from the DOM or to be hidden.
        /// </summary>
        /// <param name="waitTimeSec"></param>
        /// <param name="throwExceptionWhenNotFound"></param>
        /// <returns></returns>
        public bool WaitForElementInvisible(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitGeneric(waitTimeSec, throwExceptionWhenNotFound, errorMessage, () => !RawElement.Displayed, "Wait until not visible", whenConditionFailed: true);

        public bool WaitForElementClickable(bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementClickable(AppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);
        public bool WaitForElementClickable(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitGeneric(waitTimeSec, throwExceptionWhenNotFound, errorMessage, () => RawElement != null && RawElement.Displayed && RawElement.IsCssDisplayed() && RawElement.Enabled, "Wait until Clickable");


        public bool WaitForElementTextTrimEquals(string textToMatch, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementTextTrimEquals(textToMatch, AppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);
        public bool WaitForElementTextTrimEquals(string textToMatch, int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitGeneric(waitTimeSec, throwExceptionWhenNotFound, errorMessage, () => RawElement.Text.Trim() == textToMatch, $"Wait till Text (trim) is equals '{textToMatch}'");

        public bool WaitForElementTextStartsWith(string textToMatch, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementTextStartsWith(textToMatch, AppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);
        public bool WaitForElementTextStartsWith(string textToMatch, int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitGeneric(waitTimeSec, throwExceptionWhenNotFound, errorMessage, () => RawElement.Text.Trim().StartsWith(textToMatch), $"Wait till Text starts with '{textToMatch}'");

        public bool WaitForElementTextStartsWith(string[] textsToMatch, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementTextStartsWith(textsToMatch, AppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);
        public bool WaitForElementTextStartsWith(string[] textsToMatch, int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitGeneric(waitTimeSec, throwExceptionWhenNotFound, errorMessage, () => textsToMatch.Any(text => RawElement.Text.Trim().StartsWith(text)), $"Wait till either one of the Text starts with '{string.Join(",", textsToMatch)}'");

        public bool WaitForElementTextContains(string textToMatch, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementTextContains(textToMatch, AppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);
        public bool WaitForElementTextContains(string textToMatch, int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitGeneric(waitTimeSec, throwExceptionWhenNotFound, errorMessage, () => RawElement.Text.Contains(textToMatch), $"Wait till Text contains '{textToMatch}'");

        public bool WaitForElementTextContains(string[] textsToMatch, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementTextContains(textsToMatch, AppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);
        public bool WaitForElementTextContains(string[] textsToMatch, int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitGeneric(waitTimeSec, throwExceptionWhenNotFound, errorMessage, () => textsToMatch.Any(text => RawElement.Text.Contains(text)), $"Wait till either one of the Text contains '{string.Join(",", textsToMatch)}'");

        public bool WaitForElementHasSomeText(bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementHasSomeText(AppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);
        public bool WaitForElementHasSomeText(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitGeneric(waitTimeSec, throwExceptionWhenNotFound, errorMessage, () => RawElement.Text.HasValue(), $"Wait until the element contains some text");

    }
}
