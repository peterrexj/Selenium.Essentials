using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Selenium.Essentials.Api.Framework
{
    public class TestResponse
    {
        /// <summary>
        /// This is the body of the response
        /// </summary>
        public TestBody ResponseBody { get; set; }

        /// <summary>
        /// The HTTP response code (eg 200 etc)
        /// </summary>
        public HttpStatusCode ResponseCode { get; set; }

        /// <summary>
        /// The underlying System.Net.HttpResponseMessage representation of the response. 
        /// This can be useful to obtain further information on the request if needed.
        /// </summary>
        public HttpResponseMessage HttpResponseMessage { get; set; }
        public CookieCollection Cookies { get; set; }
        public TestResponse()
        {

        }

        public TestResponse(HttpResponseMessage httpResponseMessage)
        {
            HttpResponseMessage = httpResponseMessage;
        }
    }
}
