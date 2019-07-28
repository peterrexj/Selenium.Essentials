using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Selenium.Essentials
{
    public static class DataRowExtensions
    {
        public static T ToObject<T>(this DataRow dataRow) where T : new()
        {
            T item = new T();
            foreach (DataColumn column in dataRow.Table.Columns)
            {
                PropertyInfo property = item.GetType().GetProperties().FirstOrDefault(p => p.Name.EqualsIgnoreCase(column.ColumnName));

                if (property != null && dataRow[column] != DBNull.Value)
                {
                    object result = Convert.ChangeType(dataRow[column], property.PropertyType);
                    property.SetValue(item, result, null);
                }
            }

            return item;
        }

        public static IEnumerable<T> ToObject<T>(this List<DataRow> dataRow) where T : new()
        {
            foreach (var row in dataRow)
            {
                yield return row.ToObject<T>();
            }
        }
    }
}
