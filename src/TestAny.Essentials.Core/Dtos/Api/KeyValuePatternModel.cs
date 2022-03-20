using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestAny.Essentials.Core.Dtos.Api
{
    public class KeyValuePatternModel
    {
        private IEnumerable<KeyValuePair<string, string>> _content;
        private IEnumerable<KeyValuePair<string, string>> Content => _content;

        public KeyValuePatternModel(IEnumerable<KeyValuePair<string, string>> content)
        {
            if (content == null)
            {
                _content = new List<KeyValuePair<string, string>>();
            }
            else
            {
                _content = content;
            }
        }

        public bool Exists(string key) => _content.Any(k => k.Key == key);
        public string Filter(string key) => Exists(key) ? _content.First(k => k.Key == key).Value : string.Empty;
        public List<string> FilterGetALl(string key) => Exists(key) ? _content.Where(k => k.Key == key).Select(k => k.Value).ToList() : new List<string>();
    }
}
