using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Selenium.Essentials.Api.Framework;
using Selenium.Essentials.Utilities.Extensions;

namespace Selenium.Essentials.Api
{
    public class TestBody
    {
        private TestHtmlDoc _htmlContent;
        private string _stringContent;

        public string StringContent
        {
            get
            {
                return _stringContent;
            }

            private set
            {
                if (_htmlContent != null)
                    _htmlContent = null;

                _stringContent = value;
            }
        }

        public dynamic JsonContent
        {
            get
            {
                try
                {
                    return JsonConvert.DeserializeObject<dynamic>(StringContent);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public TestHtmlDoc HtmlContent
        {
            get
            {
                if (_htmlContent == null)
                    _htmlContent = new TestHtmlDoc(StringContent);

                return _htmlContent;
            }
        }

        public string FilterByXpath(string xpathExpression, string attributeToRetrive) => HtmlContent.Select(xpathExpression).FirstOrDefault()?.GetAttributeValue(attributeToRetrive, string.Empty);
        public IEnumerable<string> FilterByXpathGetAll(string xpathExpression, string attributeToRetrive) => HtmlContent.Select(xpathExpression).Select(s => s.GetAttributeValue(attributeToRetrive, string.Empty));
        public string FilterByXpathAndGetInnerText(string xpathExpression) => HtmlContent.Select(xpathExpression).FirstOrDefault().InnerText;

        public IEnumerable<string> FilterJsonContent(string filterText) => StringContent.ApplyJsonPathExpression(filterText);

        public TestBody(string content)
        {
            StringContent = content;
        }
    }
}
