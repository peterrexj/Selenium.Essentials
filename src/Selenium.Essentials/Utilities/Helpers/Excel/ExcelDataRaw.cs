using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials
{
    public abstract class ExcelDataRaw : IDisposable
    {
        protected ConcurrentBag<dynamic> _data;

        protected ExcelDataRaw()
        {
            _data = new ConcurrentBag<dynamic>();
        }

        public void Dispose()
        {
            _data = null;
        }
    }
}
