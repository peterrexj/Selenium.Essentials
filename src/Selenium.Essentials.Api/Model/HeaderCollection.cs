using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials
{
    public class HeaderCollection : List<TestApiHeader>
    {
        public HeaderCollection()
        {

        }
        public HeaderCollection(IEnumerable<TestApiHeader> headers)
            : base(headers)
        {

        }
    }
}
