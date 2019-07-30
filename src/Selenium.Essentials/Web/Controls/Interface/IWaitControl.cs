using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Essentials
{
    public interface IWaitControl
    {
        bool WaitForElementEnabled(bool throwExceptionWhenNotFound = true, string errorMessage = null);
        bool WaitForElementEnabled(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null);

        bool WaitForElementVisible(bool throwExceptionWhenNotFound = true, string errorMessage = null);
        bool WaitForElementVisible(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null);

        bool WaitForElementExists(bool throwExceptionWhenNotFound = true, string errorMessage = null);
        bool WaitForElementExists(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null);

        bool WaitForElementCssDisplayed(bool throwExceptionWhenNotFound = true, string errorMessage = null);
        bool WaitForElementCssDisplayed(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null);

        bool WaitForElementInvisible(bool throwExceptionWhenNotFound = true, string errorMessage = null);
        bool WaitForElementInvisible(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null);

        bool WaitForElementClickable(bool throwExceptionWhenNotFound = true, string errorMessage = null);
        bool WaitForElementClickable(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null);

        bool WaitForElementTextTrimEquals(string textToMatch, bool throwExceptionWhenNotFound = true, string errorMessage = null);
        bool WaitForElementTextTrimEquals(string textToMatch, int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null);

        bool WaitForElementTextStartsWith(string textToMatch, bool throwExceptionWhenNotFound = true, string errorMessage = null);
        bool WaitForElementTextStartsWith(string textToMatch, int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null);

        bool WaitForElementTextStartsWith(string[] textsToMatch, bool throwExceptionWhenNotFound = true, string errorMessage = null);
        bool WaitForElementTextStartsWith(string[] textsToMatch, int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null);

        bool WaitForElementTextContains(string textToMatch, bool throwExceptionWhenNotFound = true, string errorMessage = null);
        bool WaitForElementTextContains(string textToMatch, int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null);

        bool WaitForElementTextContains(string[] textsToMatch, bool throwExceptionWhenNotFound = true, string errorMessage = null);
        bool WaitForElementTextContains(string[] textsToMatch, int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null);

        bool WaitForElementHasSomeText(bool throwExceptionWhenNotFound = true, string errorMessage = null);
        bool WaitForElementHasSomeText(int waitTimeSec, bool throwExceptionWhenNotFound = true, string errorMessage = null);
    }
}
