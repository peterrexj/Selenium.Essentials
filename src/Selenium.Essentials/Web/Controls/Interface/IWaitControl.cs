using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Essentials
{
    public interface IWaitControl
    {
        bool WaitUntilElementEnabled(bool throwExceptionWhenNotFound = true, string errorMessage = null);
        bool WaitUntilElementEnabled(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null);

        bool WaitUntilElementVisible(bool throwExceptionWhenNotFound = true, string errorMessage = null);
        bool WaitUntilElementVisible(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null);

        bool WaitUntilElementExists(bool throwExceptionWhenNotFound = true, string errorMessage = null);
        bool WaitUntilElementExists(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null);

        bool WaitUntilElementCssDisplayed(bool throwExceptionWhenNotFound = true, string errorMessage = null);
        bool WaitUntilElementCssDisplayed(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null);

        bool WaitUntilElementInvisible(bool throwExceptionWhenNotFound = true, string errorMessage = null);
        bool WaitUntilElementInvisible(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null);

        bool WaitUntilElementClickable(bool throwExceptionWhenNotFound = true, string errorMessage = null);
        bool WaitUntilElementClickable(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null);

        bool WaitUntilElementTextTrimEquals(string textToMatch, bool throwExceptionWhenNotFound = true, string errorMessage = null);
        bool WaitUntilElementTextTrimEquals(string textToMatch, int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null);

        bool WaitUntilElementTextStartsWith(string textToMatch, bool throwExceptionWhenNotFound = true, string errorMessage = null);
        bool WaitUntilElementTextStartsWith(string textToMatch, int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null);

        bool WaitUntilElementTextStartsWith(string[] textsToMatch, bool throwExceptionWhenNotFound = true, string errorMessage = null);
        bool WaitUntilElementTextStartsWith(string[] textsToMatch, int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null);

        bool WaitUntilElementTextContains(string textToMatch, bool throwExceptionWhenNotFound = true, string errorMessage = null);
        bool WaitUntilElementTextContains(string textToMatch, int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null);

        bool WaitUntilElementTextContains(string[] textsToMatch, bool throwExceptionWhenNotFound = true, string errorMessage = null);
        bool WaitUntilElementTextContains(string[] textsToMatch, int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null);

        bool WaitUntilElementHasSomeText(bool throwExceptionWhenNotFound = true, string errorMessage = null);
        bool WaitUntilElementHasSomeText(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null);
    }
}
