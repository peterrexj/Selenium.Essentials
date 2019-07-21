using Selenium.Essentials.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class PayloadDataExcelAttribute : Attribute
    {
        public ExcelData ExcelData { get; private set; }
        public PayloadDataExcelAttribute(string filename, string worksheet, string key, string column)
        {
            ExcelData = new ExcelData(filename, worksheet, key, column, "");
        }
    }
}
