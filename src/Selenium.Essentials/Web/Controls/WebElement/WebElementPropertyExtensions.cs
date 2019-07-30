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
        public static IWebElement GetFirstVisibleElement(this IList<IWebElement> elementsList)
            => elementsList?.FirstOrDefault(elm => elm.IsVisible() && elm.IsCssDisplayed());
        public static List<IWebElement> GetAllVisibleElements(this IList<IWebElement> elementsList)
            => elementsList?.Where(elm => elm.IsVisible() && elm.IsCssDisplayed()).EmptyIfNull().ToList();

        public static bool IsReadonly(this IWebElement element) => element.Exists() && element.GetAttribute("readonly").HasValue();
        public static bool IsDisabled(this IWebElement element) => element.Exists() && !(element.IsEnabled() || element.IsReadonly());
        public static bool IsEnabled(this IWebElement element) => element.Exists() && element.Enabled;
        public static bool IsVisible(this IWebElement element) => element.Exists() && element.Displayed && element.IsCssDisplayed();
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

        public static string Value(this IWebElement element) => element.GetAttribute("value");
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
        public static string Class(this IWebElement element) => element.GetAttribute("class");
        public static string[] Classes(this IWebElement element) => element.Class().SplitAndTrim(" ").ToArray();

        public static string Id(this IWebElement element) => element.GetAttribute("id");
        public static Dictionary<string, string> GetAttributes(this IWebElement element)
        {
            return new Dictionary<string, string>();
        }

        public static void WaitAndClick(this IWebElement element, IWebDriver driver, int waitSeconds = 0)
        {
            waitSeconds = waitSeconds == 0 ? SeAppConfig.DefaultTimeoutWaitPeriodInSeconds : waitSeconds;
            element.WaitForElementVisible(driver, waitTimeSec: waitSeconds);
            element.Click();
        }
        public static void WaitAndSendKeys(this IWebElement element, IWebDriver driver, string valueToSet, int waitSeconds = 0)
        {
            waitSeconds = waitSeconds == 0 ? SeAppConfig.DefaultTimeoutWaitPeriodInSeconds : waitSeconds;
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
