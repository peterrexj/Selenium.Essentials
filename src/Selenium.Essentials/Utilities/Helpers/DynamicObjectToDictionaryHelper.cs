using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Selenium.Essentials
{
    public static class DynamicObjectToDictionaryHelper
    {
        /// <summary>
        /// This allows you to convert a dynamic object to a dictionary where the
        /// key is the propery name of the object.
        /// 
        /// Usage:
        /// dynamic headers = new { Referer = "google.com" };
        /// var header = myObj.ToDictionary();
        /// 
        /// See: 
        /// http://stackoverflow.com/questions/11576886/how-to-convert-object-to-dictionarytkey-tvalue-in-c
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IDictionary<string, object> ToDictionary(this object source)
        {
            return source.ToDictionary<object>();
        }

        public static IDictionary<string, T> ToDictionary<T>(this object source)
        {
            if (source == null)
                ThrowExceptionWhenSourceArgumentIsNull();

            var dictionary = new Dictionary<string, T>();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
                AddPropertyToDictionary(property, source, dictionary);
            return dictionary;
        }

        private static void AddPropertyToDictionary<T>(PropertyDescriptor property, object source, Dictionary<string, T> dictionary)
        {
            object value = property.GetValue(source);
            if (IsOfType<T>(value))
                dictionary.Add(property.Name, (T)value);
        }

        private static bool IsOfType<T>(object value)
        {
            return value is T;
        }

        private static void ThrowExceptionWhenSourceArgumentIsNull()
        {
            throw new ArgumentNullException("source", "Unable to convert object to a dictionary. The source object is null.");
        }
    }
}
