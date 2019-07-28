using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials
{
    public interface IExcelData
    {
        /// <summary>
        /// Represents the sheet present in the excel work book
        /// All the data will be inside the worksheet
        /// </summary>
        ExcelSheets Sheets { get; }

        /// <summary>
        /// Returns the first worksheet from the collection of worksheet
        /// Most of use case will be to load the first sheet and use that
        /// </summary>
        IExcelSheet FirstSheet { get; }

        /// <summary>
        /// Returns the names of the worksheet present in the excel file
        /// </summary>
        IList<string> WorksheetNames { get; }
        //TODO.Need to implement these functions
        //IEnumerable<dynamic> ApplyFilter(string columnName, string filterValue);
        //dynamic ApplyFilterAndGetFirstRecord(string columnName, string filterValue);

        void UpdateCell(string worksheet, int column, int row, string value);
        void UpdateCell(string worksheet, string column, int row, string value);

        void AutoFitColumns(string worksheet);

        void SaveWorkBook(string path);
    }
}
