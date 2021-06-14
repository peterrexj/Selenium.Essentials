using Pj.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Selenium.Essentials.SampleTest.Core
{
    internal static class BrowserCapabilityHelper
    {
        private static List<BrowserCapabilitiesModal> _browserCapabilitiesModals;
        internal static List<BrowserCapabilitiesModal> CurrentBrowserCapabilities
        {
            get
            {
                if (_browserCapabilitiesModals == null)
                {
                    _browserCapabilitiesModals = new List<BrowserCapabilitiesModal>();

                    var capFiles = Directory.EnumerateFiles(IoHelper.LocalFile(TestUtility.EnvData["PathToBrowserCapabilities"]), "*.json");

                    if (capFiles.Any())
                    {
                        _browserCapabilitiesModals.AddRange(
                            capFiles.SelectMany(f => 
                                SerializationHelper.DeSerializeFromJsonFile<List<BrowserCapabilitiesModal>>(f)).ToList()
                        );
                    }
                }
                return _browserCapabilitiesModals;
            }
        }
    }
}
