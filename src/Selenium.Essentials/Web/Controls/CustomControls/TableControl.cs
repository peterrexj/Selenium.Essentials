﻿using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using OpenQA.Selenium;
using FluentAssertions;
using Pj.Library;

namespace Selenium.Essentials
{
    public class TableControl : BaseControl
    {
        public bool RenderedInUi = true;
        private int _totalColumns;

        private CollectionControl _bodyRowControls => new CollectionControl(Driver, By.CssSelector("tbody>tr"), parentControl: this);
        private CollectionControl _bodyColumnControls => new CollectionControl(Driver, By.CssSelector("tbody>tr>th"), parentControl: this);
        private CollectionControl _headerColumnControls => new CollectionControl(Driver, By.CssSelector("thead>tr>th"), parentControl: this);

        private IEnumerable<string> _headerColumns => _headerColumnControls.Get();
        private IEnumerable<string> _bodyColumns => _bodyColumnControls.Get();
        private IEnumerable<string> _columnNames;
        public int TotalColumns => ColumnNames.Count();
        public int TotalBodyColumns => _bodyColumnControls.Total;
        public int TotalRows => _bodyRowControls.Total;

        public IEnumerable<string> ColumnNames
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


        public TableControl(
            IWebDriver driver, 
            By selector, 
            BaseControl parentControl = null,
            By loadWaitingSelector = null) : base(driver, selector, parentControl)
        {
            {
                if (WaitUntilElementVisible(waitTimeSec: 2, throwExceptionWhenNotFound: false))
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
            RawElement.WaitUntilElementVisible(Driver);
            _loadingSpinner.WaitClickTillElementGoesInvisible();

            if (expectingRows)
            {
                GetBodyControl<WebControl>(1, 1, getContentWithinTDelement: true).WaitUntilElementVisible(errorMessage: "Table expecting atleast one row to be rendered");
            }
        }
    }
}
