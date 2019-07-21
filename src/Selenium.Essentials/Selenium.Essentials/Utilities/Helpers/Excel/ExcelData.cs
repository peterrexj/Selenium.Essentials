using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FluentAssertions;
using Selenium.Essentials.Utilities.Extensions;
using Selenium.Essentials.Utilities.Helpers;

namespace Selenium.Essentials.Utilities
{
    public class ExcelData
    {
        public string FilePath { get; set; }
        public string WorkSheet { get; set; }
        public string Key { get; set; }
        public string Column { get; set; }


        public string ExcelDataSingleValue { get; private set; }
        public IExcelData ExcelDataRaw { get; private set; }
        public Dictionary<string, string> DataContent { get; private set; }
        public dynamic ExcelDataDynamic { get; private set; }

        public ExcelData(string filePath, string worksheet, string key = "", string column = "", string filter = "")
        {
            FilePath = Path.Combine(Utility.Runtime.ExecutingFolder, filePath);
            WorkSheet = worksheet;
            Key = key;
            Column = column;

            File.Exists(FilePath).Should()
                .BeTrue($"The Excel file trying to load is unavailable in the location {FilePath}");

            var helper = new ExcelHelper(FilePath, WorkSheet, true);

            ExcelDataRaw = helper.LoadDataset();

            if (key.HasValue() && column.HasValue() && filter.IsEmpty())
            {
                DataContent = helper.LoadDatasetBasedOnColumn(keyColumn: key, valueColumn: column);
                ExcelDataDynamic = DynamicDataContainer.GetDynamicObject(DataContent);
            }

            if (key.HasValue() && filter.HasValue())
            {
                DataContent = helper.LoadDatasetBasedOnRow(key, filter);
                ExcelDataDynamic = DynamicDataContainer.GetDynamicObject(DataContent);
            }

            if (key.HasValue() && column.HasValue() && filter.HasValue())
            {
                ExcelDataSingleValue = helper.LoadDataBasedOnColumnRow(key, column, filter);
            }
        }
    }
}
