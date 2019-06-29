using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using OpenQA.Selenium;
using Selenium.Essentials.Utilities.Extensions;
using FluentAssertions;
using Selenium.Essentials.Model;

namespace Selenium.Essentials.Web.Controls.Controls
{
    public class TableControl : BaseControl
    {
        public bool RenderedInUi = true;
        private int _totalColumns;

        private CollectionControl _bodyRowControls => new CollectionControl(Driver, By.CssSelector("tbody>tr"), parentControl: this);
        private CollectionControl _bodyColumnControls => new CollectionControl(Driver, By.CssSelector("tbody>tr>th"), parentControl: this);
        private CollectionControl _headerColumnControls => new CollectionControl(Driver, By.CssSelector("thead>tr>th"), parentControl: this);

        public int TotalRows => _bodyRowControls.Total;

        private IList<string> _headerColumns => _headerColumnControls.Get();
        private IList<string> _bodyColumns => _bodyColumnControls.Get();
        private IList<string> _columnNames;

        public int TotalColumns => ColumnNames.Count;
        public int TotalBodyColumns => _bodyColumnControls.Total;

        /// <summary>
        /// Cache contents of the table column. In case of dynamic column generation, this caching should be removed or create a new instance of the table to read the column headers
        /// </summary>
        public IList<string> ColumnNames
        {
            get
            {
                if (_columnNames == null || !_columnNames.Any())
                {
                    _columnNames = _headerColumns.Any() ? _headerColumns : _headerColumns.Union(_bodyColumns).ToArray();
                }
                return _columnNames;
            }
        }

        public int GetColumnPosition(string columnname)
        {
            ColumnNames.ContainsIgnoreCase(columnname).Should().BeTrue($"The column name specified [{columnname}] does not exist in the Grid");

            return RawElement.FindElements(By.CssSelector("thead>tr>th")).Union(RawElement.FindElements(By.CssSelector("tbody>tr>th")))
                .Select(x => x.Text)
                .Select((name, index) => new { Name = name, Index = index })
                .Where(x => x.Name.Contains(columnname, StringComparison.CurrentCultureIgnoreCase))
                .Select(x => x.Index)
                .FirstOrDefault() + 1;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="selector">Selector to the table</param>
        /// <param name="totalColumns">Total columns expected in the table</param>
        public TableControl(
            IWebDriver driver, 
            By selector, 
            BaseControl parentControl = null,
            By loadWaitingSelector = null) : base(driver, selector, parentControl)
        {
            {
                if (WaitForElementVisible(waitTimeSec: 2, throwExceptionWhenNotFound: false))
                {
                    _totalColumns = RawElement.FindElements(By.CssSelector("thead>tr>th")).Count;
                }
                else
                {
                    RenderedInUi = false;
                }
            }

            LoadWaitingSelector = loadWaitingSelector;
        }

        public T GetHeaderControl<T>(int column, int row = 1, By selector = null, bool getContentWithinTDelement = false) where T : BaseControl
        {
            if (selector != null)
            {
                return ControlFactory.CreateNew<T>(Driver, selector,
                    new WebControl(Driver,
                    By.CssSelector($"thead tr:nth-child({row}) th:nth-child({column})"), this));
            }
            else
            {
                return ControlFactory.CreateNew<T>(Driver,
                    By.CssSelector($"thead tr:nth-child({row}) th:nth-child({column})"), this);
            }
        }

        public T GetHeaderControl<T>(string column, int row = 1, By selector = null, bool getContentWithinTDelement = false) where T : BaseControl
        {
            return GetHeaderControl<T>(GetColumnPosition(column), row, selector);
        }

        /// <summary>
        /// Creates a control with in the cell based on the selector. 
        /// If the selector is empty, it will try to use the selector set while initialization
        /// If getContentWithinTDelement is true, then it will create element directly under TD, example <td>Hello</td>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <param name="selector"></param>
        /// <param name="getContentWithinTDelement"></param>
        /// <returns></returns>
        public T GetBodyControl<T>(int column, int row, By selector = null, bool getContentWithinTDelement = false) where T : BaseControl
        {
            if (selector != null)
            {
                return ControlFactory.CreateNew<T>(Driver, selector,
                    new WebControl(Driver, By.CssSelector($"tbody tr:nth-child({row}) td:nth-child({column})"), this));
            }
            else
            {
                return ControlFactory.CreateNew<T>(Driver,
                    By.CssSelector($"tbody tr:nth-child({row}) td:nth-child({column})"), this);
            }
        }

        public T GetBodyControl<T>(string column, int row, By selector = null, bool getContentWithinTDelement = false) where T : BaseControl
        {
            return GetBodyControl<T>(GetColumnPosition(column), row, selector, getContentWithinTDelement);
        }

        public T GetBodyControl<T>(TableControlSearchModel model, int row) where T : BaseControl
        {
            if (model.ColumnName.HasValue())
            {
                model.Column = GetColumnPosition(model.ColumnName);
            }

            return GetBodyControl<T>(model.Column, row, model.SelectorToFindControlInColumn, model.GetTheWholeContentWithInColumn);
        }

        public T GetFooterControl<T>(int column, int row, By selector = null, bool getContentWithinTDelement = false) where T : BaseControl
        {
            if (selector != null)
            {
                return ControlFactory.CreateNew<T>(Driver, selector,
                    new WebControl(Driver, By.CssSelector($"tfoot tr:nth-child({row}) td:nth-child({column})"), this));
            }
            else
            {
                return ControlFactory.CreateNew<T>(Driver,
                    By.CssSelector($"tfoot tr:nth-child({row}) td:nth-child({column})"), this);
            }
        }

        public T GetFooterControl<T>(string column, int row, By selector = null, bool getContentWithinTDelement = false) where T : BaseControl
        {
            return GetFooterControl<T>(GetColumnPosition(column), row, selector);
        }

        public int GetRowPosition(string column, string textToFind, bool exactMatch, By selector = null, bool getContentWithinTDelement = false)
        {
            return GetRowPosition(new List<TableControlSearchModel> { new TableControlSearchModel
            {
                ColumnName = column,
                TextToMatch = textToFind,
                DoExactMatch =exactMatch,
                SelectorToFindControlInColumn = selector,
                GetTheWholeContentWithInColumn = getContentWithinTDelement
            } });
        }
        public int GetRowPosition(int column, string textToFind, bool exactMatch, By selector = null, bool getContentWithinTDelement = false)
        {
            return GetRowPosition(new List<TableControlSearchModel> { new TableControlSearchModel
            {
                Column = column,
                TextToMatch = textToFind,
                DoExactMatch =exactMatch,
                SelectorToFindControlInColumn = selector,
                GetTheWholeContentWithInColumn = getContentWithinTDelement
            } });
        }

        public int GetRowPosition(List<TableControlSearchModel> model)
        {
            for (int i = 1; i <= TotalRows; i++)
            {
                var match = (from m in model
                             where m.DoExactMatch ? GetBodyControl<WebControl>(m, i).Text.EqualsIgnoreCase(m.TextToMatch) : GetBodyControl<WebControl>(m, i).Text.Contains(m.TextToMatch)
                             select m).Count() == model.Count;

                if (match)
                {
                    return i;
                }
            }
            return -1;
        }

        public List<string> GetColumnValues(string column, By selector = null, bool getContentWithinTDelement = false)
        {
            return GetColumnValues(GetColumnPosition(column), selector, getContentWithinTDelement);
        }
        public List<string> GetColumnValues(int column, By selector = null, bool getContentWithinTDelement = false)
        {
            var result = new List<string>();

            for (int i = 1; i <= TotalRows; i++)
            {
                result.Add(GetBodyControl<WebControl>(column, i, selector, getContentWithinTDelement).Text);
            }

            return result;
        }

        private WebControl _loadingSpinner => new WebControl(Driver, LoadWaitingSelector, this);

        public By LoadWaitingSelector { get; private set; }

        public void WaitForTableLoadComplete(bool expectingRows = false)
        {
            RawElement.WaitForElementVisible(Driver);
            _loadingSpinner.WaitClickTillElementGoesInvisible();

            if (expectingRows)
            {
                GetBodyControl<WebControl>(1, 1, getContentWithinTDelement: true).WaitForElementVisible(errorMessage: "Table expecting atleast one row to be rendered");
            }

        }

    }
}
