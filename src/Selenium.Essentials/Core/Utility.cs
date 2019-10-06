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
    /// <summary>
    /// General utility class
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Utility to access runtime
        /// </summary>
        public static class Runtime
        {
            /// <summary>
            /// Get executing assembly
            /// </summary>
            public static Assembly ExecutingAssembly => Assembly.GetExecutingAssembly();

            /// <summary>
            /// Get executing folder
            /// </summary>
            public static string ExecutingFolder => Path.GetDirectoryName(ExecutingAssembly.Location);

            /// <summary>
            /// Get logs storage folder (executing folder + Logs)
            /// </summary>
            public static string LogOutputFolder { get; set; } = Path.Combine(ExecutingFolder, "Logs");

            private static ILog _logger;
            /// <summary>
            /// Logging interface to log all your details.
            /// This interface can be implemented in your project and passed into Utility.InitializeFramework(..)
            /// which help you in custom logging.
            /// By Default it will log details into Console
            /// </summary>
            public static ILog Logger
            {
                get
                {
                    if (_logger == null)
                    {
                        _logger = new StandardOutputLogger();
                    }
                    return _logger;
                }
                internal set
                {
                    _logger = value;
                }
            }

            /// <summary>
            /// Get the assembly based on the name provided
            /// The assembly is fetched from the stacktrace information and not by reading the assembly folder
            /// </summary>
            /// <param name="name">name of the assembly to fetch</param>
            /// <returns>the assembly </returns>
            public static Assembly GetAssembly(string name) => new StackTrace().GetFrames()
                .Select(x => x?.GetMethod().ReflectedType?.Assembly).Distinct()
                .FirstOrDefault(x => x != null && (x.ManifestModule.Name.EqualsIgnoreCase(name) || 
                                    Path.GetFileNameWithoutExtension(x.ManifestModule.Name).EqualsIgnoreCase(name)));

            /// <summary>
            /// Get the calling assembly of the current invoking method from the stack trace
            /// </summary>
            public static Assembly CallingAssembly => new StackTrace().GetFrames()
                .Select(x => x?.GetMethod().ReflectedType?.Assembly).Distinct()
                .LastOrDefault(x => x != null && x.GetReferencedAssemblies().Any(y => y.FullName == ExecutingAssembly.FullName));

            /// <summary>
            /// Get the caller methodbase of the current inoking method from the stack trace
            /// </summary>
            public static MethodBase CallerMethod => new StackTrace().GetFrames()
                .Where(x => x?.GetMethod().ReflectedType?.Assembly.FullName == CallingAssembly.FullName)
                .Select(x => x.GetMethod())
                .Distinct()
                .LastOrDefault();

            /// <summary>
            /// Get the caller method name of the current invoking method from the stack trace
            /// </summary>
            /// <returns></returns>
            public static string GetCallerMethodName() => CallerMethod.Name;

            /// <summary>
            /// Get the caller type of the current invoking method from the stack trace
            /// </summary>
            public static Type CallerType => CallerMethod.DeclaringType;

            /// <summary>
            /// Get the assembly full path of the current invoking assembly from the stack trace
            /// </summary>
            public static string CallingAssemblyFullPath => CallingAssembly?.Location;

            /// <summary>
            /// Get the assembly folder of the current invoking assembly from the stack trace
            /// </summary>
            public static string CallingAssemblyFolder => Path.GetDirectoryName(CallingAssemblyFullPath);

            /// <summary>
            /// Get the assembly name with no extention of the current invoking assembly from the stack trace
            /// </summary>
            public static string CallingAssemblyNameWithNoExtension => Path.GetFileNameWithoutExtension(CallingAssemblyFullPath);

            /// <summary>
            /// Determine if the current execution is in debug mode based on debugger attached property
            /// </summary>
            public static bool IsInDebugMode => System.Diagnostics.Debugger.IsAttached;
        }


        /// <summary>
        /// Application configuration information
        /// </summary>
        public static class AppConfig
        {

            /// <summary>
            /// Load the assembly configuration based on the assembly name
            /// </summary>
            /// <param name="name">name of the assembly to load the configuration</param>
            /// <returns>configuration information of the assembly</returns>
            public static Configuration LoadConfiguration(string name)
            {
                return LoadConfiguration(Utility.Runtime.GetAssembly(name));
            }

            /// <summary>
            /// Load the assembly configuration based on the assembly
            /// </summary>
            /// <param name="assembly">Assembly to load the configuration</param>
            /// <returns></returns>
            public static Configuration LoadConfiguration(Assembly assembly)
            {
                if (assembly != null)
                {
                    return ConfigurationManager.OpenExeConfiguration(assembly.Location);
                }
                return null;
            }

            /// <summary>
            /// Read the AppSettings from the assembly configuration
            /// </summary>
            /// <param name="assemblyName">Assembly name to load the configuration</param>
            /// <returns>Dictinary with key value pair of the AppSettings</returns>
            public static Dictionary<string, string> ReadAppSettings(string assemblyName) => ReadAppSettings(Runtime.GetAssembly(assemblyName));
            public static Dictionary<string, string> ReadAppSettings(Assembly assembly)
            {
                var dataDict = new Dictionary<string, string>();

                var customAssesmblyConfig = LoadConfiguration(assembly);
                customAssesmblyConfig?.AppSettings.Settings.AllKeys.Iter(k =>
                {
                    Utility.Runtime.Logger.Log($"Reading AppSettings from custom project level[{assembly.ManifestModule.Name}] : Key: {k}, Value: {customAssesmblyConfig.AppSettings.Settings[k].Value}");
                    dataDict.AddOrUpdate(k, customAssesmblyConfig.AppSettings.Settings[k].Value);
                });
                return dataDict;
            }

            private static Configuration _configurationCaller;

            /// <summary>
            /// Configuration of the caller assembly
            /// </summary>
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

            /// <summary>
            /// AppSettings of the caller assembly
            /// </summary>
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
            logger = logger ?? new StandardOutputLogger();
            Runtime.Logger = logger;

            TestContextHelper.CreateTestContext();
        }
    }
}
