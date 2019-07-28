using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentAssertions;

namespace Selenium.Essentials
{
    public class ExcelOperationAspose : IExcelData, IDisposable
    {
        Workbook _xBook;
        private ExcelSheets _sheets;
        List<string> _worksheetNames;

        public ExcelOperationAspose(string excelFile, int columnHeaderRow = 0, int readNumberOfRows = 0, string worksheet = "", bool loadAllSheets = false)
        {
            File.Exists(excelFile).Should()
                .BeTrue($"The file requested for excel read is not found in the file system. File path {excelFile}");

            _xBook = new Workbook(excelFile);
            _sheets = new ExcelSheets();

            if (loadAllSheets)
            {
                foreach (Worksheet sheet in _xBook.Worksheets)
                {
                    _sheets.Set(sheet.Name, new ExcelOperationSheetAspose(sheet, columnHeaderRow, readNumberOfRows));
                }
            }
            else
            {
                if (string.IsNullOrEmpty(worksheet))
                {
                    if (!WorksheetNames.Any())
                        throw new Exception("There is no worksheet to load");

                    _sheets.Set(_xBook.Worksheets[0].Name, new ExcelOperationSheetAspose(_xBook.Worksheets[0], columnHeaderRow, readNumberOfRows));
                }
                else
                {
                    if (!WorksheetNames.Contains(worksheet))
                        throw new Exception("The worksheet specified does not exist in the excel file");

                    _sheets.Set(worksheet, new ExcelOperationSheetAspose(_xBook.Worksheets[worksheet], columnHeaderRow, readNumberOfRows));
                }
            }

        }

        public IList<string> WorksheetNames
        {
            get
            {
                _xBook.Should().NotBeNull("Excel data not loaded");

                if (_worksheetNames == null)
                {
                    _worksheetNames = new List<string>();
                    foreach (Worksheet sheet in _xBook.Worksheets)
                    {
                        _worksheetNames.Add(sheet.Name);
                    }
                }

                return _worksheetNames;
            }
        }

        public ExcelSheets Sheets => _sheets;

        public IExcelSheet FirstSheet => _sheets[_xBook.Worksheets[0].Name];

        public void UpdateCell(string worksheet, int column, int row, string value)
        {
            _xBook.Worksheets[worksheet].Cells[row, column].Value = value;
        }

        public void UpdateCell(string worksheet, string column, int row, string value)
        {
            var colIndex = CellsHelper.ColumnNameToIndex(column);
            _xBook.Worksheets[worksheet].Cells[row, colIndex].Value = value;
        }
        public void SaveWorkBook(string path)
        {
            StorageHelper.CreateDirectory(path);
            _xBook.Save(path);
        }

        public void Dispose()
        {
            _xBook = null;
            _sheets = null;
        }

        public void AutoFitColumns(string worksheet)
        {
            var totalColumns = _sheets[worksheet].TotalColumns;
            var totalRows = _sheets[worksheet].TotalRows;
            _xBook.Worksheets[worksheet].AutoFitColumns(0, 0, totalRows, totalColumns);
        }
    }
}
