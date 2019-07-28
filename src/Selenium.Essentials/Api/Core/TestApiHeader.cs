using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials
{
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
