using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Essentials
{
    public interface IBaseControl : IWaitControl
    {
        IWebElement RawElement { get; }
        IWebDriver Driver { get; }
        IBaseControl ParentControl { get; }
        IWebElement ParentRawElement { get; }
        string ElementId { get; }
        By By { get; }
        string Description { get; }

        bool IsReadonly { get; }
        bool IsDisabled { get; }
        bool IsEnabled { get; }
        bool IsVisible { get; }
        bool CssDisplayed { get; }
        bool IsDisplayed { get; }
        bool IsHidden { get; }
        bool Exists { get; }
        string Value { get; }
        string Text { get; }

        void Click();
        void WaitAndClick(int timeToWaitInSeconds);
        void ScrollAndClick();
        void ClickByJsScript();
        void WaitClickTillElementGoesInvisible();
        void WaitClickAndIgnoreError();

        void SendKeys(string textToSet);
        void SendEnter();
        void SendTab();

        void ScrollTo();
        void Clear();
        void Highlight();
        void SetFocusByJavascript();

        string ToString();
    }
}
