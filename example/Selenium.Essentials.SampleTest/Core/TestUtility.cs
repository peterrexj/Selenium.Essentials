using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Selenium.Essentials.SampleTest
{
    public static class TestUtility
    {
        private static Dictionary<string, string> _envData;

        public static Dictionary<string, string> EnvData
        {
            get
            {
                if (_envData == null)
                {
                    var currentEnv = Utility.AppConfig.AppSettingsCallerAssembly
                        .Where(k => k.Key.EqualsIgnoreCase("Environment"))
                        .FirstOrDefault().Value;
                    var envDataFilePath = Path.Combine(Utility.Runtime.ExecutingFolder, "DataSource", "EnvironmentData", currentEnv, "EnvData.json");
                    if (File.Exists(envDataFilePath))
                    {
                        _envData = SerializationHelper.JsonToDictionary(new PayloadDataJsonAttribute(envDataFilePath).FileContent);
                    }
                    else
                    {
                        _envData = new Dictionary<string, string>();
                    }
                    _envData.Add("Environment", currentEnv);
                }
                return _envData;
            }
        }
    }
}
