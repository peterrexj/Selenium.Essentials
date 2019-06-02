using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Essentials.Web.Controls
{
    public interface IBaseControl
    {
        IWebElement RawElement { get; }
        void Click();
        bool IsReadonly { get; }
        bool Exists { get; }
        bool CssDisplayed { get; }
        void Clear();
    }
}
