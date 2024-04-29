using OpenQA.Selenium;
using Pj.Library;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Selenium.Essentials
{
    public class CollectionControl : BaseControl
    {
        private readonly Action? _scrollCustomEvent;
        private readonly Action<int>? _scrollCustomEventConditional;
        private readonly bool _excludeIdChecksForXpathCalculation;

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

        #region Properties

        /// <summary>
        /// Total number of elements matching the selector including the hidden
        /// </summary>
        public int TotalRaw => NotExists ? 0 : RetryFindElements().Count;

        /// <summary>
        /// Backward compatibility to the Total field. Total field used to remove the hidden items but this feature is not supported anymore in the new implementation
        /// </summary>
        public int Total => TotalRaw;

        #endregion

        #region Item
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

        /// <summary>
        /// The VisibleItem<T> method in the CollectionControl class is a generic method that returns a control of type T at a given position from a collection of controls. The type T must be a subclass of BaseControl.
        ///The method takes an integer position as a parameter, which represents the position of the control in the user interface. It first checks if the requested position is greater than the total number of controls.If it is, an exception is thrown.
        ///Then, it finds the control at the given position that is visible, and retrieves its XPath.If a custom scroll event is defined, it scrolls to the control and invokes the event.
        ///Finally, it creates a new control of type T at the XPath location and returns it.If the control at the given position is not visible, an exception will be thrown.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="position"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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
            if (TotalRaw <= 0)
                throw new Exception($"The total count of controls within this collection is equal to 0 and cannot find any elements by {By}");

            var xpath = RetryFindElements().FirstOrDefault(elm => elm.IsVisible()).GetElementXPath(Driver, _excludeIdChecksForXpathCalculation);

            return ControlFactory.CreateNew<T>(Driver, By.XPath(xpath), ParentControl);
        }

        /// <summary>
        /// Return the first visible control from the list
        /// </summary>
        public WebControl FirstVisibleElement => FirstVisibleControl<WebControl>();

        #endregion

        #region Operations

        /// <summary>
        /// Click on the element at the given position
        /// </summary>
        /// <param name="position">Position of the element as visible in the UI</param>
        public void Click(int position) => Item(position).Click();

        /// <summary>
        /// Perform a double click operation on the element at the given position
        /// </summary>
        /// <param name="position"></param>
        public void DoubleClick(int position) => Item(position).DoubleClick();

        /// <summary>
        /// Set value to the element 
        /// </summary>
        /// <param name="position">Position of the element as visible in the UI</param>
        /// <param name="value">Value that needs to be set to the element</param>
        public void Set(int position, string value) => Item<TextboxControl>(position).Set(value);

        #endregion

        #region Find & Get

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
            var result = new List<string>();

            (ParentControl == null
                ? RawElement.FindElements(By)
                : ParentControl.RawElement.FindElements(By)).Select(f => f?.Text).Where(f => f.HasValue()).Iter(r => result.Add(r));

            return result;
        }

        /// <summary>
        /// Finds the element position based on the text it contains
        /// </summary>
        /// <param name="valueToSearch">Value to match</param>
        /// <returns>Position of the element as visible in the UI</returns>
        public int FindPositionByText(string valueToSearch) => Get().Select(item => item.ToLower()).ToList().IndexOf(valueToSearch.ToLower()) + 1;

        #endregion

        #region Waits

        /// <summary>
        /// Waits till the element at the position appears
        /// </summary>
        /// <param name="position"></param>
        /// <param name="waitTimeSec"></param>
        /// <param name="throwExceptionWhenNotFound"></param>
        /// <param name="errorMessage"></param>
        public void WaitForMinimumOne(int position = 1, int waitTimeSec = 0, bool throwExceptionWhenNotFound = true, string errorMessage = "")
        {
            if (TotalRaw == 0)
            {
                RawElement.WaitGeneric(driver: Driver, waitTimeSec: waitTimeSec, throwExceptionWhenNotFound: throwExceptionWhenNotFound,
                   errorMessage: errorMessage, () => TotalRaw == position, $"Collection Control failed on to find the total element by {By}",
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
            var currentTotal = TotalRaw;

            if (currentTotal == 0 && position > 1)
                throw new Exception($"The are no UI elements matching and you have requested for {position} to appear");

            if (currentTotal == 0) //If there are no such element in the UI then wait for the first one to appear
            {
                RawElement.WaitGeneric(driver: Driver,
                    waitTimeSec: waitTimeSec,
                    throwExceptionWhenNotFound: throwExceptionWhenNotFound,
                    errorMessage: errorMessage,
                    () => currentTotal == position,
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
            if (TotalRaw <= 0) return;

            var currentTotalItems = TotalRaw;
            RawElement.WaitGeneric(driver: Driver,
                    waitTimeSec: waitTimeSec,
                    throwExceptionWhenNotFound: throwExceptionWhenNotFound,
                    errorMessage: errorMessage,
                    () => currentTotalItems < TotalRaw,
                    $"Collection Control failed on element to go invisible {By}",
                    baseControl: this);
        }
        #endregion
    }
}