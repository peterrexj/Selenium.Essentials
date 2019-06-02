using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Essentials.Web.Controls
{
    public interface IEditableControl
    {
        void Set(string value);
        string Get();
    }
}
