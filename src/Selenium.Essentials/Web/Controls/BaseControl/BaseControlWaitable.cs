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
        public bool WaitUntilElementEnabled(bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitUntilElementEnabled(SeAppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

        /// <summary>
        /// Wait until the element is enabled 
        /// </summary>
        /// <param name="waitTimeSec">total amount of time to wait (in seconds) to meet the condition</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public bool WaitUntilElementEnabled(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WebElementExtensions.WaitUntilElementEnabled(RawElement, 
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
        public bool WaitUntilElementVisible(bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitUntilElementVisible(SeAppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

        /// <summary>
        /// Wait until the element is visible
        /// </summary>
        /// <param name="waitTimeSec">total amount of time to wait (in seconds) to meet the condition</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public bool WaitUntilElementVisible(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WebElementExtensions.WaitUntilElementVisible(RawElement,
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
        public bool WaitUntilElementExists(bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitUntilElementExists(SeAppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

        /// <summary>
        /// Wait until the element exists
        /// </summary>
        /// <param name="waitTimeSec">total amount of time to wait (in seconds) to meet the condition</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public bool WaitUntilElementExists(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WebElementExtensions.WaitUntilElementExists(RawElement,
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
        public bool WaitUntilElementCssDisplayed(bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitUntilElementCssDisplayed(SeAppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

        /// <summary>
        /// Wait until the element is Css Displayed (display: none not applied)
        /// </summary>
        /// <param name="waitTimeSec">total amount of time to wait (in seconds) to meet the condition</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public bool WaitUntilElementCssDisplayed(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WebElementExtensions.WaitUntilElementCssDisplayed(RawElement,
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
        public bool WaitUntilElementInvisible(bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitUntilElementInvisible(SeAppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

        /// <summary>
        /// Wait until the element is not visible
        /// </summary>
        /// <param name="waitTimeSec">total amount of time to wait (in seconds) to meet the condition</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public bool WaitUntilElementInvisible(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WebElementExtensions.WaitUntilElementInvisible(RawElement,
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
        public bool WaitUntilElementClickable(bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitUntilElementClickable(SeAppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

        /// <summary>
        /// Wait until the element is clickable
        /// </summary>
        /// <param name="waitTimeSec">total amount of time to wait (in seconds) to meet the condition</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public bool WaitUntilElementClickable(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WebElementExtensions.WaitUntilElementIsClickable(RawElement,
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
        public bool WaitUntilElementTextTrimEquals(string textToMatch, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitUntilElementTextTrimEquals(textToMatch, SeAppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

        /// <summary>
        /// Wait until the text on the element after trim is equal to the text passed for match
        /// </summary>
        /// <param name="textToMatch">text that will be used for the match</param>
        /// <param name="waitTimeSec">total amount of time to wait (in seconds) to meet the condition</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public bool WaitUntilElementTextTrimEquals(string textToMatch, int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WebElementExtensions.WaitUntilElementTextTrimEquals(RawElement,
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
        public bool WaitUntilElementTextStartsWith(string textToMatch, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitUntilElementTextStartsWith(textToMatch, SeAppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

        /// <summary>
        /// Wait until the text on the element after trim is starts with the text passed for match
        /// </summary>
        /// <param name="textToMatch">text that will be used for the match</param>
        /// <param name="waitTimeSec">total amount of time to wait (in seconds) to meet the condition</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public bool WaitUntilElementTextStartsWith(string textToMatch, int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WebElementExtensions.WaitUntilElementTextStartsWith(RawElement,
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
        /// <param name="textsToMatch">text collection that will be used for the match</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public bool WaitUntilElementTextStartsWith(string[] textsToMatch, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitUntilElementTextStartsWith(textsToMatch, SeAppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

        /// <summary>
        /// Wait until the text on the element after trim, starts with one of the element in the collection of text passed for match
        /// </summary>
        /// <param name="textsToMatch">text colletion that will be used for the match</param>
        /// <param name="waitTimeSec">total amount of time to wait (in seconds) to meet the condition</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public bool WaitUntilElementTextStartsWith(string[] textsToMatch, int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WebElementExtensions.WaitUntilElementTextStartsWith(RawElement,
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
        /// <param name="textToMatch">text that will be used for the match</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public bool WaitUntilElementTextContains(string textToMatch, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitUntilElementTextContains(textToMatch, SeAppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

        /// <summary>
        /// Wait until the text on the element contains the text passed for match
        /// </summary>
        /// <param name="textToMatch">text that will be used for the match</param>
        /// <param name="waitTimeSec">total amount of time to wait (in seconds) to meet the condition</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public bool WaitUntilElementTextContains(string textToMatch, int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WebElementExtensions.WaitUntilElementTextContains(RawElement,
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
        /// <param name="textsToMatch">text collection that will be used for the match</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public bool WaitUntilElementTextContains(string[] textsToMatch, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitUntilElementTextContains(textsToMatch, SeAppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

        /// <summary>
        /// Wait until the text on the element contains one of the element in the collection of text passed for match
        /// </summary>
        /// <param name="textsToMatch">text collection that will be used for the match</param>
        /// <param name="waitTimeSec">total amount of time to wait (in seconds) to meet the condition</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public bool WaitUntilElementTextContains(string[] textsToMatch, int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WebElementExtensions.WaitUntilElementTextContains(RawElement,
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
        public bool WaitUntilElementHasSomeText(bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitUntilElementHasSomeText(SeAppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

        /// <summary>
        /// Wait until the element has any text on it
        /// </summary>
        /// <param name="waitTimeSec">total amount of time to wait (in seconds) to meet the condition</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public bool WaitUntilElementHasSomeText(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WebElementExtensions.WaitUntilElementHasSomeText(RawElement,
                Driver,
                waitTimeSec: waitTimeSec,
                errorMessage: errorMessage,
                baseControl: this);
        #endregion
    }
}
