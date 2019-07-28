using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;

namespace Selenium.Essentials
{
    public class TestApiHtmlDoc
    {
        private HtmlDocument doc;

        public TestApiHtmlDoc(string htmlContent)
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
