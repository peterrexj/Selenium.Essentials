using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Selenium.Essentials.Api.Framework;
using Selenium.Essentials.Utilities.Extensions;

namespace Selenium.Essentials.Api
{
    public class TestApiBody
    {
        public TestApiBody(string content)
        {
            ContentString = content;
        }

        private TestApiHtmlDoc _contentHtml;
        private string _contentString;

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

        public TestApiHtmlDoc ContentHtml
        {
            get
            {
                if (_contentHtml == null)
                    _contentHtml = new TestApiHtmlDoc(ContentString);

                return _contentHtml;
            }
        }

        public string FilterByXpath(string xpathExpression, string attributeToRetrive) => 
            ContentHtml
            .Select(xpathExpression)
            .FirstOrDefault()?
            .GetAttributeValue(attributeToRetrive, string.Empty);

        public IEnumerable<string> FilterByXpathGetAll(string xpathExpression, string attributeToRetrive) => 
            ContentHtml
            .Select(xpathExpression)
            .Select(s => s.GetAttributeValue(attributeToRetrive, string.Empty));

        public string FilterByXpathAndGetInnerText(string xpathExpression) => 
            ContentHtml
            .Select(xpathExpression)
            .FirstOrDefault()
            .InnerText;

        public IEnumerable<string> FilterJsonContent(string filterText) => 
            ContentString
            .ApplyJsonPathExpression(filterText);
    }
}
