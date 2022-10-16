using OpenQA.Selenium;
using Pj.Library;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public int TotalRaw => NotExists ? 0 : RetryFindElements().Count;

        /// <summary>
        /// Total number of visible elements matching the selector
        /// </summary>
        public int Total => Enumerable.Range(0, TotalRaw).Count(pos => Item(pos).IsDisplayed && Item(pos).CssDisplayed);

        /// <summary>
        /// Get the control of type T at the given position which match in the UI 
        /// </summary>
        /// <typeparam name="T">Type of control to be returned. Example: WebControl, Button, Checkbox, Textbox</typeparam>
        /// <param name="position">Position of the element as visible in the UI</param>
        /// <returns></returns>
        public T Item<T>(int position) where T : BaseControl
        {
            if (position > TotalRaw)
                throw new Exception($"The requested item position [{position}] is greater than the total items [{TotalRaw}] available in the UI now");

            var xpath = RetryFindElements().Skip(position - 1).FirstOrDefault().GetElementXPath(Driver, _excludeIdChecksForXpathCalculation);

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

        public T VisibleItem<T>(int position) where T : BaseControl
        {
            if (position > TotalRaw)
                throw new Exception($"The requested item position [{position}] is greater than the total items [{TotalRaw}] available in the UI now");

            var xpath = RetryFindElements().Where(elm => elm.IsVisible()).Skip(position - 1).FirstOrDefault().GetElementXPath(Driver, _excludeIdChecksForXpathCalculation);

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
        /// Return the first visible control from the list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public T FirstVisibleControl<T>() where T : BaseControl
        {
            if (Total <= 0)
                throw new Exception($"The total count of controls within this collection is equal to 0 and cannot find any elements by {By}");

            var xpath = RetryFindElements().FirstOrDefault(elm => elm.IsVisible()).GetElementXPath(Driver, _excludeIdChecksForXpathCalculation);

            return ControlFactory.CreateNew<T>(Driver, By.XPath(xpath), ParentControl);
        }

        public WebControl FirstVisibleElement => FirstVisibleControl<WebControl>();

        /// <summary>
        /// Click on the element at the given position
        /// </summary>
        /// <param name="position">Position of the element as visible in the UI</param>
        public void Click(int position) => Item(position).Click();

        public void DoubleClick(int position) => Item(position).DoubleClick();

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
        public string Get(int position) => Item(position).Text;

        /// <summary>
        /// Get value of all the elements available in the UI matching the selector
        /// </summary>
        /// <returns>Collection of string value extracted from each element</returns>
        public IEnumerable<string> Get()
        {
            var currentTotal = Total;
            for (int i = 1; i <= currentTotal; i++)
            {
                yield return Get(i);
            }
        }

        /// <summary>
        /// Finds the element position based on the text it contains
        /// </summary>
        /// <param name="valueToSearch">Value to match</param>
        /// <returns>Position of the element as visible in the UI</returns>
        public int FindPositionByText(string valueToSearch)
        {
            var currentTotal = Total;

            for (int i = 1; i <= currentTotal; i++)
            {
                if (Get(i).EqualsIgnoreCase(valueToSearch))
                {
                    return i;
                }
            }
            return 0;
        }

        /// <summary>
        /// Waits till the element at the position appears
        /// </summary>
        /// <param name="position"></param>
        /// <param name="waitTimeSec"></param>
        /// <param name="throwExceptionWhenNotFound"></param>
        /// <param name="errorMessage"></param>
        public void WaitForMinimumOne(int position = 1, int waitTimeSec = 0, bool throwExceptionWhenNotFound = true, string errorMessage = "")
        {
            if (Total == 0)
            {
                RawElement.WaitGeneric(driver: Driver,
                   waitTimeSec: waitTimeSec,
                   throwExceptionWhenNotFound: throwExceptionWhenNotFound,
                   errorMessage: errorMessage,
                   () => Total == position,
                   $"Collection Control failed on to find the total element by {By}",
                   baseControl: this);
            }
        }

        /// <summary>
        /// Waits till the element on the position is available in the UI
        /// </summary>
        /// <param name="position">Position of the element as visible in the UI</param>
        /// <param name="waitTimeSec">Maximum amount of time to wait</param>
        /// <param name="throwExceptionWhenNotFound">Throw exception if the element is not found</param>
        /// <param name="errorMessage">Error message text when the element is not found</param>
        public void WaitForElementVisible(int position, int waitTimeSec = 0, bool throwExceptionWhenNotFound = true, string errorMessage = "")
        {
            var currentTotal = Total;

            if (currentTotal == 0 && position > 1)
                throw new Exception($"The are no UI elements matching and you have requested for {position} to appear");

            if (currentTotal == 0) //If there are no such element in the UI then wait for the first one to appear
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
        public void WaitForElementInvisible(int waitTimeSec = 0, bool throwExceptionWhenNotFound = true, string errorMessage = "")
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