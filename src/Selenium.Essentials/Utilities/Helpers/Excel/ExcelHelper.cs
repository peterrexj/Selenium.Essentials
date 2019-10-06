using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Selenium.Essentials
{
    internal class ExcelHelper : IDisposable
    {
        static ExcelHelper()
        {
        }

        public ExcelHelper(string filePath, string worksheet = "", bool cache = false)
        {
            FilePath = filePath;
            Worksheet = worksheet;
            Cache = cache;
        }

        internal string FilePath { get; set; }
        internal string Worksheet { get; set; }
        internal bool Cache { get; set; }

        static ConcurrentDictionary<string, IExcelData> CacheData = new ConcurrentDictionary<string, IExcelData>();

        internal IExcelData LoadDataset(int columnHeaderRow = 0, int numberOfRowsToRead = 0, bool loadAllSheets = false)
        {
            if (Cache)
            {
                if (!CacheData.ContainsKey(FilePath))
                {
                    Utility.Runtime.Logger.Log($"Loading the excel data into memory: {FilePath}");
                    CacheData.TryAdd(FilePath, new ExcelOperationAspose(FilePath, columnHeaderRow, numberOfRowsToRead, Worksheet, loadAllSheets));
                }
                return CacheData[FilePath];
            }
            else
            {
                return new ExcelOperationAspose(FilePath, columnHeaderRow, numberOfRowsToRead, Worksheet, loadAllSheets);
            }
        }

        internal Dictionary<string, string> LoadDatasetBasedOnColumn(string keyColumn, string valueColumn)
        {
            var sheetData = LoadDataset().Sheets[Worksheet];
            return (from row in sheetData.Rows
                    select new { name = sheetData.GetValue(keyColumn, row), value = sheetData.GetValue(valueColumn, row) })
                    .ToDictionary<dynamic, string, string>(r => r.name, r => r.value);
        }

        internal Dictionary<string, string> LoadDatasetBasedOnRow(string keyColumn, string filter)
        {
            var compileDictionary = new Dictionary<string, string>();
            var sheetData = LoadDataset().Sheets[Worksheet];

            var filteredData = sheetData.Rows
                .FirstOrDefault(r => ((string) sheetData.GetValue(keyColumn, r)).EqualsIgnoreCase(filter));

            foreach (var data in filteredData)
            {
                compileDictionary.Add(sheetData.GetMappedColumn(data.Key), data.Value);
            }

            return compileDictionary;
        }

        internal string LoadDataBasedOnColumnRow(string keyColumn, string valueColumn, string filter)
        {
            var sheetData = LoadDataset().Sheets[Worksheet];

            var filteredData =
                (from row in sheetData.Rows
                    where ((string) sheetData.GetValue(keyColumn, row)).EqualsIgnoreCase(filter)
                    select sheetData.GetValue(valueColumn, row)).FirstOrDefault();
            return filteredData;
        }

        public void Dispose()
        {
            CacheData.Clear();
            CacheData = null;
        }
    }
}
