using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Selenium.Essentials
{
    public class TestApiBase
    {
        protected string XmlPayloadContent
          => Utility.Runtime.CallerMethod.GetCustomAttributes().OfType<PayloadDataXmlAttribute>()
              .Select(xmlAttr => xmlAttr.FileContent).FirstOrDefault().EmptyIfNull();

        protected string JsonPayloadContent
            => Utility.Runtime.CallerMethod.GetCustomAttributes().OfType<PayloadDataJsonAttribute>()
                .Select(jsonAttr => jsonAttr.FileContent).FirstOrDefault();

        protected ExcelData ExcelDataSourceContent =>
            Utility.Runtime.CallerMethod.GetCustomAttributes().OfType<ExcelDataSourceAttribute>()
                .Select(excelEnvAttr => excelEnvAttr.ExcelData).FirstOrDefault();

        protected string AutoTransform(string content, Dictionary<string, string> data)
        {
            if (content.IsEmpty() || data.Keys.IsEmpty())
                return content;

            data.Iter(k =>
            {
                content = content.Replace("{{" + k.Key + "}}", k.Value);
            });
            return content;
        }

        protected string Transform(string content, ExcelData data)
        {
            if (content.IsEmpty() || data.DataContent.Keys.IsEmpty())
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

        //protected ExcelData ExcelDataSource { get; private set; }

        //protected void InitializeExcelData(string filePath, string worksheet, string key = "", string column = "", string filter = "")
        //{
        //    ExcelDataSource = new ExcelData(filePath, worksheet, key, column, filter);
        //}
    }
}
