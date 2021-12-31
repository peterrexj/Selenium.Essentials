using FluentAssertions;
using NUnit.Framework;
using Pj.Library;
using System;
using System.Collections.Generic;
using System.Text;
using TestAny.Essentials.Api;
using TestAny.Essentials.Core;
using TestAny.Essentials.Core.Attributes;

namespace Selenium.Essentials.UnitTests
{
    public class SerializationHelperTests : TestApiBase
    {
        [SetUp]
        public void Setup()
        {
            TestAnyAppConfig.InitializeFramework();
        }

        [TestCase]
        [PayloadDataJson(@"Data/dataSerializeToDictionary.json")]
        public void JsonToDictionaryShouldConvert()
        {
            var dictionary = JsonHelper.ConvertComplexJsonDataToDictionary(JsonPayloadContent);
            dictionary.Count.Should().Be(16, "The total number of property does not match result in dictionary");
        }
    }
}
