using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ExcelDataSourceAttribute : Attribute
    {
        public ExcelData ExcelData { get; private set; }
        public ExcelDataSourceAttribute(string filename, string worksheet, string key, string column)
        {
            ExcelData = new ExcelData(filename, worksheet, key, column, "");
        }
    }
}
