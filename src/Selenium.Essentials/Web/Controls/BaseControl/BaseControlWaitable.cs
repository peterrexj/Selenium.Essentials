using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Essentials
{
    public abstract partial class BaseControl
    {
        #region Enabled

        public bool WaitForElementEnabled(bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementEnabled(SeAppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

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
            => WaitForElementVisible(SeAppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

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
            => WaitForElementExists(SeAppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

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
            => WaitForElementCssDisplayed(SeAppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

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
            => WaitForElementInvisible(SeAppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

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
            => WaitForElementClickable(SeAppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);
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
            => WaitForElementTextTrimEquals(textToMatch, SeAppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

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
            => WaitForElementTextStartsWith(textToMatch, SeAppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);
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
            => WaitForElementTextStartsWith(textsToMatch, SeAppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);
        public bool WaitForElementTextStartsWith(string[] textsToMatch, int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WebElementExtensions.WaitForElementTextStartsWith(RawElement,
                Driver,
                textsToMatch: textsToMatch,
                waitTimeSec: waitTimeSec,
                throwExceptionWhenNotFound: throwExceptionWhenNotFound,
                errorMessage: errorMessage,
                baseControl: this);
        #endregion

        #region Text contains
        public bool WaitForElementTextContains(string textToMatch, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementTextContains(textToMatch, SeAppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);
        public bool WaitForElementTextContains(string textToMatch, int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WebElementExtensions.WaitForElementTextContains(RawElement,
                Driver,
                textToMatch: textToMatch,
                waitTimeSec: waitTimeSec,
                errorMessage: errorMessage,
                baseControl: this);
        #endregion

        #region Text[] contains
        public bool WaitForElementTextContains(string[] textsToMatch, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementTextContains(textsToMatch, SeAppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);
        public bool WaitForElementTextContains(string[] textsToMatch, int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WebElementExtensions.WaitForElementTextContains(RawElement,
                Driver,
                textsToMatch: textsToMatch,
                waitTimeSec: waitTimeSec,
                errorMessage: errorMessage,
                baseControl: this);
        #endregion

        #region Has text
        public bool WaitForElementHasSomeText(bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementHasSomeText(SeAppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);
        public bool WaitForElementHasSomeText(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WebElementExtensions.WaitForElementHasSomeText(RawElement,
                Driver,
                waitTimeSec: waitTimeSec,
                errorMessage: errorMessage,
                baseControl: this);
        #endregion
    }
}
