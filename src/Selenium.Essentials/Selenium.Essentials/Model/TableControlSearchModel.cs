using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials.Model
{
    public class TableControlSearchModel
    {
        public TableControlSearchModel()
        {
            GetTheWholeContentWithInColumn = false;
        }
        public int Column { get; set; }
        public string ColumnName { get; set; }
        public string TextToMatch { get; set; }
        public bool DoExactMatch { get; set; }
        public By SelectorToFindControlInColumn { get; set; }
        public bool GetTheWholeContentWithInColumn { get; set; }

    }
}
