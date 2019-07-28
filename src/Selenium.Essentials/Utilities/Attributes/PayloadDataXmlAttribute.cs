using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Selenium.Essentials
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class PayloadDataXmlAttribute : Attribute
    {
        public string FilePath { get; set; }
        public string FileContent { get; private set; }

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
