using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials
{
    public class TableControlSearchModel
    {
        public int Column { get; set; }
        public string ColumnName { get; set; }
        public string TextToMatch { get; set; }
        public bool DoExactMatch { get; set; }
        public string XpathToInnerControlToSearch { get; set; }

        public override string ToString()
        {
            return $"[Column]: {Column}, [ColumnName]: {ColumnName}, [TextToMatch]: {TextToMatch}, [DoExactMatch]: {DoExactMatch}, [Selector To Find Control In Column]: {XpathToInnerControlToSearch}";
        }
    }
}
