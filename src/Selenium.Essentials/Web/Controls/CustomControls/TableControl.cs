using OpenQA.Selenium;
using Pj.Library;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Selenium.Essentials
{
    public class TableControl : BaseControl
    {
        /// <summary>
        /// Cache to hold the column name and its position in the table
        /// </summary>
        private Dictionary<string, int> _cacheColumnNamePositionMapping;
        private WebControl _loadingSpinner => new(Driver, LoadWaitingSelector, this);

        /// <summary>
        /// Selector to wait for the table to load
        /// </summary>
        public By LoadWaitingSelector { get; private set; }

        public TableControl(IWebDriver driver, By selector, BaseControl parentControl = null, By loadWaitingSelector = null,
            string description = "", bool firstAvailable = false)
            : base(driver, selector, parentControl, description, firstAvailable)
        {
            LoadWaitingSelector = loadWaitingSelector;
            _cacheColumnNamePositionMapping = new Dictionary<string, int>();
        }

        #region Wait

        /// <summary>
        /// The WaitForTableLoadComplete method is used to wait until a table is fully loaded on a webpage. It takes an optional boolean parameter expectingRows which defaults to false.
        ///If expectingRows is set to true, the method will also wait until at least one row is visible in the table.This is useful in scenarios where the table is expected to have data and the test should wait until the data is loaded.
        ///The method first waits for the raw element of the table to become visible.If a LoadWaitingSelector is provided (which is a selector for a loading spinner or similar element), it waits until this element becomes invisible, indicating that loading has completed.
        ///If expectingRows is true, it also waits until at least one row in the table is visible.If no row becomes visible, it throws an error with a message indicating that at least one row was expected.
        /// </summary>
        /// <param name="expectingRows"></param>
        public void WaitForTableLoadComplete(bool expectingRows = false)
        {
            RawElement.WaitUntilElementVisible(Driver);
            if (LoadWaitingSelector != null)
            {

                _loadingSpinner.WaitUntilElementInvisible();
            }

            if (expectingRows)
            {
                GetBodyControl<WebControl>(1, 1).WaitUntilElementVisible(errorMessage: $"Table [{XpathSelector}] expecting atleast one row to be rendered");
            }
        }

        #endregion

        #region Identifiers
        private CollectionControl _bodyColumnValues(int columnPosition, string innerXpath) =>
           new(Driver, By.XPath($"{XpathSelector}//tbody//tr//td[position() = {columnPosition}]{(innerXpath.HasValue() ? innerXpath : "")}"));
        private CollectionControl _bodyRows => new(Driver, By.CssSelector("tbody>tr"), parentControl: this);
        private CollectionControl _bodyColumns => new(Driver, By.CssSelector("tbody>tr>th"), parentControl: this);
        #endregion

        #region Table Properties

        /// <summary>
        /// Gets the number of rows in the table
        /// </summary>
        public int RowCount => _bodyRows.TotalRaw;

        /// <summary>
        /// Get the number of columns in the table body
        /// </summary>
        public int ColumnCount => _bodyColumns.TotalRaw;

        /// <summary>
        /// Get the number of columns based on the colum header
        /// </summary>
        public int ColumnCountByColumnHeaders => ColumnNames.Count();

        /// <summary>
        /// Gets the number of columns in the table      
        /// <param name="row">Row (tr) position. Starts from 1</param>
        /// </summary>
        public int ColumnCountByRow(int row) => ParentControl.RawElement.FindElements(By.XPath($".//tbody/tr[position() = {row}]/td")).Count;

        private IEnumerable<string> _columnNames;
        /// <summary>
        /// Get the column names from both the table header and body header
        /// </summary>
        public IEnumerable<string> ColumnNames
        {
            get
            {
                if (_columnNames == null || !_columnNames.Any())
                {
                    if (_cacheColumnNamePositionMapping == null || _cacheColumnNamePositionMapping?.IsEmpty() == true)
                    {
                        _cacheColumnNamePositionMapping = new Dictionary<string, int>();
                        RawElement.FindElements(By.CssSelector("thead>tr>th")).Union(RawElement.FindElements(By.CssSelector("tbody>tr>th")))
                            .Select(x => x.Text)
                            .Select((name, index) => new { Name = name, Index = index })
                            .Iter(x => _cacheColumnNamePositionMapping.AddOrUpdate(x.Name, x.Index + 1));
                        _columnNames = _cacheColumnNamePositionMapping.Keys;
                    }
                    else { _columnNames = _cacheColumnNamePositionMapping.Keys; }
                }
                return _columnNames;
            }
        }

        #endregion

        #region Cell, Column & Row Items

        /// <summary>
        /// Gets the table item at the specified row and column
        /// </summary>
        /// <param name="row">Row (tr) position. Starts from 1</param>
        /// <param name="column">Column (td) position. Starts from 1</param>
        public T Item<T>(int row, int column) where T : BaseControl
            => ControlFactory.CreateNew<T>(Driver, By.XPath($".//tbody/tr[position() = {row}]/td[position() = {column}]"), parentControl: this);

        /// <summary>
        /// Gets the table item at the specified row and column
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public T Item<T>(int row, string columnName) where T : BaseControl
            => Item<T>(row, GetColumnPosition(columnName));

        /// <summary>
        /// Returns item matching the given selector in the provided table row and column
        /// </summary>
        /// <typeparam name="T">Type of control to return</typeparam>
        /// <param name="row">Row (tr) position. Starts from 1</param>
        /// <param name="column">Column (td) position. Starts from 1</param>
        /// <param name="by">Query selector</param>
        /// <returns></returns>
        public T Item<T>(int row, int column, By by) where T : BaseControl
            => ControlFactory.CreateNew<T>(Driver, by, Item<T>(row, column));

        /// <summary>
        /// Returns item matching the given selector in the provided table row and column
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <param name="columnName"></param>
        /// <param name="by"></param>
        /// <returns></returns>
        public T Item<T>(int row, string columnName, By by) where T : BaseControl
            => ControlFactory.CreateNew<T>(Driver, by, Item<T>(row, columnName));

        /// <summary>
        /// Gets the control of type WebControl at the given row and column in the table
        /// </summary>
        /// <param name="row">Row (tr) position. Starts from 1</param>
        /// <param name="column">Row (tr) position. Starts from 1</param>
        /// <returns></returns>
        public WebControl Item(int row, int column) => Item<WebControl>(row, column);

        /// <summary>
        /// Gets the control of type WebControl at the given row and column in the table
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public WebControl Item(int row, string columnName) => Item<WebControl>(row, columnName);

        /// <summary>
        /// Gets the control of type WebControl using the given selector at the provided
        /// table row and column
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="by"></param>
        /// <returns></returns>
        public WebControl Item(int row, int column, By by) => Item<WebControl>(row, column, by);

        /// <summary>
        /// Gets the control of type WebControl using the given selector at the provided table row and column
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnName"></param>
        /// <param name="by"></param>
        /// <returns></returns>
        public WebControl Item(int row, string columnName, By by) => Item<WebControl>(row, columnName, by);

        /// <summary>
        /// Gets the row at the provided row position in the table
        /// </summary>
        /// <typeparam name="T">Type of control to return</typeparam>
        /// <param name="row">Row (tr) position. Starts from 1</param>
        /// <returns></returns>
        public T Row<T>(int row) where T : BaseControl
        {
            return ControlFactory.CreateNew<T>(Driver, By.XPath($".//tbody/tr[{row}]"), ParentControl);
        }

        /// <summary>
        /// Returns the row with type WebControl at the provided position in the table
        /// </summary>
        /// <param name="row">Row (tr) position. Starts from 1</param>
        /// <returns></returns>
        public WebControl Row(int row) => Row(row);

        /// <summary>
        /// Get all the values in a column
        /// </summary>
        /// <param name="column">column header name</param>
        /// <param name="selector">selector within the table cell</param>
        /// <returns></returns>
        public List<string> GetColumnValues(string column, string innerXpath = "") => GetColumnValues(GetColumnPosition(column), innerXpath);

        /// <summary>
        /// Get all values in a column
        /// </summary>
        /// <param name="column"></param>
        /// <param name="selector"></param>
        /// <param name="quickMode"></param>
        /// <returns></returns>
        public List<string> GetColumnValues(int column, string innerXpath = "")
        {
            var result = new List<string>();

            _bodyColumnValues(column, innerXpath).Get().Iter(x => result.Add(x));

            return result;
        }

        #endregion

        #region Operations

        /// <summary>
        /// Clicks the element at the given row and column in the table
        /// </summary>
        /// <param name="row">Row (tr) position. Starts from 1</param>
        /// <param name="column">Column (td) position. Starts from 1</param>
        public void Click(int row, int column) => Item(row, column).Click();

        /// <summary>
        /// Clicks the element at the given row and column in the table
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnName"></param>
        public void Click(int row, string columnName) => Item(row, columnName).Click();

        /// <summary>
        /// Click element using given selector in the provided row and column
        /// </summary>
        /// <param name="row">Row (tr) position. Starts from 1</param>
        /// <param name="column">Column (td) position. Starts from 1</param>
        /// <param name="by">selector</param>
        public void Click(int row, int column, By by) => Item<WebControl>(row, column, by).Click();

        /// <summary>
        /// Click element using given selector in the provided row and column based on the column name in the table
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnName"></param>
        /// <param name="by"></param>
        public void Click(int row, string columnName, By by) => Item<WebControl>(row, columnName, by).Click();

        /// <summary>
        /// Click element in table based on displayed text
        /// </summary>
        /// <param name="text">The text of the element to be clicked on</param>
        public void ClickByText(string text)
        {
            ParentControl.RawElement.FindElement(By.XPath($".//*[normalize-space(text()) = '{text}']")).Click();
        }

        /// <summary>
        /// 
        /// </summary>        
        /// <param name="text">Default value is false. If true a partial match on the table items will be performed, 
        /// otherwise exact match
        /// </param>
        public void ClickByTextPartial(string text)
        {
            ParentControl.RawElement.FindElement(By.XPath($"(.//*[contains(normalize-space(.), '{text}')])[last()]")).Click();
        }

        /// <summary>
        /// Set value to the first input element at the given row and column in the table
        /// </summary>
        /// <param name="row">Row (tr) position. Starts from 1</param>
        /// <param name="column">Column (td) position. Starts from 1</param>
        /// <param name="value">Value that needs to be set to the element</param>
        public void Set(int row, int column, string value) => Item<TextboxControl>(row, column, By.TagName("input")).Set(value);

        /// <summary>
        /// Set value to the first input element at the given row and column in the table based on the column name
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        public void Set(int row, string columnName, string value) => Item<TextboxControl>(row, columnName, By.TagName("input")).Set(value);

        /// <summary>
        /// Set value to element using the By selector at the given row and column in the table
        /// </summary>
        /// <param name="row">Row (tr) position. Starts from 1</param>
        /// <param name="column">Column (td) position. Starts from 1</param>
        /// <param name="value">Value that needs to be set to the element</param>
        /// <param name="by">Query selector</param>
        public void Set(int row, int column, string value, By by) => Item<TextboxControl>(row, column, by).Set(value);

        /// <summary>
        /// Set value to element using the By selector at the given row and column in the table
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <param name="by"></param>
        public void Set(int row, string columnName, string value, By by) => Item<TextboxControl>(row, columnName, by).Set(value);

        #endregion

        #region Column Position

        /// <summary>
        /// Get column position based on the column name
        /// </summary>
        /// <param name="columnname"></param>
        /// <returns></returns>
        public int GetColumnPosition(string columnname)
        {
            if (_cacheColumnNamePositionMapping != null && _cacheColumnNamePositionMapping.ContainsKey(columnname))
            {
                return _cacheColumnNamePositionMapping[columnname];
            }

            //Call this if no cache value available
            if (ColumnNames.ContainsIgnoreCase(columnname) == false)
                throw new Exception($"The column name specified [{columnname}] does not exist in the Grid");

            if (_cacheColumnNamePositionMapping != null && _cacheColumnNamePositionMapping.ContainsKey(columnname))
            {
                return _cacheColumnNamePositionMapping[columnname];
            }
            else
            {
                return RawElement.FindElements(By.CssSelector("thead>tr>th")).Union(RawElement.FindElements(By.CssSelector("tbody>tr>th")))
                    .Select(x => x.Text)
                    .Select((name, index) => new { Name = name, Index = index })
                    .Where(x => x.Name.Contains(columnname, StringComparison.CurrentCultureIgnoreCase))
                    .Select(x => x.Index)
                    .FirstOrDefault() + 1;
            }
        }

        #endregion

        #region Row Position

        /// <summary>
        /// Get the row position in the table based on the search model. The model can have one or more filter
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int GetRowPosition(TableControlSearchModel model)
        {
            var rowCount = RowCount; //Get row count and cache it for the current execution. Any rows during this loop will not be considered in the processing

            model.Column = model.ColumnName.HasValue() ? GetColumnPosition(model.ColumnName) : model.Column;

            var values = GetColumnValues(model.Column, model.XpathToInnerControlToSearch);

            int index = -1;
            if (model.DoExactMatch)
            {
                index = values.IndexOf(model.TextToMatch) + 1;
            }
            else
            {
                index = values.FindIndex(x => x.Contains(model.TextToMatch)) + 1;
            }
            return index;
        }

        /// <summary>
        /// Return the row position based on the filter
        /// </summary>
        /// <param name="column">Column header name</param>
        /// <param name="textToFind">Filter text to match</param>
        /// <param name="exactMatch">Do a exact match or partial match</param>
        /// <param name="selector">how to find element within the table cell</param>
        /// <returns>the position where the filter match within the table</returns>
        public int GetRowPosition(string column, string textToFind, bool exactMatch, string xpathToInnerControl = "")
        {
            return GetRowPosition(new TableControlSearchModel
            {
                ColumnName = column,
                TextToMatch = textToFind,
                DoExactMatch = exactMatch,
                XpathToInnerControlToSearch = xpathToInnerControl,
            });
        }

        /// <summary>
        /// Return the row position based on the filter
        /// </summary>
        /// <param name="column">Column position</param>
        /// <param name="textToFind">Filter text to match</param>
        /// <param name="exactMatch">Do a exact match or partial match</param>
        /// <param name="selector">how to find element within the table cell</param>
        /// <returns>the position where the filter match within the table</returns>
        /// <returns></returns>
        public int GetRowPosition(int column, string textToFind, bool exactMatch, string xpathToInnerControl = "")
        {
            return GetRowPosition(new TableControlSearchModel
            {
                Column = column,
                TextToMatch = textToFind,
                DoExactMatch = exactMatch,
                XpathToInnerControlToSearch = xpathToInnerControl,
            });
        }

        #endregion

        #region Get Control

        /// <summary>
        /// Get the table control (cell element)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <param name="columnName"></param>
        /// <param name="xpathToInnerControl"></param>
        /// <param name="rowType"></param>
        /// <param name="headerType"></param>
        /// <returns></returns>
        private T GetTableControl<T>(string where, int column = 1, int row = 1,
            string columnName = "",
            string xpathToInnerControl = "",
            string headerType = "td") where T : BaseControl
        {
            if (columnName.HasValue())
            {
                column = GetColumnPosition(columnName);
            }
            return ControlFactory.CreateNew<T>(Driver, By.XPath($"{XpathSelector}//{where}//tr[position() = {row}]//{headerType}[position() = {column}]{(xpathToInnerControl.HasValue() ? xpathToInnerControl : "")}"));
        }

        /// <summary>
        /// Get Header Control based on the column and row and based on the selector
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public T GetHeaderControl<T>(int column, int row = 1, string xpathToInnerControl = null) where T : BaseControl
            => GetTableControl<T>("thead", column, row: row, xpathToInnerControl: xpathToInnerControl, headerType: "th");

        /// <summary>
        /// Get Header Control based on the column name and row and based on the selector
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public T GetHeaderControl<T>(string column, int row = 1, string xpathToInnerControl = null) where T : BaseControl
            => GetTableControl<T>("thead", columnName: column, row: row, xpathToInnerControl: xpathToInnerControl, headerType: "th");

        /// <summary>
        /// Get Body Control based on the column and row and based on the selector
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public T GetBodyControl<T>(int column, int row, string xpathToInnerControl = null) where T : BaseControl
            => GetTableControl<T>("tbody", column, row: row, xpathToInnerControl: xpathToInnerControl);

        /// <summary>
        /// Get Body Control based on the column name and row and based on the selector
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public T GetBodyControl<T>(string column, int row, string xpathToInnerControl = null) where T : BaseControl
            => GetTableControl<T>("tbody", columnName: column, row: row, xpathToInnerControl: xpathToInnerControl);

        /// <summary>
        /// Get body control based on the search model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public T GetBodyControl<T>(TableControlSearchModel model, int row) where T : BaseControl
        {
            if (model.ColumnName.HasValue())
            {
                model.Column = GetColumnPosition(model.ColumnName);
            }

            return GetTableControl<T>("tbody", model.Column, row: row, xpathToInnerControl: model.XpathToInnerControlToSearch);
        }

        /// <summary>
        /// Get the footer control based on the position provided (i,j)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public T GetFooterControl<T>(int column, int row, string xpathToInnerControl = null) where T : BaseControl
            => GetTableControl<T>("tfoot", column, row: row, xpathToInnerControl: xpathToInnerControl);

        /// <summary>
        /// Get the footer control based on the column name and row position
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public T GetFooterControl<T>(string column, int row, string xpathToInnerControl = null) where T : BaseControl
            => GetTableControl<T>("tfoot", columnName: column, row: row, xpathToInnerControl: xpathToInnerControl);

        #endregion
    }
}
