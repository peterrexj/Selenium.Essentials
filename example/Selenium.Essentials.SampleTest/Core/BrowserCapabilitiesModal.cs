using Pj.Library;
using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials.SampleTest.Core
{
    /// <summary>
    /// Model to contain the browser capabilities
    /// </summary>
    public class BrowserCapabilitiesModal
    {
        public string CapabilityName { get; set; }
        public string BrowserName { get; set; }
        public string Version { get; set; }
        public string ScreenResolution { get; set; }
        public string AppiumVersion { get; set; }
        public string DeviceName { get; set; }
        public string DeviceOrientation { get; set; }
        public string PlatformVersion { get; set; } 
        public string PlatformName { get; set; }
        public string AutomationName { get; set; }

        public Dictionary<string, object> ToCustomDictionary()
        {
            var resultCollection = new Dictionary<string, object>();
            //if (BrowserName.HasValue())
            //{
            //    resultCollection.Add("browserName", BrowserName);
            //}
            //if (PlatformName.HasValue())
            //{
            //    resultCollection.Add("platform", PlatformName);
            //}
            //if (Version.HasValue())
            //{
            //    resultCollection.Add("version", Version);
            //}

            if (ScreenResolution.HasValue()) resultCollection.Add("screenResolution", ScreenResolution); 
            if (DeviceName.HasValue()) resultCollection.Add("appium:deviceName", DeviceName); 
            if (PlatformVersion.HasValue()) resultCollection.Add("appium:platformVersion", PlatformVersion); 
            if (AutomationName.HasValue()) resultCollection.Add("appium:automationName", AutomationName); 
            //if (AppiumVersion.HasValue()) resultCollection.Add("appiumVersion", AppiumVersion); 
            //if (DeviceOrientation.HasValue()) resultCollection.Add("deviceOrientation", DeviceOrientation); 

            //if (PlatformName.HasValue()) {
            //    resultCollection.Add("platformName", PlatformName);
            //}

            return resultCollection;
        }
    }
}
