using Selenium.Essentials.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Selenium.Essentials.Utilities.Helpers
{
    public static class StorageHelper
    {
        public static void DeleteFile(string path)
        {
            if (!File.Exists(path)) return;

            Console.WriteLine($"Deleting file: {path}");
            File.Delete(path);
        }

        public static void CreateDirectory(string path)
        {
            if (Path.GetExtension(path).HasValue())
            {
                path = Path.GetDirectoryName(path);
            }
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static bool Exists(string path)
        {
            if (Path.GetExtension(path).HasValue())
            {
                return File.Exists(path);
            }
            else
            {
                return Directory.Exists(path);
            }
        }
    }
}
