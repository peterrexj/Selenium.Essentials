using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials
{
    public static class DictionaryExtensions
    {
        public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                if (value.ToString().HasValue())
                {
                    dictionary[key] = value;
                }
            }
            else
            {
                dictionary.Add(key, value);
            }
        }

        public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> destination, Dictionary<TKey, TValue> source)
        {
            if (source != null)
            {
                source.Keys.Iter(k => destination.AddOrUpdate(k, source[k]));
            }
        }
    }
}
