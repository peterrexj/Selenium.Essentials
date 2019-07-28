using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Selenium.Essentials
{
    public static class AssemblyExtensions
    {
        public static Stream GetEmbeddedResourceAsStream(this Assembly assembly, string path)
        {
            return assembly.GetManifestResourceStream(path);
        }

        public static string GetEmbeddedResourceAsText(this Assembly assembly, string path)
        {
            return new StreamReader(assembly.GetManifestResourceStream(path)).ReadToEnd();
        }
    }
}
