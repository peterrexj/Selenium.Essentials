using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials
{
    public class TestApiHttp
    {
        public Uri EnvironmentUri { get; private set; }
        public HeaderCollection CommonHeaders { get; set; }

        public TestApiHttp SetEnvironment(string environment)
        {
            EnvironmentUri = new Uri(environment);
            return this;
        }

        public TestApiHttp()
        {
            CommonHeaders = new HeaderCollection {
                new TestApiHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36"),
                new TestApiHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8"),
                new TestApiHeader("Accept-Encoding", "gzip, deflate"),
                new TestApiHeader("Accept-Language", "en-GB,en-US;q=0.9,en;q=0.8"),
                new TestApiHeader("Cache-Control", "max-age=0"),
                new TestApiHeader("Upgrade-Insecure-Requests", "1"),
                //new TestHeader("Host", "dims.cba")
            };
        }

        public TestApiRequest PrepareRequest(string path = "")
        {
            var request = new TestApiRequest(EnvironmentUri, path);
            request.AddHeaders(CommonHeaders);

            if (LoginResponse != null)
            {
                request.AddingCookies(LoginResponse.Cookies);
            }

            return request;
        }
        public TestApiResponse LoginResponse { get; set; }

        public void Login()
        {

        }
    }
}
