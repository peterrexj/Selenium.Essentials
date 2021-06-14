using Pj.Library;
using System.Drawing;

namespace Selenium.Essentials
{
    /// <summary>
    /// General utility class
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Application configuration information
        /// </summary>
      
        /// <summary>
        /// Tablet window size.
        /// Default to Size(768, 1024)
        /// </summary>
        public static Size TabletWindowSize { get; set; } = new Size(768, 1024);

        /// <summary>
        /// Mobile window size
        /// Default to Size(375, 667)
        /// </summary>
        public static Size MobileWindowSize { get; set; } = new Size(375, 667);

        /// <summary>
        /// Initialize the selenium essentials framework.
        /// Create a test context and initialze the logging
        /// </summary>
        public static void InitializeFramework(ILog logger = null)
        {
            Pj.Library.PjUtility.InitializeUtility(logger);
            TestContextHelper.CreateTestContext();
        }
    }
}
