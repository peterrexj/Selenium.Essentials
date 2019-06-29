using Selenium.Essentials.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials.Api.Framework
{
    public class TestHttp
    {
        public Uri EnvironmentUri { get; private set; }
        public HeaderCollection CommonHeaders { get; set; }

        public TestHttp SetEnvironment(string environment)
        {
            EnvironmentUri = new Uri(environment);
            return this;
        }

        public TestHttp()
        {
            CommonHeaders = new HeaderCollection {
                new TestHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36"),
                new TestHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8"),
                new TestHeader("Accept-Encoding", "gzip, deflate"),
                new TestHeader("Accept-Language", "en-GB,en-US;q=0.9,en;q=0.8"),
                new TestHeader("Cache-Control", "max-age=0"),
                new TestHeader("Upgrade-Insecure-Requests", "1"),
                //new TestHeader("Host", "dims.cba")
            };
        }

        public TestRequest PrepareRequest(string path = "")
        {
            var request = new TestRequest(EnvironmentUri, path);
            request.AddHeaders(CommonHeaders);

            if (LoginResponse != null)
            {
                request.AddingCookies(LoginResponse.Cookies);
            }

            return request;
        }
        public TestResponse LoginResponse { get; set; }

        public void Login()
        {

        }
    }
}
