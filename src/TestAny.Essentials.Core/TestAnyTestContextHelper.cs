﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TestAny.Essentials.Core
{
    /// <summary>
    /// A context container for a test case
    /// </summary>
    public static class TestAnyTestContextHelper
    {
        private static Dictionary<string, object>? _globalContextCollection;
        private static Dictionary<string, object>? _contextCollection;

        private static Dictionary<string, object> GlobalContextObject
        {
            get
            {
                _globalContextCollection ??= new Dictionary<string, object>();
                return _globalContextCollection;
            }
        }
        private static Dictionary<string, object> ContextObject
        {
            get
            {
                _contextCollection ??= new Dictionary<string, object>();
                return _contextCollection;
            }
        }
        
        /// <summary>
        /// Initialze the test context for a test case
        /// </summary>
        public static void CreateTestContext()
        {
            _contextCollection = new Dictionary<string, object>();
        }

        #region Local Context

        /// <summary>
        /// Get the value from the context container based on the key
        /// </summary>
        /// <typeparam name="T">Generic type to which to convert when fetched</typeparam>
        /// <param name="key">key to search</param>
        /// <returns>The value stored as type mentioned in T</returns>
        public static T Get<T>(string key) => (T)ContextObject[key];

        /// <summary>
        /// Get the value from context container based on the key as 'string'
        /// </summary>
        /// <param name="key">key to search</param>
        /// <returns>The value stored as string</returns>
        public static string GetAsString(string key) => Get<string>(key);

        /// <summary>
        /// Set the value into the context container. If the value is already present, it will removed first.
        /// </summary>
        /// <param name="key">unique name of the key to store in the context container</param>
        /// <param name="value">value to store</param>
        public static void Set(string key, object value)
        {
            Remove(key);
            ContextObject.Add(key, value);
        }

        /// <summary>
        /// Checks if the value exists in the context container
        /// </summary>
        /// <param name="key">key to search</param>
        /// <returns>returns true or false based on the search result</returns>
        public static bool Exists(string key) => ContextObject.ContainsKey(key);
        public static void Remove(string key)
        {
            if (Exists(key))
            {
                ContextObject.Remove(key);
            }
        }
        #endregion

        #region Global Context
        /// <summary>
        /// Get the value from the global context container based on the key
        /// </summary>
        /// <typeparam name="T">Generic type to which to convert when fetched</typeparam>
        /// <param name="key">key to search</param>
        /// <returns>The value stored as type mentioned in T</returns>
        public static T GetGlobalContext<T>(string key) => (T)GlobalContextObject[key];

        /// <summary>
        /// Get the value from global context container based on the key as 'string'
        /// </summary>
        /// <param name="key">key to search</param>
        /// <returns>The value stored as string</returns>
        public static string GetGlobalContextAsString(string key) => GetGlobalContext<string>(key);

        /// <summary>
        /// Set the value into the global context container. If the value is already present, it will removed first.
        /// </summary>
        /// <param name="key">unique name of the key to store in the context container</param>
        /// <param name="value">value to store</param>
        public static void SetGlobalContext(string key, object value)
        {
            RemoveGlobalContext(key);
            GlobalContextObject.Add(key, value);
        }

        /// <summary>
        /// Checks if the value exists in the global context container
        /// </summary>
        /// <param name="key">key to search</param>
        /// <returns>returns true or false based on the search result</returns>
        public static bool ExistsGlobalContext(string key) => GlobalContextObject.ContainsKey(key);
        public static void RemoveGlobalContext(string key)
        {
            if (ExistsGlobalContext(key))
            {
                GlobalContextObject.Remove(key);
            }
        }

        #endregion
    }
}
