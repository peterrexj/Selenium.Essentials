using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Essentials.Web.Controls
{
    public class ElementUnavailableException : WebControlException
    {
        public ElementUnavailableException(IWebDriver driver, string message, BaseControl uiControl)
            : base(driver, CreateGenericDetailsMessage(driver, uiControl, message), uiControl)
        {
        }

        public ElementUnavailableException(IWebDriver driver, BaseControl uiControl)
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
