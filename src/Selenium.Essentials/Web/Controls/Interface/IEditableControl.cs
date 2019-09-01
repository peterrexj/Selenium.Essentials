using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials
{
    public interface IEditableControl
    {
        void Set(string value);
        string Get();
    }
}
