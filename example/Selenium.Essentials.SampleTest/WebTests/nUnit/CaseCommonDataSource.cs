﻿using NUnit.Framework;
using Pj.Library;
using Selenium.Essentials.SampleTest.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Selenium.Essentials.SampleTest.WebTests.nUnit
{
    public static class CaseCommonDataSource
    {
        /// <summary>
        /// Returns all the browser capabilities as test case source. It also includes the additional parameters that is 
        /// required by the test and will be injected into the test case as parameter
        /// </summary>
        /// <param name="additionalParams"></param>
        /// <returns></returns>
        public static IEnumerable<TestCaseData> BrowserCapabilitiesWithAdditionalParams(string additionalParams)
        {
            if (BrowserCapabilityHelper.CurrentBrowserCapabilities.Any())
            {
                return BrowserCapabilityHelper.CurrentBrowserCapabilities
                    .Select(b => 
                        new TestCaseData(
                            (new string[] { b.CapabilityName }.Union(additionalParams.SplitAndTrim(","))).ToArray()
                            ));
            }
            else
            {
                return new List<TestCaseData>
                {
                    new TestCaseData(
                        ((new string[] { TestUtility.EnvData["DefaultDebugBrowser"] ?? "Chrome" })
                        .Union(additionalParams.SplitAndTrim(",")))
                        .ToArray())
                };
            }
        }

        /// <summary>
        /// Returns all the browser capabilities as test case source.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<TestCaseData> BrowserCapabilities()
        {
            if (BrowserCapabilityHelper.CurrentBrowserCapabilities.Any())
            {
                return BrowserCapabilityHelper.CurrentBrowserCapabilities.Select(b => new TestCaseData(b.CapabilityName));
            }
            else
            {
                return new List<TestCaseData> { new TestCaseData(TestUtility.EnvData["DefaultDebugBrowser"] ?? "Chrome") };
            }
        }
    }
}
