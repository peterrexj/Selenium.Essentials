using Pj.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TestAny.Essentials.Core.Attributes
{
    /// <summary>
    /// Json Payload attribute for nUnit Api and Web tests
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class PayloadDataJsonAttribute : Attribute
    {
        /// <summary>
        /// Relative path to the json file
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Content of the json file, which will be loaded during initialization
        /// </summary>
        public string FileContent { get; private set; }

        /// <summary>
        /// Read the json file and exposed through FileContent property
        /// </summary>
        /// <param name="filePath">path to the json file (relative path)</param>
        public PayloadDataJsonAttribute(string filePath)
        {
            if (!File.Exists(filePath))
            {
                FilePath = Path.Combine(Pj.Library.PjUtility.Runtime.ExecutingFolder, filePath);
            }
            else
            {
                FilePath = filePath;
            }

            if (IoHelper.FileNotExists(FilePath))
            {
                throw new Exception($"The Json file trying to load is unavailable in the location {FilePath}");
            }

            FileContent = File.ReadAllText(FilePath);
        }
    }
}