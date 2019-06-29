using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;
using Selenium.Essentials.Utilities.Extensions;

namespace Selenium.Essentials.Api.Framework
{
    public class TestHtmlDoc
    {
        private HtmlDocument doc;

        public TestHtmlDoc(string htmlContent)
        {
            doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);
        }

        public IEnumerable<HtmlNode> Select(string xpath)
        {
            return doc.DocumentNode.SelectNodes(xpath).EmptyIfNull();
        }
    }
}
