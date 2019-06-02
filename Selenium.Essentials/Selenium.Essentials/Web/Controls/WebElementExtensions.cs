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
    public static class WebElementExtensions
    {
        private const string __HightElementStyle = "background: yellow; border: 2px solid red;";
        private static bool WaitGeneric(this IWebElement element, IWebDriver driver, int waitTimeSec, bool throwExceptionWhenNotFound, string errorMessage, Func<bool> process, string reasonForFailedCondition, bool whenConditionFailed = false)
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
                    throw new WebControlException(driver, ex, messageOnFail, uiControl: element);
                }
            }

            if (!conditionSatisfied && throwExceptionWhenNotFound)
            {
                throw new ElementUnavailableException(driver, messageOnFail, element);
            }

            return conditionSatisfied;
        }
        

        public static bool WaitForElementEnabled(this IWebElement element, IWebDriver driver, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementEnabled(element, driver, AppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

        public static bool WaitForElementEnabled(this IWebElement element, IWebDriver driver, int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => element.WaitGeneric(driver, waitTimeSec, throwExceptionWhenNotFound, errorMessage, () => element.IsEnabled(), "Wait until enabled");

        public static bool WaitForElementVisible(this IWebElement element, IWebDriver driver, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => WaitForElementVisible(element, driver, AppConfig.DefaultTimeoutWaitPeriodInSeconds, throwExceptionWhenNotFound, errorMessage);

        public static bool WaitForElementVisible(this IWebElement element, IWebDriver driver, int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null)
            => element.WaitGeneric(driver, waitTimeSec, throwExceptionWhenNotFound, errorMessage, () => element.IsVisible(), "Wait until visible");

        /// <summary>
        /// Extracts the Xpath of the web element from the DOM
        /// Thread safe
        /// </summary>
        /// <param name="e"></param>
        /// <param name="driver"></param>
        /// <param name="excludeIdCheck">This will exclude to generate XPath based on the Id. There are some places in the application where the ID of the element is duplicated</param>
        /// <returns></returns>
        public static string GetElementXPath(this IWebElement e, IWebDriver driver, bool excludeIdCheck = false)
        {
            var scriptWithId = "if(c.id!==''){return'//*[@id=\"'+c.id+'\"]'}";
            var scriptToGetXpath = "gPt=function(c){" + (excludeIdCheck ? string.Empty : scriptWithId) + "if(c===document.body){return c.tagName}var a=0;var e=c.parentNode.childNodes;for(var b=0;b<e.length;b++){var d=e[b];if(d===c){return gPt(c.parentNode)+'/'+c.tagName+'['+(a+1)+']'}if(d.nodeType===1&&d.tagName===c.tagName){a++}}};return gPt(arguments[0]);";

            var path = (string)driver.ExecuteJavaScript(scriptToGetXpath, e);

            if (!path.StartsWith("//"))
            {
                path = "//" + path;
            }
            return path;
        }





        /// <summary>
        /// Retruns the first visibile/Enabled webelement from a list of webelements
        /// </summary>
        /// <param name="elementsList"></param>
        /// <returns></returns>
        public static IWebElement GetFirstVisibleElement(this IList<IWebElement> elementsList)
            => elementsList?.FirstOrDefault(elm => elm.IsVisible() && elm.IsCssDisplayed());

        public static List<IWebElement> GetAllVisibleElements(this IList<IWebElement> elementsList)
            => elementsList?.Where(elm => elm.IsVisible() && elm.IsCssDisplayed()).EmptyIfNull().ToList();

        public static bool IsReadonly(this IWebElement element) => element.Exists() && element.GetAttribute("readonly").HasValue();
        public static bool IsEnabled(this IWebElement element) => element.Exists() && element.Enabled;
        public static bool IsDisabled(this IWebElement element) => element.Exists() && !(element.IsEnabled() || element.IsReadonly());
        public static bool IsVisible(this IWebElement element) => element.Exists() && element.Displayed;
        public static bool IsCssDisplayed(this IWebElement element) => element.Exists() && !element.GetCssValue("display").EqualsIgnoreCase("none");

        public static bool Exists(this IWebElement element)
        {
            try
            {
                return element != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void WaitAndClick(this IWebElement element, IWebDriver driver, int waitSeconds = 0)
        {
            waitSeconds = waitSeconds == 0 ? AppConfig.DefaultTimeoutWaitPeriodInSeconds : waitSeconds;
            element.WaitForElementVisible(driver, waitTimeSec: waitSeconds);
            element.Click();
        }

        /// <summary>
        /// Wait for element be visible and input text
        /// </summary>
        /// <param name="element"></param>
        /// <param name="input"></param>
        public static void WaitAndSendKeys(this IWebElement element, IWebDriver driver, string valueToSet, int waitSeconds = 0)
        {
            waitSeconds = waitSeconds == 0 ? AppConfig.DefaultTimeoutWaitPeriodInSeconds : waitSeconds;
            element.WaitForElementVisible(driver, waitTimeSec: waitSeconds);
            element.Clear();
            element.SendKeys(valueToSet);
        }


        public static void Highlight(this IWebElement element, IWebDriver driver, string overrideStyle = "")
        {
            try
            {
                overrideStyle = overrideStyle.IsEmpty() ? __HightElementStyle : overrideStyle;
                driver.ExecuteJavaScript($"arguments[0].setAttribute('style', '{overrideStyle}');", element);
            }
            catch (Exception)
            {
                // ignored
            }
        }


    }
}
