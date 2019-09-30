using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Essentials
{
    public static class WebDriverExtensions
    {
        public enum Device
        {
            Mobile,
            Tablet,
            Desktop,
            Default
        }

        public static void Navigate(this IWebDriver driver, string url, bool refreshOnSamePage = true)
        {
            if (driver.Url.EqualsIgnoreCase(url) || driver.Url.ContainsIgnoreCase(url))
            {
                if (refreshOnSamePage)
                {
                    driver.Refresh();
                }
            }
            else
            {
                driver.Navigate().GoToUrl(url);
            }
        }

        public static void Refresh(this IWebDriver driver)
        {
            driver?.Navigate().Refresh();
        }

        public static void CloseDriver(this IWebDriver driver)
        {
            driver?.Quit();
        }

        [DebuggerStepThrough]
        public static void ByPassCertificateErrorOnPage(this IWebDriver driver)
        {
            if (driver.Title.EqualsIgnoreCase("Certificate Error: Navigation Blocked"))
            {
                driver.Navigate().GoToUrl("javascript:document.getElementById('overridelink').click()");
            }
        }

        public static void Resize(this IWebDriver driver, Device size)
        {
            switch (size)
            {
                case Device.Desktop:
                    driver.Manage().Window.Maximize();
                    break;
                case Device.Tablet:
                    driver.Manage().Window.Size = Utility.TabletWindowSize;
                    break;
                case Device.Mobile:
                    driver.Manage().Window.Size = Utility.MobileWindowSize;
                    break;
                case Device.Default:
                default:
                    break;
            }
        }

        public static object ExecuteJavaScript(this IWebDriver driver, string script, params object[] args) => (driver as IJavaScriptExecutor).ExecuteScript(script, args);
        public static object ExecuteJavaScript(this IWebDriver driver, string script, bool supressErrors, params object[] args)
        {
            try
            {
                return ExecuteJavaScript(driver, script, args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return String.Empty;
            }
        }

        public static void TakeScreenShot(this IWebDriver driver, string path)
        {
            try
            {
                if (driver == null) return;
                var screenShot = ((ITakesScreenshot)driver).GetScreenshot();
                screenShot.SaveAsFile(path, ScreenshotImageFormat.Jpeg);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static Dictionary<string, string> Capabilities(this IWebDriver driver)
        {
            if (driver is RemoteWebDriver)
            {
                return SerializationHelper.JsonToDictionary((driver as RemoteWebDriver).Capabilities.ToString());
            }
            return null;
        }

        public static void ScrollToPageTop(this IWebDriver driver)
        {
            try
            {
                driver.ExecuteJavaScript("window.scrollTo(0, 0)");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while scroll page to top {e.Message}");
            }
        }

        public static void ScrollToPageBottom(this IWebDriver driver)
        {
            try
            {
                driver.ExecuteJavaScript("window.scrollTo(0, document.body.scrollHeight);");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while scroll page to bottom {e.Message}");
            }
        }

        public static void Scroll(this IWebDriver driver, int length)
        {
            try
            {
                driver.ExecuteJavaScript($"window.scrollBy(0,{length})", "");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while scroll {e.Message}");
            }
        }

        public static void WaitTillPageLoad(this IWebDriver driver, int timeToWaitInSeconds = 0)
        {
            WebElementExtensions.WaitGeneric(
                element: null, 
                driver: driver,
                waitTimeSec: timeToWaitInSeconds,
                throwExceptionWhenNotFound: true,
                errorMessage: "failed while waiting for document ready state",
                process: () => driver.ExecuteJavaScript("return document.readyState").Equals("complete"),
                reasonForFailedCondition: "");
        }
    }
}
