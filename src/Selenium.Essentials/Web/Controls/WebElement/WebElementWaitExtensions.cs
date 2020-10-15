using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Selenium.Essentials
{
    public static partial class WebElementExtensions
    {
        /// <summary>
        /// Wait generic method which will be called by every wait operation.
        /// The method will determine the condition and parameters and returns accordingly
        /// </summary>
        /// <param name="element">element (IWebElement) for which the operation is performed</param>
        /// <param name="driver">driver (IWebDriver) associated with the element</param>
        /// <param name="waitTimeSec">total amount of time to wait (in seconds) to meet the condition</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <param name="process">the process that will be used to satisfy the condition. For example, () => element.Text().HasValue() </param>
        /// <param name="reasonForFailedCondition"></param>
        /// <param name="whenConditionFailed"></param>
        /// <param name="baseControl">the custom control associated with this IWebElement (element). The custom control can be passed as null if it is not associated</param>
        /// <returns>true when the condition is met or else returns false</returns>
        internal static bool WaitGeneric(this IWebElement element,
            IWebDriver driver,
            int waitTimeSec,
            bool throwExceptionWhenNotFound,
            string errorMessage,
            Func<bool> process,
            string reasonForFailedCondition,
            bool whenConditionFailed = false,
            IBaseControl baseControl = null)
        {
            waitTimeSec = waitTimeSec == 0 ? SeAppConfig.DefaultTimeoutWaitPeriodInSeconds : waitTimeSec;
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(waitTimeSec));
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
                    if (baseControl == null)
                    {
                        throw new WebControlException(driver, ex, messageOnFail, uiControl: element);
                    }
                    else
                    {
                        throw new WebControlException(driver, ex, messageOnFail, uiControl: baseControl);
                    }
                }
            }

            if (!conditionSatisfied && throwExceptionWhenNotFound)
            {
                if (baseControl == null)
                {
                    throw new ElementUnavailableException(driver, messageOnFail, element);
                }
                else
                {
                    throw new ElementUnavailableException(driver, messageOnFail, baseControl);
                }
            }

            return conditionSatisfied;
        }

        #region Wait Enabled
        /// <summary>
        /// Wait until the element is enabled 
        /// </summary>
        /// <param name="element">element (IWebElement) for which the operation is performed</param>
        /// <param name="driver">driver (IWebDriver) associated with the element</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public static bool WaitUntilElementEnabled(this IWebElement element,
            IWebDriver driver,
            bool throwExceptionWhenNotFound = true,
            string errorMessage = null)
             => WaitUntilElementEnabled(element,
                    driver,
                    SeAppConfig.DefaultTimeoutWaitPeriodInSeconds,
                    throwExceptionWhenNotFound,
                    errorMessage);

        /// <summary>
        /// Wait until the element is enabled 
        /// </summary>
        /// <param name="element">element (IWebElement) for which the operation is performed</param>
        /// <param name="driver">driver (IWebDriver) associated with the element</param>
        /// <param name="waitTimeSec">total amount of time to wait (in seconds) to meet the condition</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <param name="baseControl">the custom control associated with this IWebElement (element). The custom control can be passed as null if it is not associated</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public static bool WaitUntilElementEnabled(
            this IWebElement element,
            IWebDriver driver,
            int waitTimeSec,
            bool throwExceptionWhenNotFound = true,
            string errorMessage = null,
            IBaseControl baseControl = null)
            => element.WaitGeneric(driver,
                    waitTimeSec,
                    throwExceptionWhenNotFound,
                    errorMessage,
                    () => element?.IsEnabled() ?? false,
                    "Wait until enabled",
                    baseControl: baseControl);

        #endregion

        #region Visible
        /// <summary>
        /// Wait until the element is visible
        /// </summary>
        /// <param name="element">element (IWebElement) for which the operation is performed</param>
        /// <param name="driver">driver (IWebDriver) associated with the element</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public static bool WaitUntilElementVisible(this IWebElement element,
            IWebDriver driver,
            bool throwExceptionWhenNotFound = true,
            string errorMessage = null)
             => WaitUntilElementVisible(element,
                    driver,
                    SeAppConfig.DefaultTimeoutWaitPeriodInSeconds,
                    throwExceptionWhenNotFound,
                    errorMessage);

        /// <summary>
        /// Wait until the element is visible
        /// </summary>
        /// <param name="element">element (IWebElement) for which the operation is performed</param>
        /// <param name="driver">driver (IWebDriver) associated with the element</param>
        /// <param name="waitTimeSec">total amount of time to wait (in seconds) to meet the condition</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <param name="baseControl">the custom control associated with this IWebElement (element). The custom control can be passed as null if it is not associated</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public static bool WaitUntilElementVisible(
            this IWebElement element,
            IWebDriver driver,
            int waitTimeSec,
            bool throwExceptionWhenNotFound = true,
            string errorMessage = null,
            IBaseControl baseControl = null)
            => element.WaitGeneric(driver,
                    waitTimeSec,
                    throwExceptionWhenNotFound,
                    errorMessage,
                    () => element?.Displayed ?? false,
                    "Wait until visible",
                    baseControl: baseControl);

        #endregion

        #region Exsits
        /// <summary>
        /// Wait until the element exists
        /// </summary>
        /// <param name="element">element (IWebElement) for which the operation is performed</param>
        /// <param name="driver">driver (IWebDriver) associated with the element</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public static bool WaitUntilElementExists(this IWebElement element,
            IWebDriver driver,
            bool throwExceptionWhenNotFound = true,
            string errorMessage = null)
             => WaitUntilElementExists(element,
                    driver,
                    SeAppConfig.DefaultTimeoutWaitPeriodInSeconds,
                    throwExceptionWhenNotFound,
                    errorMessage);

        /// <summary>
        /// Wait until the element exists
        /// </summary>
        /// <param name="element">element (IWebElement) for which the operation is performed</param>
        /// <param name="driver">driver (IWebDriver) associated with the element</param>
        /// <param name="waitTimeSec">total amount of time to wait (in seconds) to meet the condition</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <param name="baseControl">the custom control associated with this IWebElement (element). The custom control can be passed as null if it is not associated</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public static bool WaitUntilElementExists(
            this IWebElement element,
            IWebDriver driver,
            int waitTimeSec,
            bool throwExceptionWhenNotFound = true,
            string errorMessage = null,
            IBaseControl baseControl = null)
            => element.WaitGeneric(driver,
                    waitTimeSec,
                    throwExceptionWhenNotFound,
                    errorMessage,
                    () => element?.Exists() ?? false,
                    "Wait until exists",
                    baseControl: baseControl);

        #endregion

        #region CssDisplayed
        /// <summary>
        /// Wait until the element is Css Displayed (display: none not applied)
        /// </summary>
        /// <param name="element">element (IWebElement) for which the operation is performed</param>
        /// <param name="driver">driver (IWebDriver) associated with the element</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public static bool WaitUntilElementCssDisplayed(this IWebElement element,
            IWebDriver driver,
            bool throwExceptionWhenNotFound = true,
            string errorMessage = null)
             => WaitUntilElementCssDisplayed(element,
                    driver,
                    SeAppConfig.DefaultTimeoutWaitPeriodInSeconds,
                    throwExceptionWhenNotFound,
                    errorMessage);

        /// <summary>
        /// Wait until the element is Css Displayed (display: none not applied)
        /// </summary>
        /// <param name="element">element (IWebElement) for which the operation is performed</param>
        /// <param name="driver">driver (IWebDriver) associated with the element</param>
        /// <param name="waitTimeSec">total amount of time to wait (in seconds) to meet the condition</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <param name="baseControl">the custom control associated with this IWebElement (element). The custom control can be passed as null if it is not associated</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public static bool WaitUntilElementCssDisplayed(
            this IWebElement element,
            IWebDriver driver,
            int waitTimeSec,
            bool throwExceptionWhenNotFound = true,
            string errorMessage = null,
            IBaseControl baseControl = null)
            => element.WaitGeneric(driver,
                    waitTimeSec,
                    throwExceptionWhenNotFound,
                    errorMessage,
                    () => element?.IsCssDisplayed() ?? false,
                    "Wait until displayed (not have display-none)",
                    baseControl: baseControl);

        #endregion

        #region Invisible
        /// <summary>
        /// Wait until the element is not visible 
        /// </summary>
        /// <param name="element">element (IWebElement) for which the operation is performed</param>
        /// <param name="driver">driver (IWebDriver) associated with the element</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public static bool WaitUntilElementInvisible(this IWebElement element,
            IWebDriver driver,
            bool throwExceptionWhenNotFound = true,
            string errorMessage = null)
             => WaitUntilElementInvisible(element,
                    driver,
                    SeAppConfig.DefaultTimeoutWaitPeriodInSeconds,
                    throwExceptionWhenNotFound,
                    errorMessage);

        /// <summary>
        /// Wait until the element is not visible 
        /// </summary>
        /// <param name="element">element (IWebElement) for which the operation is performed</param>
        /// <param name="driver">driver (IWebDriver) associated with the element</param>
        /// <param name="waitTimeSec">total amount of time to wait (in seconds) to meet the condition</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <param name="baseControl">the custom control associated with this IWebElement (element). The custom control can be passed as null if it is not associated</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public static bool WaitUntilElementInvisible(
            this IWebElement element,
            IWebDriver driver,
            int waitTimeSec,
            bool throwExceptionWhenNotFound = true,
            string errorMessage = null,
            IBaseControl baseControl = null)
            => element.WaitGeneric(driver,
                    waitTimeSec,
                    throwExceptionWhenNotFound,
                    errorMessage,
                    () => !element?.Displayed ?? true,
                    "Wait until element is invisible",
                    baseControl: baseControl);
        #endregion

        #region Clickable
        /// <summary>
        /// Wait until the element is clickable
        /// </summary>
        /// <param name="element">element (IWebElement) for which the operation is performed</param>
        /// <param name="driver">driver (IWebDriver) associated with the element</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public static bool WaitUntilElementIsClickable(this IWebElement element,
            IWebDriver driver,
            bool throwExceptionWhenNotFound = true,
            string errorMessage = null)
             => WaitUntilElementIsClickable(element,
                    driver,
                    SeAppConfig.DefaultTimeoutWaitPeriodInSeconds,
                    throwExceptionWhenNotFound,
                    errorMessage);

        /// <summary>
        /// Wait until the element is clickable
        /// </summary>
        /// <param name="element">element (IWebElement) for which the operation is performed</param>
        /// <param name="driver">driver (IWebDriver) associated with the element</param>
        /// <param name="waitTimeSec">total amount of time to wait (in seconds) to meet the condition</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <param name="baseControl">the custom control associated with this IWebElement (element). The custom control can be passed as null if it is not associated</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public static bool WaitUntilElementIsClickable(
            this IWebElement element,
            IWebDriver driver,
            int waitTimeSec,
            bool throwExceptionWhenNotFound = true,
            string errorMessage = null,
            IBaseControl baseControl = null)
            => element.WaitGeneric(driver,
                    waitTimeSec,
                    throwExceptionWhenNotFound,
                    errorMessage,
                    () => element != null ? 
                        (element.Exists() && element.Displayed && element.IsCssDisplayed() && element.Enabled) : false,
                    "Wait until element is clickable",
                    baseControl: baseControl);
        #endregion

        #region Text trim equals
        /// <summary>
        /// Wait until the text on the element after trim is equal to the text passed for match
        /// </summary>
        /// <param name="element">element (IWebElement) for which the operation is performed</param>
        /// <param name="driver">driver (IWebDriver) associated with the element</param>
        /// <param name="textToMatch">text that will be used for the match</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public static bool WaitUntilElementTextTrimEquals(this IWebElement element,
            IWebDriver driver,
            string textToMatch,
            bool throwExceptionWhenNotFound = true,
            string errorMessage = null)
             => WaitUntilElementTextTrimEquals(element,
                    driver,
                    textToMatch,
                    SeAppConfig.DefaultTimeoutWaitPeriodInSeconds,
                    throwExceptionWhenNotFound,
                    errorMessage);

        /// <summary>
        /// Wait until the text on the element after trim is equal to the text passed for match
        /// </summary>
        /// <param name="element">element (IWebElement) for which the operation is performed</param>
        /// <param name="driver">driver (IWebDriver) associated with the element</param>
        /// <param name="textToMatch">text that will be used for the match</param>
        /// <param name="waitTimeSec">total amount of time to wait (in seconds) to meet the condition</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <param name="baseControl">the custom control associated with this IWebElement (element). The custom control can be passed as null if it is not associated</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public static bool WaitUntilElementTextTrimEquals(
            this IWebElement element,
            IWebDriver driver,
            string textToMatch,
            int waitTimeSec,
            bool throwExceptionWhenNotFound = true,
            string errorMessage = null,
            IBaseControl baseControl = null)
            => element.WaitGeneric(driver,
                    waitTimeSec,
                    throwExceptionWhenNotFound,
                    errorMessage,
                    () => element != null ? 
                        (element.Exists() && element.Text.Trim().EqualsIgnoreCase(textToMatch)) : false,
                    $"Wait till Text (trim) is equals '{textToMatch}'",
                    baseControl: baseControl);
        #endregion

        #region Text starts with
        /// <summary>
        /// Wait until the text on the element after trim is starts with the text passed for match
        /// </summary>
        /// <param name="element">element (IWebElement) for which the operation is performed</param>
        /// <param name="driver">driver (IWebDriver) associated with the element</param>
        /// <param name="textToMatch">text that will be used for the match</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public static bool WaitUntilElementTextStartsWith(this IWebElement element,
            IWebDriver driver,
            string textToMatch,
            bool throwExceptionWhenNotFound = true,
            string errorMessage = null)
             => WaitUntilElementTextStartsWith(element,
                    driver,
                    textToMatch,
                    SeAppConfig.DefaultTimeoutWaitPeriodInSeconds,
                    throwExceptionWhenNotFound,
                    errorMessage);

        /// <summary>
        /// Wait until the text on the element after trim is starts with the text passed for match
        /// </summary>
        /// <param name="element">element (IWebElement) for which the operation is performed</param>
        /// <param name="driver">driver (IWebDriver) associated with the element</param>
        /// <param name="textToMatch">text that will be used for the match</param>
        /// <param name="waitTimeSec">total amount of time to wait (in seconds) to meet the condition</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <param name="baseControl">the custom control associated with this IWebElement (element). The custom control can be passed as null if it is not associated</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public static bool WaitUntilElementTextStartsWith(
            this IWebElement element,
            IWebDriver driver,
            string textToMatch,
            int waitTimeSec,
            bool throwExceptionWhenNotFound = true,
            string errorMessage = null,
            IBaseControl baseControl = null)
            => element.WaitGeneric(driver,
                    waitTimeSec,
                    throwExceptionWhenNotFound,
                    errorMessage,
                    () => element != null ? (element.Exists() && element.Text.Trim().StartsWith(textToMatch)) : false,
                    $"Wait till Text starts with '{textToMatch}'",
                    baseControl: baseControl);
        #endregion

        #region Text collection starts with
        /// <summary>
        /// Wait until the text on the element after trim, starts with one of the element in the collection of text passed for match
        /// </summary>
        /// <param name="element">element (IWebElement) for which the operation is performed</param>
        /// <param name="driver">driver (IWebDriver) associated with the element</param>
        /// <param name="textsToMatch">text collection that will be used for the match</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public static bool WaitUntilElementTextStartsWith(this IWebElement element,
            IWebDriver driver,
            string[] textsToMatch,
            bool throwExceptionWhenNotFound = true,
            string errorMessage = null)
             => WaitUntilElementTextStartsWith(element,
                    driver,
                    textsToMatch,
                    SeAppConfig.DefaultTimeoutWaitPeriodInSeconds,
                    throwExceptionWhenNotFound,
                    errorMessage);

        /// <summary>
        /// Wait until the text on the element after trim, starts with one of the element in the collection of text passed for match
        /// </summary>
        /// <param name="element">element (IWebElement) for which the operation is performed</param>
        /// <param name="driver">driver (IWebDriver) associated with the element</param>
        /// <param name="textsToMatch">text collection that will be used for the match</param>
        /// <param name="waitTimeSec">total amount of time to wait (in seconds) to meet the condition</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <param name="baseControl">the custom control associated with this IWebElement (element). The custom control can be passed as null if it is not associated</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public static bool WaitUntilElementTextStartsWith(
            this IWebElement element,
            IWebDriver driver,
            string[] textsToMatch,
            int waitTimeSec,
            bool throwExceptionWhenNotFound = true,
            string errorMessage = null,
            IBaseControl baseControl = null)
            => element.WaitGeneric(driver,
                    waitTimeSec,
                    throwExceptionWhenNotFound,
                    errorMessage,
                    () => element != null ? 
                        (element.Exists() && textsToMatch.Any(text => element.Text.Trim().StartsWith(text))) : false,
                    $"Wait till either one of the Text starts with '{string.Join(",", textsToMatch)}'",
                    baseControl: baseControl);
        #endregion

        #region Text contains
        /// <summary>
        /// Wait until the text on the element contains the text passed for match
        /// </summary>
        /// <param name="element">element (IWebElement) for which the operation is performed</param>
        /// <param name="driver">driver (IWebDriver) associated with the element</param>
        /// <param name="textToMatch">text that will be used for the match</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public static bool WaitUntilElementTextContains(this IWebElement element,
            IWebDriver driver,
            string textToMatch,
            bool throwExceptionWhenNotFound = true,
            string errorMessage = null)
             => WaitUntilElementTextContains(element,
                    driver,
                    textToMatch,
                    SeAppConfig.DefaultTimeoutWaitPeriodInSeconds,
                    throwExceptionWhenNotFound,
                    errorMessage);

        /// <summary>
        /// Wait until the text on the element contains the text passed for match
        /// </summary>
        /// <param name="element">element (IWebElement) for which the operation is performed</param>
        /// <param name="driver">driver (IWebDriver) associated with the element</param>
        /// <param name="textToMatch">text that will be used for the match</param>
        /// <param name="waitTimeSec">total amount of time to wait (in seconds) to meet the condition</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <param name="baseControl">the custom control associated with this IWebElement (element). The custom control can be passed as null if it is not associated</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public static bool WaitUntilElementTextContains(
            this IWebElement element,
            IWebDriver driver,
            string textToMatch,
            int waitTimeSec,
            bool throwExceptionWhenNotFound = true,
            string errorMessage = null,
            IBaseControl baseControl = null)
            => element.WaitGeneric(driver,
                    waitTimeSec,
                    throwExceptionWhenNotFound,
                    errorMessage,
                    () => element != null ? (element.Exists() && element.Text.Contains(textToMatch)) : false,
                    $"Wait till Text contains '{textToMatch}'",
                    baseControl: baseControl);
        #endregion

        #region Text collection contains
        /// <summary>
        /// Wait until the text on the element contains one of the element in the collection of text passed for match
        /// </summary>
        /// <param name="element">element (IWebElement) for which the operation is performed</param>
        /// <param name="driver">driver (IWebDriver) associated with the element</param>
        /// <param name="textsToMatch">text collection that will be used for the match</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public static bool WaitUntilElementTextContains(this IWebElement element,
            IWebDriver driver,
            string[] textsToMatch,
            bool throwExceptionWhenNotFound = true,
            string errorMessage = null)
             => WaitUntilElementTextContains(element,
                    driver,
                    textsToMatch,
                    SeAppConfig.DefaultTimeoutWaitPeriodInSeconds,
                    throwExceptionWhenNotFound,
                    errorMessage);

        /// <summary>
        /// Wait until the text on the element contains one of the element in the collection of text passed for match
        /// </summary>
        /// <param name="element">element (IWebElement) for which the operation is performed</param>
        /// <param name="driver">driver (IWebDriver) associated with the element</param>
        /// <param name="textsToMatch">text collection that will be used for the match</param>
        /// <param name="waitTimeSec">total amount of time to wait (in seconds) to meet the condition</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <param name="baseControl">the custom control associated with this IWebElement (element). The custom control can be passed as null if it is not associated</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public static bool WaitUntilElementTextContains(
            this IWebElement element,
            IWebDriver driver,
            string[] textsToMatch,
            int waitTimeSec,
            bool throwExceptionWhenNotFound = true,
            string errorMessage = null,
            IBaseControl baseControl = null)
            => element.WaitGeneric(driver,
                    waitTimeSec,
                    throwExceptionWhenNotFound,
                    errorMessage,
                    () => element != null ?
                        (element.Exists() && textsToMatch.Any(text => element.Text.Contains(text))) : false,
                    $"Wait till either one of the Text contains '{string.Join(",", textsToMatch)}'",
                    baseControl: baseControl);
        #endregion

        #region Element has some text
        /// <summary>
        /// Wait until the element has any text on it
        /// </summary>
        /// <param name="element">element (IWebElement) for which the operation is performed</param>
        /// <param name="driver">driver (IWebDriver) associated with the element</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public static bool WaitUntilElementHasSomeText(this IWebElement element,
            IWebDriver driver,
            bool throwExceptionWhenNotFound = true,
            string errorMessage = null)
             => WaitUntilElementHasSomeText(element,
                    driver,
                    SeAppConfig.DefaultTimeoutWaitPeriodInSeconds,
                    throwExceptionWhenNotFound,
                    errorMessage);

        /// <summary>
        /// Wait until the element has any text on it
        /// </summary>
        /// <param name="element">element (IWebElement) for which the operation is performed</param>
        /// <param name="driver">driver (IWebDriver) associated with the element</param>
        /// <param name="waitTimeSec">total amount of time to wait (in seconds) to meet the condition</param>
        /// <param name="throwExceptionWhenNotFound">throw an exception when the condition is not met</param>
        /// <param name="errorMessage">message for the exception when condition is not met</param>
        /// <param name="baseControl">the custom control associated with this IWebElement (element). The custom control can be passed as null if it is not associated</param>
        /// <returns>true when the condition is met or else returns false</returns>
        public static bool WaitUntilElementHasSomeText(
            this IWebElement element,
            IWebDriver driver,
            int waitTimeSec,
            bool throwExceptionWhenNotFound = true,
            string errorMessage = null,
            IBaseControl baseControl = null)
            => element.WaitGeneric(driver,
                    waitTimeSec,
                    throwExceptionWhenNotFound,
                    errorMessage,
                    () => element != null ? (element.Exists() && element.Text.HasValue()) : false,
                    $"Wait until the element contains any text value",
                    baseControl: baseControl);
        #endregion
    }
}
