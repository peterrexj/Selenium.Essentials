using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Selenium.Essentials
{
    public static class CookieExtensions
    {
        /// <summary>
        /// Converts a System.Net.Cookie to Selenium Cookie
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static OpenQA.Selenium.Cookie ClientCookieToSeleniumCookie(this Cookie cookie)
        {
            var dictOfCookieValue = new Dictionary<string, object>
            {
                { "name", cookie.Name },
                { "value", cookie.Value },
                { "domain", cookie.Domain },
                { "path", cookie.Path },
                { "secure", cookie.Secure },
                { "IsHttpOnly", cookie.HttpOnly },
                { "Expiry", cookie.Expires }
            };
            return OpenQA.Selenium.Cookie.FromDictionary(dictOfCookieValue);
        }
    }
}
