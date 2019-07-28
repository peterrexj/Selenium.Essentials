using FluentAssertions;
using NUnit.Framework;
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
            Utility.InitializeFramework();
        }

        [TestCase]
        [PayloadDataJson(@"Data\dataSerializeToDictionary.json")]
        public void JsonToDictionaryShouldConvert()
        {
            var dictionary = SerializationHelper.JsonToDictionary(JsonPayloadContent);
            dictionary.Count.Should().Be(10, "The total number of property does not match result in dictionary");
        }
    }
}
