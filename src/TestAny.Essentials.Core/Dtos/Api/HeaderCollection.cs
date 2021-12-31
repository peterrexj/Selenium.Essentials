using System;
using System.Collections.Generic;
using System.Text;

namespace TestAny.Essentials.Core.Dtos.Api
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
