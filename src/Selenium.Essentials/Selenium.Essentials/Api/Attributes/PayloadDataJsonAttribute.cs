using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Selenium.Essentials.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class PayloadDataJsonAttribute : Attribute
    {
        public string FilePath { get; set; }
        public string FileContent { get; private set; }

        public PayloadDataJsonAttribute(string filePath)
        {
            if (!File.Exists(filePath))
            {
                FilePath = Path.Combine(Utility.Runtime.ExecutingFolder, filePath);
            }

            File.Exists(FilePath).Should()
                .BeTrue($"The Json file trying to load is unavailable in the location {FilePath}");

            FileContent = File.ReadAllText(FilePath);
        }
    }
}