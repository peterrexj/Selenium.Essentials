using System;
using System.Collections.Generic;
using System.Text;
using TestAny.Essentials.Core.Dtos.Api;

namespace TestAny.Essentials.Api
{
    /// <summary>
    /// Base http class for performing api tests
    /// </summary>
    public class TestApiHttp
    {
        /// <summary>
        /// Environment or domain path (example: https://www.yourdomain.com)
        /// </summary>
        public Uri EnvironmentUri { get; private set; }

        /// <summary>
        /// Default headers required for the Api. Headers can be added based on the request you need to make.
        /// </summary>
        public HeaderCollection CommonHeaders { get; set; }

        /// <summary>
        /// Set the environment or domain
        /// </summary>
        /// <param name="environment">example: https://www.yourdomain.com</param>
        /// <returns></returns>
        public TestApiHttp SetEnvironment(string environment)
        {
            EnvironmentUri = new Uri(environment);
            return this;
        }

        public TestApiHttp()
        {
            CommonHeaders = new HeaderCollection();
        }

        /// <summary>
        /// Prepare your api request and you need to pass the route information
        /// Example: new TestApiHttp().SetEnvironment("https://www.yourdomain.com").PrepareRequest("/path/to/my/endpoint")
        /// </summary>
        /// <param name="path">endpoint or the route of your api</param>
        /// <returns></returns>
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

        /// <summary>
        /// This response will hold the cookie information. For example when you perform a login using credentials
        /// and set response received to this property, 
        /// every subsequent request will try to use the cookies which was received for the login, which make all other request authenticated
        /// </summary>
        public TestApiResponse LoginResponse { get; set; }
    }
}
