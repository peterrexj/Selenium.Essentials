using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Selenium.Essentials
{
    public static partial class WebElementExtensions
    {
        private const string __HightElementStyle = "background: yellow; border: 2px solid red;";

        /// <summary>
        /// Returns the Xpath selector of the element.
        /// </summary>
        /// <param name="e">web element</param>
        /// <param name="driver">IWebDriver associated with the element</param>
        /// <param name="excludeIdCheck">Exclude ID based xpath selector</param>
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
        /// Returns the first element which is visible in the browser from the collection
        /// </summary>
        /// <param name="elementsList">collection of elements in which first visible element will be searched</param>
        /// <returns>return first IWebElement which is visible from the collection</returns>
        public static IWebElement GetFirstVisibleElement(this IList<IWebElement> elementsList)
            => elementsList?.FirstOrDefault(elm => elm.IsVisible() && elm.IsCssDisplayed());

        /// <summary>
        /// Returns all the visible element in the browser from the collection
        /// </summary>
        /// <param name="elementsList">collection of elements in which visible elements will be searched</param>
        /// <returns>collection of IWebElements visible in the UI</returns>
        public static List<IWebElement> GetAllVisibleElements(this IList<IWebElement> elementsList)
            => elementsList?.Where(elm => elm.IsVisible() && elm.IsCssDisplayed()).EmptyIfNull().ToList();

        /// <summary>
        /// Whether element is readonly
        /// </summary>
        /// <param name="element">element on which operation needs to be performed</param>
        /// <returns>true if readonly or else false</returns>
        public static bool IsReadonly(this IWebElement element) => element.Exists() && element.GetAttribute("readonly").HasValue();

        /// <summary>
        /// Whether element is disabled (conditions to satisfy - Exists(), not Enabled(), not Readonly())
        /// </summary>
        /// <param name="element">element on which operation needs to be performed</param>
        /// <returns>true if disabled or else false</returns>
        public static bool IsDisabled(this IWebElement element) => element.Exists() && !(element.IsEnabled() || element.IsReadonly());

        /// <summary>
        /// Whether element is enabled
        /// </summary>
        /// <param name="element">element on which operation needs to be performed</param>
        /// <returns>true if enabled or else false</returns>
        public static bool IsEnabled(this IWebElement element) => element.Exists() && element.Enabled;

        /// <summary>
        /// Whether element is visible
        /// </summary>
        /// <param name="element">element on which operation needs to be performed</param>
        /// <returns>true if visible or else false</returns>
        public static bool IsVisible(this IWebElement element) => element.Exists() && element.Displayed && element.IsCssDisplayed();

        /// <summary>
        /// Whether element is CSS displayed (display: none is not applied to the element)
        /// </summary>
        /// <param name="element">element on which operation needs to be performed</param>
        /// <returns>true if css displayed or else false</returns>
        public static bool IsCssDisplayed(this IWebElement element) => element.Exists() && !element.GetCssValue("display").EqualsIgnoreCase("none");

        /// <summary>
        /// Whether element exists
        /// </summary>
        /// <param name="element">element on which operation needs to be performed</param>
        /// <returns>true if the element exists or else false</returns>
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

        /// <summary>
        /// Returns the value of the [value] attribute of the element 
        /// </summary>
        /// <param name="element">element on which operation needs to be performed</param>
        /// <returns>attribute:value's content</returns>
        public static string Value(this IWebElement element) => element.GetAttribute("value");

        /// <summary>
        /// Returns the text available on the element. Either text or the attribute("value") is returned which ever is available
        /// </summary>
        /// <param name="element">element on which operation needs to be performed</param>
        /// <returns>Text or Attribute(value) which ever is available</returns>
        public static string Text(this IWebElement element)
        {
            try
            {
                if (element.Text.HasValue())
                {
                    return element.Text;
                }

                try
                {
                    if (element.Value().HasValue())
                    {
                        return element.Value();
                    }
                }
                catch (Exception)
                {
                    // ignored
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot get Text from the control due to: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Returns all the class available on the element
        /// </summary>
        /// <param name="element">element on which operation needs to be performed</param>
        /// <returns>class available to the element</returns>
        public static string Class(this IWebElement element) => element.GetAttribute("class");

        /// <summary>
        /// Returns all class available on the element
        /// </summary>
        /// <param name="element">element on which operation needs to be performed</param>
        /// <returns>collection of class as an array available to the element</returns>
        public static string[] Classes(this IWebElement element) => element.Class().SplitAndTrim(" ").ToArray();

        /// <summary>
        /// Returns the value of the [id] attribute of the element 
        /// </summary>
        /// <param name="element"></param>
        /// <returns>value of the [id] attribute</returns>
        public static string Id(this IWebElement element) => element.GetAttribute("id");

        /// <summary>
        /// Returns all attributes available to the element
        /// </summary>
        /// <param name="element"></param>
        /// <returns>Dictionary of all attributes for the element</returns>
        public static Dictionary<string, string> GetAttributes(this IWebElement element)
        {
            return new Dictionary<string, string>();
        }

        /// <summary>
        /// Wait for the element and performs a click operation
        /// </summary>
        /// <param name="element">element on which the click has to be performed</param>
        /// <param name="driver">driver associated with the element in order to perform waits</param>
        /// <param name="waitSeconds">total amount of time to wait for the element to be available</param>
        public static void WaitAndClick(this IWebElement element, IWebDriver driver, int waitSeconds = 0)
        {
            waitSeconds = waitSeconds == 0 ? SeAppConfig.DefaultTimeoutWaitPeriodInSeconds : waitSeconds;
            element.WaitForElementVisible(driver, waitTimeSec: waitSeconds);
            element.Click();
        }

        /// <summary>
        /// Wait for the element and send the keys
        /// </summary>
        /// <param name="element">element on which the send operation has to be performed</param>
        /// <param name="driver">driver associated with the element in order to perform wait</param>
        /// <param name="valueToSet">value to send</param>
        /// <param name="waitSeconds">total amount of time to wait for the element to be available</param>
        public static void WaitAndSendKeys(this IWebElement element, IWebDriver driver, string valueToSet, int waitSeconds = 0)
        {
            waitSeconds = waitSeconds == 0 ? SeAppConfig.DefaultTimeoutWaitPeriodInSeconds : waitSeconds;
            element.WaitForElementVisible(driver, waitTimeSec: waitSeconds);
            element.Clear();
            element.SendKeys(valueToSet);
        }

        /// <summary>
        /// Highlights the element in the browser by drawing a border or override using your style
        /// </summary>
        /// <param name="element">element on which the send operation has to be performed</param>
        /// <param name="driver">driver associated with the element in order to perform wait</param>
        /// <param name="overrideStyle">style to needs to be applied to the element</param>
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
