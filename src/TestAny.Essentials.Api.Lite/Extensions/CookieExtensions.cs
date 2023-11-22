using Pj.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TestAny.Essentials.Api
{
    public static class CookieExtensions
    {
        public static CookieCollection Merge(this CookieCollection cookieCollection01, CookieCollection cookieCollection02)
        {
            var newCollection = new CookieCollection();

            foreach (System.Net.Cookie cookie in cookieCollection01)
            {
                newCollection.Add(cookie);
            }

            foreach (System.Net.Cookie cookie in cookieCollection02)
            {
                if (!newCollection.Contains(cookie.Name))
                {
                    newCollection.Add(cookie);
                }
            }

            return newCollection;
        }

        public static CookieCollection Concat(this CookieCollection cookieCollection01, CookieCollection cookieCollection02)
        {
            var newCollection = new CookieCollection();

            foreach (System.Net.Cookie cookie in cookieCollection01)
            {
                newCollection.Add(cookie);
            }

            foreach (System.Net.Cookie cookie in cookieCollection02)
            {
                newCollection.Add(cookie);
            }

            return newCollection;
        }

        public static bool Contains(this CookieCollection cookieCollection, string name)
        {
            foreach (System.Net.Cookie cookie in cookieCollection)
            {
                if (cookie.Name.Equals(name))
                {
                    return true;
                }
            }
            return false;
        }

        public static CookieCollection ExtractCookies(this CookieCollection cookieCollection, IEnumerable<KeyValuePair<string, string>> headers, string requestUrl)
        {
            foreach (var item in headers.Where(k => k.Key.EqualsIgnoreCase("Set-Cookie")))
            {
                cookieCollection.Add(ParseCookieString(item.Value, requestUrl));
            }
            return cookieCollection;
        }

        static private CookieCollection ParseCookieString(string cookieString, string requestUrl)
        {
            var availableProperties = new string[] { "expires", "path", "secure", "httponly", "samesite", "domain", "priority" };

            var strTemp = new StringBuilder();
            Queue<Tuple<string, string>> cookieQueue = new Queue<Tuple<string, string>>();
            CookieCollection cookieCollection = new CookieCollection();
            List<List<Tuple<string, string>>> collection = new List<List<Tuple<string, string>>>();
            List<Tuple<string, string>> inOrderOfEntity = new List<Tuple<string, string>>();
            Tuple<string, string> tempExtractedContent;
            var propKey = "";
            var propValue = "";

            if (!(cookieString.Trim().EndsWith(",") || cookieString.Trim().EndsWith(";")))
            {
                cookieString = $"{cookieString}; ";
            }

            foreach (var c in cookieString)
            {
                if (c == '=' && propKey.IsEmpty())
                {
                    propKey = strTemp.ToString();
                    strTemp.Clear();
                }
                else if ((c == ';') || (c == ',' && !propKey.Contains("expires")))
                {
                    propValue = strTemp.ToString();
                    strTemp.Clear();
                    if (propKey.HasValue())
                    {
                        cookieQueue.Enqueue(new Tuple<string, string>(propKey.EmptyIfNull().Trim(), propValue.EmptyIfNull().Trim()));
                    }
                    else if (propValue.HasValue())
                    {
                        cookieQueue.Enqueue(new Tuple<string, string>(propValue.EmptyIfNull().Trim(), ""));
                    }
                    propKey = string.Empty;
                    propValue = string.Empty;
                }
                else
                {
                    strTemp.Append(c.ToString());
                }
            }

            while (cookieQueue.Count > 0)
            {
                tempExtractedContent = cookieQueue.Dequeue();
                if (availableProperties.Contains(tempExtractedContent.Item1.ToLower()) || inOrderOfEntity.Count == 0)
                {
                    inOrderOfEntity.Add(tempExtractedContent);
                }
                else
                {
                    collection.Add(inOrderOfEntity);
                    inOrderOfEntity = new List<Tuple<string, string>> { tempExtractedContent };
                }
            }

            if (inOrderOfEntity.Count > 0)
            {
                collection.Add(inOrderOfEntity);
            }

            foreach (var singleCookieDefinition in collection)
            {
                try
                {
                    var coreValueProperty = singleCookieDefinition.FirstOrDefault(d => !availableProperties.Contains(d.Item1.ToLower()));
                    if (coreValueProperty != null)
                    {
                        Cookie cookie = new Cookie(coreValueProperty.Item1, coreValueProperty.Item2);
                        if (singleCookieDefinition.Any(d => d.Item1.EqualsIgnoreCase("path")))
                        {
                            cookie.Path = singleCookieDefinition.FirstOrDefault(d => d.Item1.EqualsIgnoreCase("path")).Item2;
                        }
                        cookie.Domain = singleCookieDefinition.Any(s => s.Item1.EqualsIgnoreCase("domain") && s.Item2.HasValue())
                            ? singleCookieDefinition.FirstOrDefault(s => s.Item1.EqualsIgnoreCase("domain")).Item2
                            : new Uri(requestUrl).Host.SplitAndTrim(":").First();
                        if (singleCookieDefinition.Any(s => s.Item1.EqualsIgnoreCase("expires") && s.Item2.HasValue()))
                        {
                            if (DateTime.TryParse(singleCookieDefinition.First(s => s.Item1.EqualsIgnoreCase("expires")).Item2, out DateTime parsedExpireValue))
                            {
                                cookie.Expires = parsedExpireValue;
                            }
                        }
                        cookie.Secure = singleCookieDefinition.Any(d => d.Item1.EqualsIgnoreCase("secure"));
                        cookie.HttpOnly = singleCookieDefinition.Any(d => d.Item1.EqualsIgnoreCase("httponly"));

                        cookieCollection.Add(cookie);
                    }
                }
                catch (Exception ex)
                {
                    PjUtility.Runtime.Logger.Log(ex.ToString());
                }
            }

            return cookieCollection;
        }
    }
}
