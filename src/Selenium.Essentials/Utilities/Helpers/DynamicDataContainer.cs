using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Selenium.Essentials.Utilities.Helpers
{
    public class DynamicDataContainer : DynamicObject
    {
        public static dynamic GetDynamicObject(Dictionary<string, string> properties)
        {
            return new DynamicDataContainer(properties);
        }

        private readonly Dictionary<string, string> _properties;

        public DynamicDataContainer(Dictionary<string, string> properties)
        {
            _properties = properties;
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _properties.Keys;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (!_properties.ContainsKey(binder.Name))
            {
                _properties.Add(binder.Name, "");
            }
            if (_properties.ContainsKey(binder.Name))
            {
                result = _properties[binder.Name];
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (_properties.ContainsKey(binder.Name))
            {
                _properties[binder.Name] = value as string;
                return true;
            }

            return false;
        }
    }
}
