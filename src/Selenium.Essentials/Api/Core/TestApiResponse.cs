using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Selenium.Essentials
{
    /// <summary>
    /// Api Response
    /// </summary>
    public class TestApiResponse
    {
        public TestApiResponse() { }
        public TestApiResponse(HttpResponseMessage httpResponseMessage)
        {
            HttpResponseMessage = httpResponseMessage;
        }

        /// <summary>
        /// Response code
        /// </summary>
        public HttpStatusCode ResponseCode { get; set; }

        /// <summary>
        /// Response message received from the request
        /// </summary>
        public HttpResponseMessage HttpResponseMessage { get; set; }

        /// <summary>
        /// Cookies received as part of the request
        /// </summary>
        public CookieCollection Cookies { get; set; }

        /// <summary>
        /// Response body container
        /// </summary>
        public TestApiBody ResponseBody { get; set; }

        /// <summary>
        /// Assertion on the status code of the request
        /// </summary>
        public void AssertResponseStatusForSuccess()
        {
            var passStatus = new[] { HttpStatusCode.OK, HttpStatusCode.Accepted };
            passStatus
                .Contains(this.HttpResponseMessage.StatusCode)
                .Should()
                .BeTrue($"The response from the server resulted with status code: {this.HttpResponseMessage.StatusCode} with reason: {this.HttpResponseMessage.ReasonPhrase}");
        }
    }
}
