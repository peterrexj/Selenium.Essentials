using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Selenium.Essentials
{
    /// <summary>
    /// Extention methods for Assembly
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Returns the embedded resource as a stream for the given path
        /// </summary>
        /// <param name="assembly">Assembly on which resource is to be fetched</param>
        /// <param name="path">Path of the file which needs to be fetched wihin the assembly</param>
        /// <returns></returns>
        public static Stream GetEmbeddedResourceAsStream(this Assembly assembly, string path)
        {
            return assembly.GetManifestResourceStream(path);
        }

        /// <summary>
        /// Returns the embedded resource as a string for the given path
        /// </summary>
        /// <param name="assembly">Assembly on which resource is to be fetched</param>
        /// <param name="path">Path of the file which needs to be fetched wihin the assembly</param>
        /// <returns></returns>
        public static string GetEmbeddedResourceAsText(this Assembly assembly, string path)
        {
            return new StreamReader(assembly.GetManifestResourceStream(path)).ReadToEnd();
        }
    }
}
