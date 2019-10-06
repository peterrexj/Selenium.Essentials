using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials
{
    /// <summary>
    /// Interface for logging message
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Log message with the error details
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        void Log(string message, Exception ex = null);
    }
}
