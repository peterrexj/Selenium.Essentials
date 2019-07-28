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

        public static void Navigate(this IWebDriver driver, string url)
        {
            if (driver.Url.EqualsIgnoreCase(url))
            {
                driver.Refresh();
            }
            else
            {
                driver.Navigate().GoToUrl(url);
            }
        }

        /// <summary>
        /// Bypass Certificate error page in IE browsers
        /// </summary>
        /// <param name="driver">webdriver</param>
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
            catch (Exception)
            {
                return String.Empty;
            }
        }

        public static void Refresh(this IWebDriver driver)
        {
            try
            {
                driver.Navigate().Refresh();
            }
            catch (Exception) { }
        }

        public static void CloseDriver(this IWebDriver driver)
        {
            driver.Close();
            driver.Quit();
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
        
        public static string GetBrowserType(this IWebDriver driver)
        {
            ICapabilities capabilities = ((RemoteWebDriver)driver).Capabilities;
            return capabilities["BrowserName"].ToString();
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

        /// <summary>
        /// to scroll up or down the browser
        /// </summary>
        /// <param name="Driver">weddriver</param>
        /// <param name="length">how much you like to scroll in pixel minus value means scroll up</param>
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

        ///// <summary>
        ///// Scroll to the web element
        ///// </summary>
        ///// <param name="_driver"></param>
        ///// <param name="element"></param>
        //public static void ScrollToWebElement(this IWebDriver driver, IWebElement element)
        //{
        //    try
        //    {
        //        Actions actnew = new Actions(driver);
        //        element.ConditionalWait(TimeSpan.FromSeconds(40), TimeSpan.FromSeconds(2));
        //        actnew.MoveToElement(element);
        //        actnew.Build().Perform();
        //        Utility.HardWait(1);
        //    }
        //    catch (Exception)
        //    {
        //        driver.Scroll("600");
        //    }
        //}

        ///// <summary>
        ///// add waiting time for findelement
        ///// </summary>
        ///// <param name="driver"></param>
        ///// <param name="by"></param>
        ///// <param name="timeoutInSeconds">how many sec for waiting this </param>
        ///// <returns></returns>
        //public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        //{
        //    try
        //    {
        //        if (timeoutInSeconds > 0)
        //        {
        //            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
        //            return wait.Until(drv => drv.FindElement(@by));
        //        }

        //        return driver.FindElement(@by);
        //    }
        //    catch (WebDriverTimeoutException)
        //    {
        //        return null;
        //    }
        //}



        //public static void ImplicitWait(this IWebDriver driver, int seconds)
        //{
        //    driver.Manage().Timeouts().ImplicitWait = (TimeSpan.FromSeconds(seconds));
        //}

        ///// <summary>
        ///// Swith frame and wait for specified elemnet to be enabled.
        ///// </summary>
        ///// <param name="driver"> webdriver</param>
        ///// <param name="element">webelement</param>
        ///// <returns>bool</returns>
        //public static bool WaitAndSwitchForFrame(this IWebDriver driver, IWebElement element)
        //{
        //    // int i = 0;
        //    // bool Flag = false;
        //    // System.Threading.Thread.Sleep(10000);
        //    DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(driver)
        //    {
        //        Timeout = TimeSpan.FromSeconds(Waits._maxWaitTime),
        //        PollingInterval = TimeSpan.FromMilliseconds(500)
        //    };
        //    wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
        //    bool waitcondition = wait.Until<bool>((dd) =>
        //    {
        //        //i = i++;
        //        // Log(i);
        //        try
        //        {
        //            //  dd.
        //            dd.SwitchTo().DefaultContent();
        //            // System.Threading.Thread.Sleep(1000);

        //            dd.SwitchTo().Frame(0);
        //            if (element.Enabled & element.Displayed)
        //            {
        //                // element.Click();
        //                return true;
        //            }
        //            else { return false; }
        //        }
        //        catch (Exception)
        //        {
        //            // Log(e.Message);
        //            return false;
        //        }
        //    });

        //    return waitcondition;
        //}

        ///// <summary>
        ///// This is to wait for the page to load completly
        ///// </summary>
        ///// <param name="driver"></param>
        //public static void WaitForPageLoadComplete(this IWebDriver driver)
        //{
        //    new WebDriverWait(driver, TimeSpan.FromSeconds(350)).Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
        //}

        ///// <summary>
        ///// Verify the documents are downloaded
        ///// </summary>
        ///// <returns></returns>

        //public static bool VerifyDownload(this IWebDriver driver, string documentName)
        //{
        //    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

        //    js.ExecuteScript("window.open()");
        //    Utility.HardWait(10);
        //    driver.SwitchTo().Window(driver.WindowHandles[1]);

        //    driver.Navigate().GoToUrl("chrome://downloads/");

        //    IWebElement searchBox = driver.FindElement(By.XPath("//body"));
        //    if (searchBox.Text.ToLower().Contains(documentName.ToLower()))
        //    {
        //        driver.TakeScreenShot();
        //        driver.Close();
        //        driver.SwitchTo().Window(driver.WindowHandles[0]);
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }

        //}
    }
}
