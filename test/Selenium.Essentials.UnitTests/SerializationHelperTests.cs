using FluentAssertions;
using NUnit.Framework;
using Pj.Library;
using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials.UnitTests
{
    public class SerializationHelperTests : TestApiBase
    {
        [SetUp]
        public void Setup()
        {
            SeAppConfig.InitializeFramework();
        }

        [TestCase]
        [PayloadDataJson(@"Data/dataSerializeToDictionary.json")]
        public void JsonToDictionaryShouldConvert()
        {
            var dictionary = SerializationHelper.ConvertComplexJsonDataToDictionary(JsonPayloadContent);
            dictionary.Count.Should().Be(16, "The total number of property does not match result in dictionary");
        }
    }
}
