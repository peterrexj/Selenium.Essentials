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
        /// <summary>
        /// Wait until the element is enabled 
        /// </summary>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public bool WaitForElementEnabled(bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementEnabled(SeAppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

        /// <summary>
        /// Wait until the element is enabled 
        /// </summary>
        /// <param name="waitTimeSec">total amount of time to wait (in seconds) to meet the condition</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public bool WaitForElementEnabled(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WebElementExtensions.WaitForElementEnabled(RawElement, 
                Driver, 
                waitTimeSec: waitTimeSec, 
                throwExceptionWhenNotFound: throwExceptionWhenNotFound, 
                errorMessage: errorMessage, 
                baseControl: this);

        #endregion

        #region Visible
        /// <summary>
        /// Wait until the element is visible
        /// </summary>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public bool WaitForElementVisible(bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementVisible(SeAppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

        /// <summary>
        /// Wait until the element is visible
        /// </summary>
        /// <param name="waitTimeSec">total amount of time to wait (in seconds) to meet the condition</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public bool WaitForElementVisible(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WebElementExtensions.WaitForElementVisible(RawElement,
                Driver,
                waitTimeSec: waitTimeSec,
                throwExceptionWhenNotFound: throwExceptionWhenNotFound,
                errorMessage: errorMessage,
                baseControl: this);
        #endregion

        #region Exists
        /// <summary>
        /// Wait until the element exists
        /// </summary>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public bool WaitForElementExists(bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementExists(SeAppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

        /// <summary>
        /// Wait until the element exists
        /// </summary>
        /// <param name="waitTimeSec">total amount of time to wait (in seconds) to meet the condition</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public bool WaitForElementExists(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WebElementExtensions.WaitForElementExists(RawElement,
                Driver,
                waitTimeSec: waitTimeSec,
                throwExceptionWhenNotFound: throwExceptionWhenNotFound,
                errorMessage: errorMessage,
                baseControl: this);
        #endregion

        #region CssDisplayed
        /// <summary>
        /// Wait until the element is Css Displayed (display: none not applied)
        /// </summary>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public bool WaitForElementCssDisplayed(bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementCssDisplayed(SeAppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

        /// <summary>
        /// Wait until the element is Css Displayed (display: none not applied)
        /// </summary>
        /// <param name="waitTimeSec">total amount of time to wait (in seconds) to meet the condition</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public bool WaitForElementCssDisplayed(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WebElementExtensions.WaitForElementCssDisplayed(RawElement,
                Driver,
                waitTimeSec: waitTimeSec,
                throwExceptionWhenNotFound: throwExceptionWhenNotFound,
                errorMessage: errorMessage,
                baseControl: this);
        #endregion

        #region Invisible
        /// <summary>
        /// Wait until the element is not visible 
        /// </summary>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public bool WaitForElementInvisible(bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementInvisible(SeAppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

        /// <summary>
        /// Wait until the element is not visible
        /// </summary>
        /// <param name="waitTimeSec">total amount of time to wait (in seconds) to meet the condition</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public bool WaitForElementInvisible(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WebElementExtensions.WaitForElementInvisible(RawElement,
                Driver,
                waitTimeSec: waitTimeSec,
                throwExceptionWhenNotFound: throwExceptionWhenNotFound,
                errorMessage: errorMessage,
                baseControl: this);
        #endregion

        #region Is Clickable
        /// <summary>
        /// Wait until the element is clickable
        /// </summary>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public bool WaitForElementClickable(bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementClickable(SeAppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

        /// <summary>
        /// Wait until the element is clickable
        /// </summary>
        /// <param name="waitTimeSec">total amount of time to wait (in seconds) to meet the condition</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public bool WaitForElementClickable(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WebElementExtensions.WaitForElementIsClickable(RawElement,
                Driver,
                waitTimeSec: waitTimeSec,
                throwExceptionWhenNotFound: throwExceptionWhenNotFound,
                errorMessage: errorMessage,
                baseControl: this);
        #endregion

        #region Trimmed Text Equals
        /// <summary>
        /// Wait until the text on the element after trim is equal to the text passed for match
        /// </summary>
        /// <param name="textToMatch">text that will be used for the match</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public bool WaitForElementTextTrimEquals(string textToMatch, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementTextTrimEquals(textToMatch, SeAppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

        /// <summary>
        /// Wait until the text on the element after trim is equal to the text passed for match
        /// </summary>
        /// <param name="textToMatch">text that will be used for the match</param>
        /// <param name="waitTimeSec">total amount of time to wait (in seconds) to meet the condition</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
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
        /// <summary>
        /// Wait until the text on the element after trim is starts with the text passed for match
        /// </summary>
        /// <param name="textToMatch">text that will be used for the match</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public bool WaitForElementTextStartsWith(string textToMatch, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementTextStartsWith(textToMatch, SeAppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

        /// <summary>
        /// Wait until the text on the element after trim is starts with the text passed for match
        /// </summary>
        /// <param name="textToMatch">text that will be used for the match</param>
        /// <param name="waitTimeSec">total amount of time to wait (in seconds) to meet the condition</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
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
        /// <summary>
        /// Wait until the text on the element after trim, starts with one of the element in the collection of text passed for match
        /// </summary>
        /// <param name="textsToMatch">text that will be used for the match</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public bool WaitForElementTextStartsWith(string[] textsToMatch, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementTextStartsWith(textsToMatch, SeAppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

        /// <summary>
        /// Wait until the text on the element after trim, starts with one of the element in the collection of text passed for match
        /// </summary>
        /// <param name="textsToMatch">text that will be used for the match</param>
        /// <param name="waitTimeSec">total amount of time to wait (in seconds) to meet the condition</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
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
        /// <summary>
        /// Wait until the text on the element contains the text passed for match
        /// </summary>
        /// <param name="textToMatch"></param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public bool WaitForElementTextContains(string textToMatch, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementTextContains(textToMatch, SeAppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

        /// <summary>
        /// Wait until the text on the element contains the text passed for match
        /// </summary>
        /// <param name="textToMatch"></param>
        /// <param name="waitTimeSec">total amount of time to wait (in seconds) to meet the condition</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public bool WaitForElementTextContains(string textToMatch, int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WebElementExtensions.WaitForElementTextContains(RawElement,
                Driver,
                textToMatch: textToMatch,
                waitTimeSec: waitTimeSec,
                errorMessage: errorMessage,
                baseControl: this);
        #endregion

        #region Text[] contains
        /// <summary>
        /// Wait until the text on the element contains one of the element in the collection of text passed for match
        /// </summary>
        /// <param name="textsToMatch">text that will be used for the match</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public bool WaitForElementTextContains(string[] textsToMatch, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementTextContains(textsToMatch, SeAppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

        /// <summary>
        /// Wait until the text on the element contains one of the element in the collection of text passed for match
        /// </summary>
        /// <param name="textsToMatch">text that will be used for the match</param>
        /// <param name="waitTimeSec">total amount of time to wait (in seconds) to meet the condition</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public bool WaitForElementTextContains(string[] textsToMatch, int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WebElementExtensions.WaitForElementTextContains(RawElement,
                Driver,
                textsToMatch: textsToMatch,
                waitTimeSec: waitTimeSec,
                errorMessage: errorMessage,
                baseControl: this);
        #endregion

        #region Has text
        /// <summary>
        /// Wait until the element has any text on it
        /// </summary>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
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
