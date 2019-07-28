using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Essentials
{
    public class ElementUnavailableException : WebControlException
    {
        public ElementUnavailableException(IWebDriver driver, string message, IBaseControl uiControl)
            : base(driver, CreateGenericDetailsMessage(driver, uiControl, message), uiControl)
        {
        }

        public ElementUnavailableException(IWebDriver driver, IBaseControl uiControl)
            : base(driver, CreateGenericDetailsMessage(driver, uiControl, "Element unavailable"), uiControl)
        {
        }

        public ElementUnavailableException(IWebDriver driver, string message, IWebElement uiControl)
            : base(driver, CreateGenericDetailsMessage(driver, uiControl, message), uiControl)
        {
        }

        public ElementUnavailableException(IWebDriver driver, IWebElement uiControl)
            : base(driver, CreateGenericDetailsMessage(driver, uiControl, "Element unavailable"), uiControl)
        {
        }
    }
}
