using Pj.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using static Pj.Library.PjUtility;

namespace Selenium.Essentials
{
    /// <summary>
    /// Api test base class, inherit this class on top of your api test class
    /// </summary>
    public class TestApiBase
    {
        /// <summary>
        /// Xml payload content based on the usage of the xml payload attribute
        /// </summary>
        protected string XmlPayloadContent
          => Runtime.CallerMethod.GetCustomAttributes().OfType<PayloadDataXmlAttribute>()
              .Select(xmlAttr => xmlAttr.FileContent).FirstOrDefault().EmptyIfNull();

        /// <summary>
        /// json payload content based on the usage of the json payload attribute
        /// </summary>
        protected string JsonPayloadContent
            => Runtime.CallerMethod.GetCustomAttributes().OfType<PayloadDataJsonAttribute>()
                .Select(jsonAttr => jsonAttr.FileContent).FirstOrDefault();

        /// <summary>
        /// excel data content based on the usage of the excel payload attribute
        /// </summary>
        protected ExcelData ExcelDataSourceContent =>
            Runtime.CallerMethod.GetCustomAttributes().OfType<ExcelDataSourceAttribute>()
                .Select(excelEnvAttr => excelEnvAttr.ExcelData).FirstOrDefault();

        /// <summary>
        /// Auto transform the template with the content from dictionary.
        /// The content provides the template which may contain {{key}}, that will be matched from dictionary
        /// </summary>
        /// <param name="content">template which may contain the {{key}}</param>
        /// <param name="data">dictionary to look for the value to replace</param>
        /// <returns></returns>
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

        /// <summary>
        /// Auto transform the template with the content from excel data.
        /// The content provides the template which may contain {{key}}, that will be matched from excel data
        /// Load the excel using the attribute or excel loader and pass to this transform
        /// </summary>
        /// <param name="content">template to transform</param>
        /// <param name="data">excel data either loaded from the attribute or excel loader</param>
        /// <returns></returns>
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
    }
}
