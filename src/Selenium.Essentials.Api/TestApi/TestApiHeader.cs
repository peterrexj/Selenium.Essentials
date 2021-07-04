using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials
{
    /// <summary>
    /// Api Header class which will hold the header information which will used for request
    /// </summary>
    public class TestApiHeader
    {
        public TestApiHeader(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }
        public string Value { get; set; }
    }
}
