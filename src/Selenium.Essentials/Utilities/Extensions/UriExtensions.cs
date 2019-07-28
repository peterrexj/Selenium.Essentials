using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials
{
    public static class UriExtensions
    {
        private const string __DOMAIN_EXTRACT = @"^(?:https?:\/\/)?(?:[^@\n]+@)?(?:www\.)?([^:\/\n]+)";

        /// <summary>
        /// Returns the name of the Domain
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetDomainName(this Uri url) => url.ToString().RegexMatchGroupValue(__DOMAIN_EXTRACT, 1)?.ToString();

        /// <summary>
        /// Returns the name of the Domain including the scheme (protocol)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetDomain(this Uri url) => $"https://{GetDomainName(url)}";
    }
}
