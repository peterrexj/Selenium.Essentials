using Pj.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TestAny.Essentials.Api.Extensions.Attributes;
using TestAny.Essentials.Core.Attributes;
using static Pj.Library.PjUtility;

namespace TestAny.Essentials.Api.Extensions
{
    /// <summary>
    /// Extension methods for Excel data transformation and template processing
    /// </summary>
    public static class ExcelDataTransformExtensions
    {
        /// <summary>
        /// Gets Excel data content based on the usage of the ExcelDataSourceAttribute on the calling method
        /// </summary>
        /// <param name="callerMethod">The method to check for ExcelDataSourceAttribute</param>
        /// <returns>ExcelData from the attribute, or null if not found</returns>
        public static ExcelData? GetExcelDataSourceContent(this MethodBase callerMethod)
        {
            return callerMethod?.GetCustomAttributes().OfType<ExcelDataSourceAttribute>()
                .Select(excelEnvAttr => excelEnvAttr.ExcelData).FirstOrDefault();
        }

        /// <summary>
        /// Gets Excel data content based on the usage of the ExcelDataSourceAttribute on the current calling method
        /// </summary>
        /// <returns>ExcelData from the attribute, or null if not found</returns>
        public static ExcelData? GetExcelDataSourceContent()
        {
            return Runtime.CallerMethod.GetExcelDataSourceContent();
        }

        /// <summary>
        /// Auto transform the template with the content from Excel data.
        /// The content provides the template which may contain {{key}}, that will be matched from excel data
        /// Load the excel using the attribute or excel loader and pass to this transform
        /// </summary>
        /// <param name="content">template to transform</param>
        /// <param name="data">excel data either loaded from the attribute or excel loader</param>
        /// <returns>Transformed content with placeholders replaced</returns>
        public static string TransformWithExcelData(this string content, ExcelData data)
        {
            if (content.IsEmpty() || data?.DataContent?.Keys.IsEmpty() != false)
                return content;

            data.DataContent.Iter(k =>
            {
                content = content.Replace("{{" + k.Key + "}}", k.Value);
                if (data.ExcelDataRaw.Sheets[data.WorkSheet].ColumnMapping.Any(x => x.Value.EqualsIgnoreCase(k.Key)))
                {
                    var originalKeyName = data
                        .ExcelDataRaw
                        .Sheets[data.WorkSheet]
                        .ColumnMapping
                        .First(x => x.Value.EqualsIgnoreCase(k.Key))
                        .Key;

                    content = content.Replace("{{" + originalKeyName + "}}", k.Value);
                }
            });
            return content;
        }

        /// <summary>
        /// Auto transform the template with the content from dictionary.
        /// The content provides the template which may contain {{key}}, that will be matched from dictionary
        /// </summary>
        /// <param name="content">template which may contain the {{key}}</param>
        /// <param name="data">dictionary to look for the value to replace</param>
        /// <returns>Transformed content with placeholders replaced</returns>
        public static string TransformWithDictionary(this string content, Dictionary<string, string> data)
        {
            if (content.IsEmpty() || data?.Keys.IsEmpty() != false)
                return content;

            data.Iter(k =>
            {
                content = content.Replace("{{" + k.Key + "}}", k.Value);
            });
            return content;
        }

        /// <summary>
        /// Auto transform the template with the content from Excel data loaded from the current method's ExcelDataSourceAttribute.
        /// The content provides the template which may contain {{key}}, that will be matched from excel data
        /// </summary>
        /// <param name="content">template to transform</param>
        /// <returns>Transformed content with placeholders replaced</returns>
        public static string TransformWithExcelDataSource(this string content)
        {
            var excelData = GetExcelDataSourceContent();
            return content.TransformWithExcelData(excelData);
        }
    }
}
