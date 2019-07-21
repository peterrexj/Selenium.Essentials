using FluentAssertions;
using NUnit.Framework;
using Selenium.Essentials.Api;
using Selenium.Essentials.IntegrationTests.Definitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials.IntegrationTests.Tests
{
    class OpenWeatherApiTests : TestApiBase
    {
        OpenWeatherService _openWeatherService;
        
        [SetUp]
        public void Setup()
        {
            Utility.InitializeFramework();
            _openWeatherService = new OpenWeatherService();
        }

        [TestCase("Sydney")]
        public void OpenWeatherByCityName(string city)
        {
            var forcast = _openWeatherService.GetForcastWeatherByCityName(city, OpenWeatherService.ResponseFormat.Json);
            var responseCodeWeatherApp = (string)forcast.cod;
            responseCodeWeatherApp.Should().Be("200", $"The weather app was not able to get forcast for city: {city}");
        }
    }
}
