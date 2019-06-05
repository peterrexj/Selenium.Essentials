using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Essentials.Web.Controls
{
    public interface IWaitableControl
    {
        bool WaitForElementEnabled(bool throwExceptionWhenNotFound = true, string errorMessage = null);
        bool WaitForElementEnabled(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null);

        bool WaitForElementVisible(bool throwExceptionWhenNotFound = true, string errorMessage = null);

    }
}
