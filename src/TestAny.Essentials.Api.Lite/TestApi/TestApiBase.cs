using Pj.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TestAny.Essentials.Core.Attributes;
using static Pj.Library.PjUtility;

namespace TestAny.Essentials.Api
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
    }
}
