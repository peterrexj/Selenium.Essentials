using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Pj.Library;
using static Pj.Library.PjUtility;

namespace Selenium.Essentials
{
    /// <summary>
    /// Api Response body representation
    /// </summary>
    public class TestApiBody
    {
        public TestApiBody(string content)
        {
            ContentString = content;
        }

        private TestApiHtmlDoc _contentHtml;
        private string _contentString;

        /// <summary>
        /// Returns the response message content as string
        /// </summary>
        public string ContentString
        {
            get
            {
                return _contentString;
            }

            private set
            {
                if (_contentHtml != null)
                    _contentHtml = null;

                _contentString = value;
            }
        }

        /// <summary>
        /// Returns the response message as Json (which is of type dynamic)
        /// </summary>
        public dynamic ContentJson
        {
            get
            {
                try
                {
                    return JsonConvert.DeserializeObject<dynamic>(ContentString);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Returns the response message as type HtmlDocument (HtmlAgilityPack)
        /// </summary>
        public TestApiHtmlDoc ContentHtml
        {
            get
            {
                if (_contentHtml == null)
                    _contentHtml = new TestApiHtmlDoc(ContentString);

                return _contentHtml;
            }
        }

        /// <summary>
        /// Convert the Json to a type
        /// </summary>
        /// <typeparam name="T">Type to which the json needs to be converted</typeparam>
        /// <returns>T: Type to which json is converted</returns>
        public T ToType<T>()
        {
            try
            {
                return SerializationHelper.DeSerializeJsonFromString<T>(JsonConvert.SerializeObject(ContentJson));
            }
            catch (Exception ex)
            {
                Runtime.Logger.Log($"Conversion failed to type: {typeof(T).Name}. Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Convert the Json to a type
        /// </summary>
        /// <typeparam name="T">Type to which the json needs to be converted</typeparam>
        /// <param name="content">json in form of dynamic content</param>
        /// <returns>T: Type to which json is converted</returns>
        public T ToType<T>(dynamic content)
        {
            try
            {
                return SerializationHelper.DeSerializeJsonFromString<T>(JsonConvert.SerializeObject(content));
            }
            catch (Exception ex)
            {
                Runtime.Logger.Log($"Conversion failed to type: {typeof(T).Name}. Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Filter the Html content using xpath selector and returns the attribute value
        /// </summary>
        /// <param name="xpathExpression">xpath expression to filter</param>
        /// <param name="attributeToRetrive">attribute within the html tag to return</param>
        /// <returns>The attribute value of the element matched</returns>
        public string FilterByXpath(string xpathExpression, string attributeToRetrive) =>
            ContentHtml
            .Select(xpathExpression)
            .FirstOrDefault()?
            .GetAttributeValue(attributeToRetrive, string.Empty);

        /// <summary>
        /// Filter the Html content using xpath selector and returns the attribute values of all match element
        /// </summary>
        /// <param name="xpathExpression">xpath expression to filter</param>
        /// <param name="attributeToRetrive">attribute of the element which will read and returned</param>
        /// <returns>The collection of attribute value of the element matched</returns>
        public IEnumerable<string> FilterByXpathGetAll(string xpathExpression, string attributeToRetrive) =>
            ContentHtml
            .Select(xpathExpression)
            .Select(s => s.GetAttributeValue(attributeToRetrive, string.Empty));

        /// <summary>
        /// Filter the Html content using xpath selector and return the InnerText of the element
        /// </summary>
        /// <param name="xpathExpression">xpath expression to filter</param>
        /// <returns>InnerText of the matched xapth</returns>
        public string FilterByXpathAndGetInnerText(string xpathExpression) =>
            ContentHtml
            .Select(xpathExpression)
            .FirstOrDefault()
            .InnerText;

        /// <summary>
        /// Filter the json using json path expression
        /// </summary>
        /// <param name="filterText">json path expression</param>
        /// <returns>returns the IEnumerable<string> which matched the json path expression against the content</returns>
        public IEnumerable<string> FilterJsonContent(string filterText) =>
            ContentString
            .ApplyJsonPathExpression(filterText);
    }
}
