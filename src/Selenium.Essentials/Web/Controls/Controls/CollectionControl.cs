using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Essentials
{
    public class CollectionControl : BaseControl
    {
        private Action _scrollCustomEvent;
        private Action<int> _scrollCustomEventConditional;
        private bool _excludeIdChecksForXpathCalculation;

        public CollectionControl(IWebDriver driver, By by, BaseControl parentControl = null, string description = null, bool firstAvailable = false, bool excludeIdChecksForXpathCalculation = false)
            : base(driver, by, parentControl, description, firstAvailable)
        {
            _excludeIdChecksForXpathCalculation = excludeIdChecksForXpathCalculation;
        }

        public CollectionControl(IWebDriver driver, By by, Action customScroll, BaseControl parentControl = null, string description = null, bool firstAvailable = false)
            : base(driver, by, parentControl, description, firstAvailable)
        {
            _scrollCustomEvent = customScroll;
        }

        public CollectionControl(IWebDriver driver, By by, Action<int> customScroll, BaseControl parentControl = null, string description = null, bool firstAvailable = false)
            : base(driver, by, parentControl, description, firstAvailable)
        {
            _scrollCustomEventConditional = customScroll;
        }

        /// <summary>
        /// Total number of elements matching the selector including the hidden
        /// </summary>
        public int TotalRaw => Driver.FindElements(By).Count;

        /// <summary>
        /// Total number of visible elements matching the selector
        /// </summary>
        public int Total => Enumerable.Range(1, TotalRaw).Count(pos => Item(pos).IsDisplayed && Item(pos).CssDisplayed);

        /// <summary>
        /// Get the control of type T at the given position which match in the UI 
        /// </summary>
        /// <typeparam name="T">Type of control to be returned. Example: WebControl, Button, Checkbox, Textbox</typeparam>
        /// <param name="position">Position of the element as visible in the UI</param>
        /// <returns></returns>
        public T Item<T>(int position) where T : BaseControl
        {
            (position <= TotalRaw).Should()
                .BeTrue($"The requested item position [{position}] is greater than the total items [{TotalRaw}] available in the UI now");

            var xpath = Driver.FindElements(By).Skip(position - 1).First().GetElementXPath(Driver, _excludeIdChecksForXpathCalculation);

            if (_scrollCustomEvent != null)
            {
                ControlFactory.CreateNew<WebControl>(Driver, By.XPath(xpath), ParentControl).ScrollTo();
                _scrollCustomEvent.Invoke();
            }

            if (_scrollCustomEventConditional != null)
            {
                ControlFactory.CreateNew<WebControl>(Driver, By.XPath(xpath), ParentControl).ScrollTo();
                _scrollCustomEventConditional.Invoke(position);
            }

            return ControlFactory.CreateNew<T>(Driver, By.XPath(xpath), ParentControl);
        }


        /// <summary>
        /// Get the control of type WebControl at the given position which match in the UI 
        /// </summary>
        /// <param name="position">Position of the element as visible in the UI</param>
        /// <returns></returns>
        public WebControl Item(int position) => Item<WebControl>(position);

        /// <summary>
        /// Click on the element at the given position
        /// </summary>
        /// <param name="position">Position of the element as visible in the UI</param>
        public void Click(int position) => Item(position).Click();

        /// <summary>
        /// Set value to the element 
        /// </summary>
        /// <param name="position">Position of the element as visible in the UI</param>
        /// <param name="value">Value that needs to be set to the element</param>
        public void Set(int position, string value) => Item<TextboxControl>(position).Set(value);

        /// <summary>
        /// Get value of the element at the given position
        /// </summary>
        /// <param name="position">Position of the element as visible in the UI</param>
        /// <returns></returns>
        public string Get(int position) => Item(position).Value ?? Item(position).Text;

        /// <summary>
        /// Get value of all the elements available in the UI matching the selector
        /// </summary>
        /// <returns>Collection of string value extracted from each element</returns>
        public IList<string> Get() => Enumerable.Range(1, Total).Select(Get).ToList();

        /// <summary>
        /// Finds the element position based on the text it contains
        /// </summary>
        /// <param name="valueToSearch">Value to match</param>
        /// <returns>Position of the element as visible in the UI</returns>
        public int FindPositionByText(string valueToSearch) => Get().Select(item => item.ToLower()).ToList().IndexOf(valueToSearch.ToLower()) + 1;

        /// <summary>
        /// Waits till the element on the position is available in the UI
        /// </summary>
        /// <param name="position">Position of the element as visible in the UI</param>
        /// <param name="waitTimeSec">Maximum amount of time to wait</param>
        /// <param name="throwExceptionWhenNotFound">Throw exception if the element is not found</param>
        /// <param name="errorMessage">Error message text when the element is not found</param>
        public void WaitForElementVisible(int position, int waitTimeSec = 0, bool throwExceptionWhenNotFound = true, string errorMessage = "")
        {
            (Total == 0 && position > 1).Should()
                .BeFalse($"The are no UI elements matching and you have requested for {position} to appear");

            if (Total == 0) //If there are no such element in the UI then wait for the first one to appear
            {
                RawElement.WaitGeneric(driver: Driver,
                    waitTimeSec: waitTimeSec,
                    throwExceptionWhenNotFound: throwExceptionWhenNotFound,
                    errorMessage: errorMessage,
                    () => Total == position,
                    $"Collection Control failed on element to be visible {By}",
                    baseControl: this);
            }
        }

        /// <summary>
        /// Waits till the element in the position is removed from the UI
        /// </summary>
        /// <param name="waitTimeSec">Maximum amount of time to wait</param>
        /// <param name="throwExceptionWhenNotFound">Throw exception if the element is not found</param>
        /// <param name="errorMessage">Error message text when the element is not found</param>
        public new void WaitForElementInvisible(int waitTimeSec = 0, bool throwExceptionWhenNotFound = true, string errorMessage = "")
        {
            if (Total <= 0) return;

            var tempTotal = Total;
            RawElement.WaitGeneric(driver: Driver,
                    waitTimeSec: waitTimeSec,
                    throwExceptionWhenNotFound: throwExceptionWhenNotFound,
                    errorMessage: errorMessage,
                    () => tempTotal < Total,
                    $"Collection Control failed on element to go invisible {By}",
                    baseControl: this);
        }
    }
}
