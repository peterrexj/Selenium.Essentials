
namespace Selenium.Essentials.IntegrationTests.Definitions
{
    public class OpenWeatherService
    {
        public enum ResponseFormat
        {
            Json,
            Xml,
            Html
        }
        private const string __OpenWeatherDomain = "http://api.openweathermap.org";
        private const string __OpenWeatherApiKey = "246f355cb15be9063138812ffcaa523c";

        public dynamic GetForcastWeatherByCityName(string cityName, ResponseFormat format)
        {
            var route = GetWebQuery(city: cityName,
                            forcast: true,
                            asXml: format == ResponseFormat.Xml,
                            asHtml: format == ResponseFormat.Html);

            return new TestApiHttp()
                .SetEnvironment(__OpenWeatherDomain)
                .PrepareRequest(route)
                .Get()
                .ResponseBody
                .ContentJson;
        }

        private string _contactString(string query) => query.Contains("?") ? "&" : "?";
        private string GetWebQuery(string city, bool forcast, bool asXml, bool asHtml)
        {
            string query = "/data/2.5";

            if (forcast)
            {
                query = $"{query}/forecast";
            }
            if (city.HasValue())
            {
                query = $"{query}{_contactString(query)}q={city}";
            }
            if (asXml)
            {
                query = $"{query}{_contactString(query)}mode=xml";
            }
            if (asHtml)
            {
                query = $"{query}{_contactString(query)}mode=html";
            }
            query = $"{query}{_contactString(query)}APPID={__OpenWeatherApiKey}";

            return query;
        }
    }
}
