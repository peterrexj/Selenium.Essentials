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
            waitTimeSec = waitTimeSec == 0 ? AppConfig.DefaultTimeoutWaitPeriodInSeconds : waitTimeSec;
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
        public static bool WaitForElementEnabled(this IWebElement element,
            IWebDriver driver,
            bool throwExceptionWhenNotFound = true,
            string errorMessage = null)
             => WaitForElementEnabled(element,
                    driver,
                    AppConfig.DefaultTimeoutWaitPeriodInSeconds,
                    throwExceptionWhenNotFound,
                    errorMessage);

        public static bool WaitForElementEnabled(
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
                    () => element.IsEnabled(),
                    "Wait until enabled",
                    baseControl: baseControl);

        #endregion

        #region Visible
        public static bool WaitForElementVisible(this IWebElement element,
            IWebDriver driver,
            bool throwExceptionWhenNotFound = true,
            string errorMessage = null)
             => WaitForElementVisible(element,
                    driver,
                    AppConfig.DefaultTimeoutWaitPeriodInSeconds,
                    throwExceptionWhenNotFound,
                    errorMessage);

        public static bool WaitForElementVisible(
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
                    () => element.Displayed,
                    "Wait until visible",
                    baseControl: baseControl);

        #endregion

        #region Exsits
        public static bool WaitForElementExists(this IWebElement element,
            IWebDriver driver,
            bool throwExceptionWhenNotFound = true,
            string errorMessage = null)
             => WaitForElementExists(element,
                    driver,
                    AppConfig.DefaultTimeoutWaitPeriodInSeconds,
                    throwExceptionWhenNotFound,
                    errorMessage);

        public static bool WaitForElementExists(
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
                    () => element.Exists(),
                    "Wait until exists",
                    baseControl: baseControl);

        #endregion

        #region CssDisplayed
        public static bool WaitForElementCssDisplayed(this IWebElement element,
            IWebDriver driver,
            bool throwExceptionWhenNotFound = true,
            string errorMessage = null)
             => WaitForElementCssDisplayed(element,
                    driver,
                    AppConfig.DefaultTimeoutWaitPeriodInSeconds,
                    throwExceptionWhenNotFound,
                    errorMessage);

        public static bool WaitForElementCssDisplayed(
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
                    () => element.IsCssDisplayed(),
                    "Wait until displayed (not have display-none)",
                    baseControl: baseControl);

        #endregion

        #region Invisible
        public static bool WaitForElementInvisible(this IWebElement element,
            IWebDriver driver,
            bool throwExceptionWhenNotFound = true,
            string errorMessage = null)
             => WaitForElementInvisible(element,
                    driver,
                    AppConfig.DefaultTimeoutWaitPeriodInSeconds,
                    throwExceptionWhenNotFound,
                    errorMessage);

        public static bool WaitForElementInvisible(
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
                    () => !element.Displayed,
                    "Wait until element is invisible",
                    baseControl: baseControl);
        #endregion

        #region Clickable
        public static bool WaitForElementIsClickable(this IWebElement element,
            IWebDriver driver,
            bool throwExceptionWhenNotFound = true,
            string errorMessage = null)
             => WaitForElementIsClickable(element,
                    driver,
                    AppConfig.DefaultTimeoutWaitPeriodInSeconds,
                    throwExceptionWhenNotFound,
                    errorMessage);

        public static bool WaitForElementIsClickable(
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
                    () => element.Exists() && element.Displayed && element.IsCssDisplayed() && element.Enabled,
                    "Wait until element is clickable",
                    baseControl: baseControl);
        #endregion

        #region Text trim equals
        public static bool WaitForElementTextTrimEquals(this IWebElement element,
            IWebDriver driver,
            string textToMatch,
            bool throwExceptionWhenNotFound = true,
            string errorMessage = null)
             => WaitForElementTextTrimEquals(element,
                    driver,
                    textToMatch,
                    AppConfig.DefaultTimeoutWaitPeriodInSeconds,
                    throwExceptionWhenNotFound,
                    errorMessage);

        public static bool WaitForElementTextTrimEquals(
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
                    () => element.Exists() && element.Text.Trim().EqualsIgnoreCase(textToMatch),
                    $"Wait till Text (trim) is equals '{textToMatch}'",
                    baseControl: baseControl);
        #endregion

        #region Text starts with
        public static bool WaitForElementTextStartsWith(this IWebElement element,
            IWebDriver driver,
            string textToMatch,
            bool throwExceptionWhenNotFound = true,
            string errorMessage = null)
             => WaitForElementTextStartsWith(element,
                    driver,
                    textToMatch,
                    AppConfig.DefaultTimeoutWaitPeriodInSeconds,
                    throwExceptionWhenNotFound,
                    errorMessage);

        public static bool WaitForElementTextStartsWith(
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
                    () => element.Exists() && element.Text.Trim().StartsWith(textToMatch),
                    $"Wait till Text starts with '{textToMatch}'",
                    baseControl: baseControl);
        #endregion

        #region Text collection starts with
        public static bool WaitForElementTextStartsWith(this IWebElement element,
            IWebDriver driver,
            string[] textsToMatch,
            bool throwExceptionWhenNotFound = true,
            string errorMessage = null)
             => WaitForElementTextStartsWith(element,
                    driver,
                    textsToMatch,
                    AppConfig.DefaultTimeoutWaitPeriodInSeconds,
                    throwExceptionWhenNotFound,
                    errorMessage);

        public static bool WaitForElementTextStartsWith(
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
                    () => element.Exists() && textsToMatch.Any(text => element.Text.Trim().StartsWith(text)),
                    $"Wait till either one of the Text starts with '{string.Join(",", textsToMatch)}'",
                    baseControl: baseControl);
        #endregion

        #region Text contains
        public static bool WaitForElementTextContains(this IWebElement element,
            IWebDriver driver,
            string textToMatch,
            bool throwExceptionWhenNotFound = true,
            string errorMessage = null)
             => WaitForElementTextContains(element,
                    driver,
                    textToMatch,
                    AppConfig.DefaultTimeoutWaitPeriodInSeconds,
                    throwExceptionWhenNotFound,
                    errorMessage);

        public static bool WaitForElementTextContains(
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
                    () => element.Exists() && element.Text.Contains(textToMatch),
                    $"Wait till Text contains '{textToMatch}'",
                    baseControl: baseControl);
        #endregion

        #region Text collection contains
        public static bool WaitForElementTextContains(this IWebElement element,
            IWebDriver driver,
            string[] textsToMatch,
            bool throwExceptionWhenNotFound = true,
            string errorMessage = null)
             => WaitForElementTextContains(element,
                    driver,
                    textsToMatch,
                    AppConfig.DefaultTimeoutWaitPeriodInSeconds,
                    throwExceptionWhenNotFound,
                    errorMessage);

        public static bool WaitForElementTextContains(
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
                    () => element.Exists() && textsToMatch.Any(text => element.Text.Contains(text)),
                    $"Wait till either one of the Text contains '{string.Join(",", textsToMatch)}'",
                    baseControl: baseControl);
        #endregion

        #region Text collection contains
        public static bool WaitForElementHasSomeText(this IWebElement element,
            IWebDriver driver,
            bool throwExceptionWhenNotFound = true,
            string errorMessage = null)
             => WaitForElementHasSomeText(element,
                    driver,
                    AppConfig.DefaultTimeoutWaitPeriodInSeconds,
                    throwExceptionWhenNotFound,
                    errorMessage);

        public static bool WaitForElementHasSomeText(
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
                    () => element.Exists() && element.Text.HasValue(),
                    $"Wait until the element contains any text value",
                    baseControl: baseControl);
        #endregion
    }
}
