using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Selenium.Essentials
{
    public class TestApiResponse
    {
        public TestApiResponse() { }
        public TestApiResponse(HttpResponseMessage httpResponseMessage)
        {
            HttpResponseMessage = httpResponseMessage;
        }

        public HttpStatusCode ResponseCode { get; set; }
        public HttpResponseMessage HttpResponseMessage { get; set; }
        public CookieCollection Cookies { get; set; }
        public TestApiBody ResponseBody { get; set; }

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
