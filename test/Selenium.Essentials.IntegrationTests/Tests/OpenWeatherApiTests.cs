using NUnit.Framework;
using Pj.Library;
using Selenium.Essentials.IntegrationTests.Definitions;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using TestAny.Essentials.Api;
using TestAny.Essentials.Core;

namespace Selenium.Essentials.IntegrationTests.Tests
{
    class OpenWeatherApiTests : TestApiBase
    {
        OpenWeatherService _openWeatherService;
        
        [SetUp]
        public void Setup()
        {
            TestAnyAppConfig.InitializeFramework();
            _openWeatherService = new OpenWeatherService();
        }

        [TestCase("Sydney")]
        public void OpenWeatherByCityName(string city)
        {
            var forcast = _openWeatherService.GetForcastWeatherByCityName(city, OpenWeatherService.ResponseFormat.Json);
            var responseCodeWeatherApp = (string)forcast.cod;
            Assert.IsTrue(responseCodeWeatherApp == "200", $"The weather app was not able to get forcast for city: {city}");
        }

        [TestCase("test1.jpg", "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c2/Ritratto_di_papa_Giovanni_Paolo_II_%281984_–_edited%29.jpg/220px-Ritratto_di_papa_Giovanni_Paolo_II_%281984_–_edited%29.jpg")]
        [TestCase("test2.jpg", "https://upload.wikimedia.org/wikipedia/commons/3/37/Beato_Tomás_Reggio.jpg")]
        [TestCase("test3.png", "https://upload.wikimedia.org/wikipedia/commons/thumb/1/1b/Louis-Zéphirin_Moreau.png/220px-Louis-Zéphirin_Moreau.png")]
        [TestCase("test4.jpg", "https://upload.wikimedia.org/wikipedia/en/d/d1/Leonellasgorbati.jpg")]
        [TestCase("test5.jpg", "https://upload.wikimedia.org/wikipedia/commons/thumb/f/f4/Karol_Wojtyla_image_(cropped).jpg/170px-Karol_Wojtyla_image_(cropped).jpg")]
        public void Should_Be_Able_To_Download(string localfilePath, string path)
        {
            localfilePath = Path.Combine((Pj.Library.PjUtility.Runtime.ExecutingFolder), localfilePath);
            IoHelper.DeleteFile(localfilePath);
            var res = new TestApiHttp()
                .OpenFullUrl(path)
                .AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36")
                .Download(localfilePath);

            res.AssertResponseStatusForSuccess();
            Assert.IsTrue(File.Exists(localfilePath));
        }
    }
}
