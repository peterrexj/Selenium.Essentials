using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Essentials
{
    public class ImageControl : BaseControl
    {
        public ImageControl(IWebDriver driver, By by, BaseControl parentControl = null, string description = null, bool firstAvailable = false)
            : base(driver, by, parentControl, description, firstAvailable)
        {
        }

        public string Source => GetAttribute("src");
    }
}