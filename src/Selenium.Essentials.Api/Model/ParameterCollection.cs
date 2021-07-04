using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials
{
    public class ParameterCollection : Dictionary<string, object>
    {
        public new ParameterCollection Add(string key, object value)
        {
            if (!base.ContainsKey(key))
            {
                base.Add(key, value?.ToString());
            }
            return this;
        }

        public ParameterCollection AddOrUpdate(string key, object value)
        {
            if (!base.ContainsKey(key))
            {
                base.Add(key, value?.ToString());
            }
            else
            {
                base[key] = value?.ToString();
            }
            return this;
        }
    }
}
