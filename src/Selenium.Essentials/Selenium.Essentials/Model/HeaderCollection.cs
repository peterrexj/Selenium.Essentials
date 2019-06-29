using Selenium.Essentials.Api;
using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials.Model
{
    public class HeaderCollection : List<TestHeader>
    {
        public HeaderCollection()
        {

        }
        public HeaderCollection(IEnumerable<TestHeader> headers)
            : base(headers)
        {

        }
    }
}
