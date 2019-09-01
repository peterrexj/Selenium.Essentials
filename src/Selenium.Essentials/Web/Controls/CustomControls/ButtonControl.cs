using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Essentials
{
    /// <summary>
    /// Button control to use in page object. 
    /// </summary>
    public class ButtonControl : BaseControl
    {
        public ButtonControl(IWebDriver driver, By by, BaseControl parentControl = null, string description = null, bool firstAvailable = false)
            : base(driver, by, parentControl, description, firstAvailable)
        {
        }
    }
}
