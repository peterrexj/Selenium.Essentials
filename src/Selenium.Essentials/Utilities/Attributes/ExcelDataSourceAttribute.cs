using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials
{
    /// <summary>
    /// Excel Payload attribute for nUnit Api and Web tests
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ExcelDataSourceAttribute : Attribute
    {
        /// <summary>
        /// Excel data loaded
        /// </summary>
        public ExcelData ExcelData { get; private set; }

        /// <summary>
        /// Initialize the excel attribute
        /// </summary>
        /// <param name="filename">Relative path to the excel with in the project to load</param>
        /// <param name="worksheet">Worksheet in the excel to load</param>
        /// <param name="key">The column which contains the key (unique data which can be used as key to each row)</param>
        /// <param name="column">The column which contains the value</param>
        public ExcelDataSourceAttribute(string filename, string worksheet, string key, string column)
        {
            ExcelData = new ExcelData(filename, worksheet, key, column, "");
        }
    }
}
