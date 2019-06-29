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

        public bool WaitForElementEnabled(bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementEnabled(AppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

        public bool WaitForElementEnabled(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WebElementExtensions.WaitForElementEnabled(RawElement, 
                Driver, 
                waitTimeSec: waitTimeSec, 
                throwExceptionWhenNotFound: throwExceptionWhenNotFound, 
                errorMessage: errorMessage, 
                baseControl: this);

        #endregion

        #region Visible
        public bool WaitForElementVisible(bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementVisible(AppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

        public bool WaitForElementVisible(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WebElementExtensions.WaitForElementVisible(RawElement,
                Driver,
                waitTimeSec: waitTimeSec,
                throwExceptionWhenNotFound: throwExceptionWhenNotFound,
                errorMessage: errorMessage,
                baseControl: this);
        #endregion

        #region Exists
        public bool WaitForElementExists(bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementExists(AppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

        public bool WaitForElementExists(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WebElementExtensions.WaitForElementExists(RawElement,
                Driver,
                waitTimeSec: waitTimeSec,
                throwExceptionWhenNotFound: throwExceptionWhenNotFound,
                errorMessage: errorMessage,
                baseControl: this);
        #endregion

        #region CssDisplayed
        public bool WaitForElementCssDisplayed(bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementCssDisplayed(AppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

        public bool WaitForElementCssDisplayed(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WebElementExtensions.WaitForElementCssDisplayed(RawElement,
                Driver,
                waitTimeSec: waitTimeSec,
                throwExceptionWhenNotFound: throwExceptionWhenNotFound,
                errorMessage: errorMessage,
                baseControl: this);
        #endregion

        #region Invisible
        public bool WaitForElementInvisible(bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementInvisible(AppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

        public bool WaitForElementInvisible(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WebElementExtensions.WaitForElementInvisible(RawElement,
                Driver,
                waitTimeSec: waitTimeSec,
                throwExceptionWhenNotFound: throwExceptionWhenNotFound,
                errorMessage: errorMessage,
                baseControl: this);
        #endregion

        #region Is Clickable
        public bool WaitForElementClickable(bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementClickable(AppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);
        public bool WaitForElementClickable(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WebElementExtensions.WaitForElementIsClickable(RawElement,
                Driver,
                waitTimeSec: waitTimeSec,
                throwExceptionWhenNotFound: throwExceptionWhenNotFound,
                errorMessage: errorMessage,
                baseControl: this);
        #endregion

        #region Trimmed Text Equals
        public bool WaitForElementTextTrimEquals(string textToMatch, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementTextTrimEquals(textToMatch, AppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

        public bool WaitForElementTextTrimEquals(string textToMatch, int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WebElementExtensions.WaitForElementTextTrimEquals(RawElement,
                Driver,
                textToMatch: textToMatch,
                waitTimeSec: waitTimeSec,
                throwExceptionWhenNotFound: throwExceptionWhenNotFound,
                errorMessage: errorMessage,
                baseControl: this);
        #endregion

        #region Text tarts with
        public bool WaitForElementTextStartsWith(string textToMatch, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementTextStartsWith(textToMatch, AppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);
        public bool WaitForElementTextStartsWith(string textToMatch, int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WebElementExtensions.WaitForElementTextStartsWith(RawElement,
                Driver,
                textToMatch: textToMatch,
                waitTimeSec: waitTimeSec,
                throwExceptionWhenNotFound: throwExceptionWhenNotFound,
                errorMessage: errorMessage,
                baseControl: this);
        #endregion

        #region Text[] starts with
        public bool WaitForElementTextStartsWith(string[] textsToMatch, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementTextStartsWith(textsToMatch, AppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);
        public bool WaitForElementTextStartsWith(string[] textsToMatch, int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WebElementExtensions.WaitForElementTextStartsWith(RawElement,
                Driver,
                textsToMatch: textsToMatch,
                waitTimeSec: waitTimeSec,
                throwExceptionWhenNotFound: throwExceptionWhenNotFound,
                errorMessage: errorMessage,
                baseControl: this);
        #endregion


        public bool WaitForElementTextContains(string textToMatch, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementTextContains(textToMatch, AppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);
        public bool WaitForElementTextContains(string textToMatch, int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WebElementExtensions.WaitForElementTextContains(RawElement,
                Driver,
                textToMatch: textToMatch,
                waitTimeSec: waitTimeSec,
                errorMessage: errorMessage,
                baseControl: this);


        public bool WaitForElementTextContains(string[] textsToMatch, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementTextContains(textsToMatch, AppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);
        public bool WaitForElementTextContains(string[] textsToMatch, int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WebElementExtensions.WaitForElementTextContains(RawElement,
                Driver,
                textsToMatch: textsToMatch,
                waitTimeSec: waitTimeSec,
                errorMessage: errorMessage,
                baseControl: this);


        public bool WaitForElementHasSomeText(bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementHasSomeText(AppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);
        public bool WaitForElementHasSomeText(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WebElementExtensions.WaitForElementHasSomeText(RawElement,
                Driver,
                waitTimeSec: waitTimeSec,
                errorMessage: errorMessage,
                baseControl: this);
    }
}
