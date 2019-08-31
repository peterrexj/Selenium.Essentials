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
            CommonHeaders = new HeaderCollection();
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
