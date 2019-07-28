using Aspose.Cells;
using Selenium.Essentials.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Selenium.Essentials.Utilities
{
    public class ExcelOperationSheetAspose : ExcelDataRaw, IExcelSheet
    {
        private readonly int _columnHeaderRow;
        private readonly Worksheet _xSheet;
        private int _readRecords;
        private IDictionary<int, string> _columnNames;
        private IDictionary<int, string> _columnMapping;

        public ExcelOperationSheetAspose(Worksheet xSheet, int columnHeaderRow = 0, int readNumberOfRows = 0)
        {
            _xSheet = xSheet;
            _columnHeaderRow = columnHeaderRow != 0 ? columnHeaderRow - 1 : columnHeaderRow;
            _readRecords = readNumberOfRows;

            GenerateData();
        }

        public int TotalRows => _xSheet.Cells.MaxDataRow;
        public int TotalColumns => _xSheet.Cells.MaxDataColumn;
        public IList<string> ColumnsAsPropertyName => _columnNames.Select(c => c.Value).ToList();
        public IList<string> ColumnNames => _columnMapping.Select(c => c.Value).ToList();
        public IDictionary<string, string> ColumnMapping => ColumnNames.Zip(ColumnsAsPropertyName, (k, v) => new { k, v }).ToDictionary(x => x.k, x => x.v);
        public IEnumerable<dynamic> Rows => _data;

        private void GenerateData()
        {
            InitializeColumnNames();

            var rangeCells = _xSheet.Cells;

            _readRecords = _readRecords == 0 ? TotalRows : _readRecords + _columnHeaderRow;

            var cts = new CancellationTokenSource();
            var pOptions = new ParallelOptions
            {
                CancellationToken = cts.Token,
                MaxDegreeOfParallelism = 1 //System.Environment.ProcessorCount
            };
            try
            {
                Parallel.For(_columnHeaderRow + 1, _readRecords + 1, pOptions, i =>
                {
                    var obj = new ExpandoObject() as IDictionary<string, object>;
                    for (var j = 0; j <= TotalColumns; j++)
                    {
                        obj.Add(_columnNames[j], rangeCells[i, j].StringValue);
                    }
                    _data.Add(obj);
                    pOptions.CancellationToken.ThrowIfCancellationRequested();
                });
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                cts.Dispose();
            }
        }

        private void InitializeColumnNames()
        {
            var columnNameRow = _xSheet.Cells.Rows[_columnHeaderRow];

            _columnNames = new Dictionary<int, string>();
            _columnMapping = new Dictionary<int, string>();

            for (var j = 0; j <= TotalColumns; j++)
            {
                _columnMapping.Add(j, columnNameRow[j].StringValue);
                _columnNames.Add(j, columnNameRow[j].StringValue.ConvertToDbFormatColumnName());
            }
        }

        public string GetMappedColumn(string formattedColumn)
            => ColumnMapping.FirstOrDefault(k => k.Value.EqualsIgnoreCase(formattedColumn)).Key;

        public string GetValue(string columnName, dynamic row)
        {
            return (row as IDictionary<string, object>)
                .Where(d => d.Key == GetColumnDynamicPropertyName(columnName)).Select(d => d.Value as string).FirstOrDefault();
        }

        private int FindColumnNumber(string columnName)
        {
            if (_columnMapping.Any(c => c.Value == columnName))
                return _columnMapping.Where(c => c.Value == columnName).Select(c => c.Key).FirstOrDefault();
            else if (_columnNames.Any(c => c.Value == columnName))
                return _columnNames.Where(c => c.Value == columnName).Select(c => c.Key).FirstOrDefault();
            else
                throw new Exception("The specified column does not exist in the excel");
        }

        private string GetColumnDynamicPropertyName(string columnName)
        {
            return _columnNames.Where(c => c.Key == FindColumnNumber(columnName)).Select(d => d.Value).FirstOrDefault();
        }
        public string GetCellValue(int column, int row)
        {
            return (Rows.Skip(row).Take(1).FirstOrDefault() as IDictionary<string, object>)
                .Where(d => d.Key == _columnNames.Where(c => c.Key == column).Select(c => c.Value).FirstOrDefault())
                .Select(d => d.Value as string).FirstOrDefault();
        }
    }
}
