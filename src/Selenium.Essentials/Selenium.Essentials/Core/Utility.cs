using Selenium.Essentials.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Selenium.Essentials
{
    public static class Utility
    {
        public static class Runtime
        {
            public static Assembly ExecutingAssembly => Assembly.GetExecutingAssembly();
            public static string ExecutingFolder => Path.GetDirectoryName(ExecutingAssembly.Location);
            public static string LogOutputFolder { get; set; } = Path.Combine(ExecutingFolder, "Logs");

            public static Assembly GetAssembly(string name) => new StackTrace().GetFrames()
                .Select(x => x?.GetMethod().ReflectedType?.Assembly).Distinct()
                .FirstOrDefault(x => x != null && (x.ManifestModule.Name.EqualsIgnoreCase(name) || 
                                    Path.GetFileNameWithoutExtension(x.ManifestModule.Name).EqualsIgnoreCase(name)));

            public static Assembly CallingAssembly => new StackTrace().GetFrames()
                .Select(x => x?.GetMethod().ReflectedType?.Assembly).Distinct()
                .LastOrDefault(x => x != null && x.GetReferencedAssemblies().Any(y => y.FullName == ExecutingAssembly.FullName));

            public static MethodBase CallerMethod => new StackTrace().GetFrames()
                .Where(x => x?.GetMethod().ReflectedType?.Assembly.FullName == CallingAssembly.FullName)
                .Select(x => x.GetMethod())
                .Distinct()
                .LastOrDefault();

            public static string GetCallerMethodName() => CallerMethod.Name;
            public static Type CallerType => CallerMethod.DeclaringType;
            public static string CallingAssemblyFullPath => CallingAssembly?.Location;
            public static string CallingAssemblyFolder => Path.GetDirectoryName(CallingAssemblyFullPath);
            public static string CallingAssemblyNameWithNoExtension => Path.GetFileNameWithoutExtension(CallingAssemblyFullPath);
        }

        public static class AppConfig
        {
            public static Configuration LoadConfiguration(string name)
            {
                return LoadConfiguration(Utility.Runtime.GetAssembly(name));
            }
            public static Configuration LoadConfiguration(Assembly assembly)
            {
                if (assembly != null)
                {
                    return ConfigurationManager.OpenExeConfiguration(assembly.Location);
                }
                return null;
            }

            public static Dictionary<string, string> ReadAppSettings(string assemblyName) => ReadAppSettings(Runtime.GetAssembly(assemblyName));
            public static Dictionary<string, string> ReadAppSettings(Assembly assembly)
            {
                var dataDict = new Dictionary<string, string>();

                var customAssesmblyConfig = LoadConfiguration(assembly);
                customAssesmblyConfig?.AppSettings.Settings.AllKeys.Iter(k =>
                {
                    Console.WriteLine($"Reading AppSettings from custom project level[{assembly.ManifestModule.Name}] : Key: {k}, Value: {customAssesmblyConfig.AppSettings.Settings[k].Value}");
                    dataDict.AddOrUpdate(k, customAssesmblyConfig.AppSettings.Settings[k].Value);
                });
                return dataDict;
            }

            private static Configuration _configurationCaller;
            public static Configuration ConfigurationCaller
            {
                get
                {
                    if (_configurationCaller == null)
                    {
                        _configurationCaller = LoadConfiguration(Runtime.CallingAssembly);
                    }
                    return _configurationCaller;
                }
            }

            private static Dictionary<string, string> _appsettingsCallerAssembly;
            public static Dictionary<string, string> AppSettingsCallerAssembly
            {
                get
                {
                    if (_appsettingsCallerAssembly == null)
                    {
                        _appsettingsCallerAssembly = ReadAppSettings(Runtime.CallingAssembly);
                    }
                    return _appsettingsCallerAssembly;
                }
            }
        }

        public static Size TabletWindowSize { get; set; } = new Size(768, 1024);
        public static Size MobileWindowSize { get; set; } = new Size(375, 667);

        public static void InitializeFramework()
        {
            TestContextHelper.CreateTestContext();
        }

    }
}
