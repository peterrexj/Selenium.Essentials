using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials.Utilities
{
    public interface IExcelSheet
    {
        /// <summary>
        /// Represents rows in the excel with the column name starts with "_" and to all lower case and removes all spaces and special characters
        /// For example: "First Name"  will be "_firstname"
        /// </summary>
        IEnumerable<dynamic> Rows { get; }

        /// <summary>
        /// Returns total number of rows present in the excel
        /// </summary>
        int TotalRows { get; }

        /// <summary>
        /// Returns total number of columns present in the excel
        /// </summary>
        int TotalColumns { get; }

        /// <summary>
        /// Return the property name for the class used as a reference to the column name
        /// </summary>
        IList<string> ColumnsAsPropertyName { get; }

        /// <summary>
        /// Returns the available column name in the excel
        /// </summary>
        IList<string> ColumnNames { get; }

        IDictionary<string, string> ColumnMapping { get; }

        string GetMappedColumn(string formattedColumn);

        /// <summary>
        /// Gets the cell value based on the values passed. The parameters accept row and column name.
        /// Which will return column value in the row passed in.
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        string GetValue(string columnName, dynamic row);

        /// <summary>
        /// Gets the cell value as per column and row specified
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        string GetCellValue(int column, int row);
    }
}
