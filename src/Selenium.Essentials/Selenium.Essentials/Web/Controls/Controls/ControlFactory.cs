using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Selenium.Essentials.Web.Controls.Controls
{
    /// <summary>
    /// Factory for producing user specific control on request
    /// </summary>
    internal static class ControlFactory
    {
        /// <summary>
        /// Create a control based on the type
        /// </summary>
        /// <typeparam name="T">Type of control, for example WebControl or Button</typeparam>
        /// <param name="driver">WebDriver related to area where the control is going be created</param>
        /// <param name="selector">How to map the control in the UI</param>
        /// <param name="parentControl">If the control is to be search with in a parent control</param>
        /// <returns></returns>
        public static T CreateNew<T>(IWebDriver driver, By selector, IBaseControl parentControl = null) where T : BaseControl
        {
            var type = typeof(T).Name;

            if (typeof(T) == typeof(WebControl))
            {
                return new WebControl(driver, selector, parentControl as BaseControl) as T;
            }
            else if (typeof(T) == typeof(ButtonControl))
            {
                return new ButtonControl(driver, selector, parentControl as BaseControl) as T;
            }
            else if (typeof(T) == typeof(Checkbox))
            {
                return new Checkbox(driver, selector, parentControl as BaseControl) as T;
            }
            else if (typeof(T) == typeof(LinkControl))
            {
                return new LinkControl(driver, selector, parentControl as BaseControl) as T;
            }
            else if (typeof(T) == typeof(TextboxControl))
            {
                return new TextboxControl(driver, selector, parentControl as BaseControl) as T;
            }
            else if (typeof(T) == typeof(SelectControl))
            {
                return new SelectControl(driver, selector, parentControl as BaseControl) as T;
            }
            else if (typeof(T) == typeof(FileUploadControl))
            {
                return new FileUploadControl(driver, selector, parentControl as BaseControl) as T;
            }
            else
            {
                return new WebControl(driver, selector, parentControl as BaseControl) as T;
            }
        }
    }
}
