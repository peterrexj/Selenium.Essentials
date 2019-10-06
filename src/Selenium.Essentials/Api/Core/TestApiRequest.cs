using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Essentials
{
    /// <summary>
    /// Request class for the Api test
    /// </summary>
    public class TestApiRequest
    {
        public Uri Uri { get; private set; }
        public string JsonBody { get; private set; }
        public string Body { get; private set; }
        public ParameterCollection QueryParams { get; private set; }
        public ParameterCollection PostParams { get; private set; }
        public HeaderCollection Headers { get; private set; }
        public CookieCollection Cookies { get; private set; }
        public bool NtmlAuthentication { get; private set; }
        private AuthenticationHeaderValue HeaderAuthentication { get; set; }

        public TestApiRequest(Uri baseUri, string path)
        {
            Init();
            InitUri(baseUri, path);
        }

        private void Init()
        {
            Headers = new HeaderCollection();
            QueryParams = new ParameterCollection();
            PostParams = new ParameterCollection();
            Cookies = new CookieCollection();
            _handleSslCertificateErrors = true;
            _logSslCertificateErrors = false;
        }
        private void InitUri(Uri baseUri, string path)
        {
            baseUri.Should().NotBeNull("The baseUri is required. Call SetEnvironment method or set EnvironmentUri property on TestApiHttp object");

            if (path.IsEmpty())
            {
                Uri = baseUri;
                return;
            }

            if (baseUri.AbsoluteUri.Contains('?'))
            {
                throw new ArgumentException("Cannot append a path when the base URI contains query parameters.");
            }

            Uri = new Uri(baseUri, path);
        }

        /// <summary>
        /// Remove all headers
        /// </summary>
        /// <returns></returns>
        public virtual TestApiRequest RemoveAllHeaders()
        {
            Headers.Clear();
            return this;
        }

        /// <summary>
        /// Add a header required for making the request
        /// </summary>
        /// <param name="key">key or name of the header</param>
        /// <param name="value">value of the header</param>
        /// <returns></returns>
        public virtual TestApiRequest AddHeader(string key, string value)
        {
            if (Headers.Any(tHead => tHead.Key.EqualsIgnoreCase(key)))
            {
                Headers.Remove(Headers.First(tHead => tHead.Key.EqualsIgnoreCase(key)));
            }
            Headers.Add(new TestApiHeader(key, value));
            return this;
        }

        /// <summary>
        /// Add a collection of headers required for making the request
        /// </summary>
        /// <param name="headers">header collection</param>
        /// <returns></returns>
        public virtual TestApiRequest AddHeaders(HeaderCollection headers)
        {
            Headers.AddRange(headers);
            return this;
        }

        /// <summary>
        /// Add basic authentication information if required for making the api request.
        /// This depends on your api request
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public virtual TestApiRequest AddBasicAuthorizationHeader(string username, string password)
        {
            HeaderAuthentication = new AuthenticationHeaderValue(
                   "Basic",
                   Convert.ToBase64String(
                       Encoding.ASCII.GetBytes($"{username}:{password}")));
            return this;
        }

        /// <summary>
        /// Add the default headers which is required to make a web request. 
        /// Web request are normal page request that are made from a browser
        /// </summary>
        /// <returns></returns>
        public virtual TestApiRequest AddDefaultWebHeaders()
        {
            var defaultWebHeaders = new HeaderCollection {
                new TestApiHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36"),
                new TestApiHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8"),
                new TestApiHeader("Accept-Encoding", "gzip, deflate"),
                new TestApiHeader("Accept-Language", "en-GB,en-US;q=0.9,en;q=0.8"),
                new TestApiHeader("Cache-Control", "max-age=0"),
                new TestApiHeader("Upgrade-Insecure-Requests", "1"),
            };
            Headers.AddRange(defaultWebHeaders);
            return this;
        }

        /// <summary>
        /// Add cookie collection
        /// </summary>
        /// <param name="cookies"></param>
        /// <returns></returns>
        public virtual TestApiRequest AddingCookies(CookieCollection cookies)
        {
            Cookies = cookies;
            return this;
        }

        /// <summary>
        /// Set the NTML (Windows authentication) before making api request
        /// </summary>
        /// <param name="useNtml"></param>
        /// <returns></returns>
        public virtual TestApiRequest SetNtmlAuthentication(bool useNtml = true)
        {
            NtmlAuthentication = useNtml;
            return this;
        }

        /// <summary>
        /// Sets the version of the Secure Sockets Layer (SSL) or Transport Layer Security (TLS) protocol to use for new connections.
        /// Example: SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12
        /// </summary>
        /// <param name="securityProtocolType">Supported security protocol(s)</param>
        /// <returns></returns>
        public virtual TestApiRequest SetSecurityProtocol(SecurityProtocolType securityProtocolType)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            return this;
        }

        /// <summary>
        /// Sets the parameters as header for the api request
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public virtual TestApiRequest SetQueryParamsAsHeader(ParameterCollection values)
        {
            var nameValueCollection = values.Select(kvp => new KeyValuePair<string, string>(kvp.Key, kvp.Value as string)).ToList();
            foreach (var item in nameValueCollection)
            {
                Headers.Add(new TestApiHeader(item.Key, item.Value));
            }
            return this;
        }

        /// <summary>
        /// Set the query parameters
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public virtual TestApiRequest SetQueryParams(ParameterCollection values)
        {
            var nameValueCollection = values.Select(kvp => new KeyValuePair<string, string>(kvp.Key, kvp.Value as string)).ToList();
            var content = new FormUrlEncodedContent(nameValueCollection);
            var queryStr = content.ReadAsStringAsync().Result;
            var schemeHostAndPortPart = Uri.GetLeftPart(UriPartial.Authority);
            var uri = new Uri(schemeHostAndPortPart + Uri.AbsolutePath + "?" + queryStr);

            QueryParams = values;
            Uri = uri;

            return this;
        }

        /// <summary>
        /// Set the POST parameters
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual TestApiRequest SetContent(ParameterCollection parameters)
        {
            PostParams = parameters;
            return this;
        }

        /// <summary>
        /// Set Json body for the POST operation
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public virtual TestApiRequest SetJsonBody(string json)
        {
            JsonBody = json;
            return this;
        }

        /// <summary>
        /// Set default body for the POST operation
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public virtual TestApiRequest SetBody(string body)
        {
            Body = body;
            return this;
        }

        #region Operations
        /// <summary>
        /// Makes a Delete request (sync)
        /// </summary>
        /// <returns></returns>
        public TestApiResponse Delete() => DeleteAsync().Result;

        /// <summary>
        /// Makes a Delete request (async)
        /// </summary>
        /// <returns></returns>
        public virtual async Task<TestApiResponse> DeleteAsync() => await SendRequestAsync(HttpMethod.Delete);

        /// <summary>
        /// Makes a Get request (sync)
        /// </summary>
        /// <returns></returns>
        public TestApiResponse Get() => GetAsync().Result;

        /// <summary>
        /// Makes a Get request (async)
        /// </summary>
        /// <returns></returns>
        public virtual async Task<TestApiResponse> GetAsync() => await SendRequestAsync(HttpMethod.Get);

        /// <summary>
        /// Makes a Download request (sync)
        /// </summary>
        /// <param name="filePath">Path to save the file</param>
        /// <returns></returns>
        public virtual TestApiResponse Download(string filePath) => DownloadAsync(filePath).Result;

        /// <summary>
        /// Makes a Download request (async)
        /// </summary>
        /// <param name="filePath">Path to save the file</param>
        /// <returns></returns>
        public virtual async Task<TestApiResponse> DownloadAsync(string filePath) => await SendRequestAsync(HttpMethod.Get, requestToDownloadFile: filePath);

        /// <summary>
        /// Makes a Post request (sync)
        /// </summary>
        /// <returns></returns>
        public virtual TestApiResponse Post()
        {
            try
            {
                return PostAsync().Result;
            }
            catch (Exception ex)
            {
                if (!ex.ToString().Contains("A task was canceled"))
                {
                    throw ex;
                }
                else
                {
                    throw new Exception($"The query timed out performing the action with a message 'A task was canceled from Server'");
                }
            }
        }

        /// <summary>
        /// Makes a Post request (async)
        /// </summary>
        /// <returns></returns>
        public virtual async Task<TestApiResponse> PostAsync() => await SendRequestAsync(HttpMethod.Post);

        /// <summary>
        /// Makes a Put request (sync)
        /// </summary>
        /// <returns></returns>
        public virtual TestApiResponse Put()
        {
            try
            {
                return PutAsync().Result;
            }
            catch (Exception ex)
            {
                if (!ex.ToString().Contains("A task was canceled"))
                {
                    throw ex;
                }
                else
                {
                    throw new Exception($"The query timed out performing the action with a message 'A task was canceled from Server'");
                }
            }
        }

        /// <summary>
        /// Makes a Put request (async)
        /// </summary>
        /// <returns></returns>
        public virtual async Task<TestApiResponse> PutAsync() => await SendRequestAsync(HttpMethod.Put);
        #endregion

        private bool _handleSslCertificateErrors;
        private bool _logSslCertificateErrors;

        /// <summary>
        /// Handle the Ssl certificate error during the request
        /// </summary>
        /// <param name="handleSslCertExceptions">True if you want to handle the ssl exception and continue</param>
        /// <param name="logSslCertExceptions">True if you want to log the ssl exception</param>
        /// <returns></returns>
        public virtual TestApiRequest HandleSslCertificateErrors(bool handleSslCertExceptions, bool logSslCertExceptions)
        {
            _handleSslCertificateErrors = handleSslCertExceptions;
            _logSslCertificateErrors = logSslCertExceptions;
            return this;
        }

        /// <summary>
        /// Base handler for making the api request
        /// </summary>
        /// <param name="httpMethod"></param>
        /// <param name="requestToDownloadFile"></param>
        /// <returns></returns>
        private async Task<TestApiResponse> SendRequestAsync(HttpMethod httpMethod, string requestToDownloadFile = null)
        {
            var httpClientHandler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                CookieContainer = new CookieContainer(),
                UseCookies = true
            };

            if (_handleSslCertificateErrors)
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
                {
                    if (_logSslCertificateErrors)
                    {
                        Utility.Runtime.Logger.Log($"Problem with the Certificate: {cert.Subject}");
                        Utility.Runtime.Logger.Log($"Sender: {sender}");
                        Utility.Runtime.Logger.Log($"cert: {cert}");
                        Utility.Runtime.Logger.Log($"chain: {chain}");
                        Utility.Runtime.Logger.Log($"sslPolicyErrors: {sslPolicyErrors}");
                    }
                    return true;
                };
            }

            if (NtmlAuthentication)
            {
                httpClientHandler.UseDefaultCredentials = true;
            }

            var client = new HttpClient(httpClientHandler);

            if (HeaderAuthentication != null)
            {
                client.DefaultRequestHeaders.Authorization = HeaderAuthentication;
            }

            client.Timeout = TimeSpan.FromSeconds(SeAppConfig.DefaultApiResponseTimeoutWaitPeriodInSeconds);

            foreach (var kvp in Headers)
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation(kvp.Key, kvp.Value);
            }

            foreach (Cookie cookie in Cookies)
            {
                httpClientHandler.CookieContainer.Add(cookie);
            }

            HttpResponseMessage httpResponseMessage = null;

            if (httpMethod == HttpMethod.Post)
            {
                if (JsonBody.IsEmpty() && Body.IsEmpty())
                {
                    var content = new FormUrlEncodedContent(PostParams.Select(kvp => new KeyValuePair<string, string>(kvp.Key, (string)kvp.Value)));
                    httpResponseMessage = await client.PostAsync(Uri.AbsoluteUri, content);
                }
                else if (JsonBody.HasValue())
                {
                    var request = new HttpRequestMessage(httpMethod, Uri);
                    request.Content = new StringContent(JsonBody, Encoding.UTF8, "application/json");
                    httpResponseMessage = await client.SendAsync(request);
                }
                else if (Body.HasValue())
                {
                    var request = new HttpRequestMessage(httpMethod, Uri);
                    request.Content = new StringContent(Body, Encoding.UTF8, "text/xml");
                    httpResponseMessage = await client.SendAsync(request);
                }
            }
            else if (httpMethod == HttpMethod.Put)
            {
                if (JsonBody.IsEmpty() && Body.IsEmpty())
                {
                    var content = new FormUrlEncodedContent(PostParams.Select(kvp => new KeyValuePair<string, string>(kvp.Key, (string)kvp.Value)));
                    httpResponseMessage = await client.PutAsync(Uri.AbsoluteUri, content);
                }
                else if (JsonBody.HasValue())
                {
                    var request = new HttpRequestMessage(httpMethod, Uri);
                    request.Content = new StringContent(JsonBody, Encoding.UTF8, "application/json");
                    httpResponseMessage = await client.SendAsync(request);
                }
                else if (Body.HasValue())
                {
                    var request = new HttpRequestMessage(httpMethod, Uri);
                    request.Content = new StringContent(Body, Encoding.UTF8, "text/xml");
                    httpResponseMessage = await client.SendAsync(request);
                }
            }
            else if (httpMethod == HttpMethod.Get)
            {
                Utility.Runtime.Logger.Log($"Requesting GET: {Uri.AbsoluteUri}");
                httpResponseMessage = requestToDownloadFile.IsEmpty() ? await client.GetAsync(Uri) : await client.GetAsync(Uri, HttpCompletionOption.ResponseHeadersRead);
            }
            else if (httpMethod == HttpMethod.Delete)
            {
                if (JsonBody.IsEmpty())
                {
                    Utility.Runtime.Logger.Log($"Requesting DELETE: {Uri.AbsoluteUri}");
                    httpResponseMessage = await client.DeleteAsync(Uri);
                }
                else
                {
                    var request = new HttpRequestMessage(httpMethod, Uri);
                    request.Content = new StringContent(JsonBody, Encoding.UTF8, "application/json");
                    httpResponseMessage = await client.SendAsync(request);
                }
            }

            var schemeHostAndPortPart = Uri.GetLeftPart(UriPartial.Authority);
            var uri = new Uri(schemeHostAndPortPart);

            if (requestToDownloadFile.HasValue())
            {
                StorageHelper.CreateDirectory(requestToDownloadFile);
                if (!File.Exists(requestToDownloadFile))
                {
                    using (Stream streamToReadFrom = await httpResponseMessage.Content.ReadAsStreamAsync())
                    using (Stream output = File.OpenWrite(requestToDownloadFile))
                    {
                        streamToReadFrom.CopyTo(output);
                    }
                }
                return new TestApiResponse()
                {
                    ResponseCode = httpResponseMessage.StatusCode,
                    Cookies = httpClientHandler.CookieContainer.GetCookies(uri),
                    ResponseBody = null,
                    HttpResponseMessage = httpResponseMessage,
                };
            }

            var result = await httpResponseMessage.Content.ReadAsStringAsync();

            var response = new TestApiResponse()
            {
                ResponseCode = httpResponseMessage.StatusCode,
                Cookies = httpClientHandler.CookieContainer.GetCookies(uri),
                ResponseBody = new TestApiBody(result),
                HttpResponseMessage = httpResponseMessage,
            };

            if (response.ResponseCode != HttpStatusCode.OK && response.ResponseCode != HttpStatusCode.Accepted)
            {
                Utility.Runtime.Logger.Log($"Resposne status: {response.ResponseCode}, ResponseMessage: {response.ResponseBody.ContentString}");
            }

            return response;
        }
    }
}
