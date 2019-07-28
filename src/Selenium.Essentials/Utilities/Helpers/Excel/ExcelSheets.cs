using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials.Utilities
{
    public class ExcelSheets
    {
        private readonly IDictionary<string, IExcelSheet> _sheets = new Dictionary<string, IExcelSheet>();

        public IExcelSheet Get(string key)
        {
            return _sheets[key];
        }

        public void Set(string key, IExcelSheet value)
        {
            if (_sheets.ContainsKey(key))
            {
                _sheets[key] = value;
            }
            else
            {
                _sheets.Add(key, value);
            }
        }

        public IExcelSheet this[string key]
        {
            get { return Get(key); }

            set { Set(key, value); }
        }
    }
}
