using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Selenium.Essentials
{
    /// <summary>
    /// Xml Payload attribute for nUnit Api and Web tests
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class PayloadDataXmlAttribute : Attribute
    {
        /// <summary>
        /// Relative path to the xml file
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Content of the xml file, which will be loaded during initialization
        /// </summary>
        public string FileContent { get; private set; }

        /// <summary>
        /// Read the xml file and exposed through FileContent property
        /// </summary>
        /// <param name="filePath"></param>
        public PayloadDataXmlAttribute(string filePath)
        {
            if (!File.Exists(filePath))
            {
                FilePath = Path.Combine(Utility.Runtime.ExecutingFolder, filePath);
            }
            else
            {
                FilePath = filePath;
            }

            File.Exists(FilePath).Should()
                .BeTrue($"The Xml file trying to load is unavailable in the location {FilePath}");

            FileContent = File.ReadAllText(FilePath);
        }
    }
}
