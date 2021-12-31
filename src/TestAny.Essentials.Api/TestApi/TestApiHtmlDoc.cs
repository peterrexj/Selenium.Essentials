using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;
using Pj.Library;

namespace TestAny.Essentials.Api
{
    /// <summary>
    /// Api response in the form of HTML document
    /// </summary>
    public class TestApiHtmlDoc
    {
        private HtmlDocument doc;

        public TestApiHtmlDoc(string htmlContent)
        {
            doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);
        }

        /// <summary>
        /// Selects the html nodes based on the xpath filter
        /// </summary>
        /// <param name="xpath">xpath filter expression</param>
        /// <returns>Html node matching the xpath expression</returns>
        public IEnumerable<HtmlNode> Select(string xpath)
        {
            return doc.DocumentNode.SelectNodes(xpath).EmptyIfNull();
        }
    }
}
