using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials
{
    public static class TestContextHelper
    {
        private static Dictionary<string, object> _contextCollection;

        private static Dictionary<string, object> ContextObject => _contextCollection;

        public static void CreateTestContext()
        {
            _contextCollection = new Dictionary<string, object>();
        }

        public static T Get<T>(string key) => (T)ContextObject[key];
        public static string GetAsString(string key) => Get<string>(key);
        public static void Set(string key, object value)
        {
            Remove(key);
            ContextObject.Add(key, value);
        }
        public static bool Exists(string key) => ContextObject.ContainsKey(key);
        public static void Remove(string key)
        {
            if (Exists(key))
            {
                ContextObject.Remove(key);
            }
        }

        public static IWebDriver Driver
        {
            get
            {
                if (Exists("Driver"))
                {
                    return Get<IWebDriver>("Driver");
                }
                return null;
            }
        }
    }


}
