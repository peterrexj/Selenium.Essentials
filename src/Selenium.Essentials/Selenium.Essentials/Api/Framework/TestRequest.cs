using Selenium.Essentials.Model;
using Selenium.Essentials.Utilities.Extensions;
using Selenium.Essentials.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Essentials.Api.Framework
{
    public class TestRequest
    {
        public Uri Uri { get; private set; }
        public string JsonBody { get; private set; }
        public ParameterCollection QueryParams { get; private set; }
        public ParameterCollection PostParams { get; private set; }
        public HeaderCollection Headers { get; private set; }
        public CookieCollection Cookies { get; private set; }

        public TestRequest(Uri baseUri, string path)
        {
            Init();
            InitUri(baseUri, path);
        }
        private AuthenticationHeaderValue HeaderAuthentication { get; set; }

        private void Init()
        {
            Headers = new HeaderCollection();
            QueryParams = new ParameterCollection();
            PostParams = new ParameterCollection();
            Cookies = new CookieCollection();
        }
        private void InitUri(Uri baseUri, string path)
        {
            if (baseUri == null)
            {
                throw new ArgumentNullException("baseUri needs to be specified");
            }

            if (string.IsNullOrWhiteSpace(path))
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

        public virtual TestRequest RemoveAllHeaders()
        {
            Headers.Clear();
            return this;
        }
        public virtual TestRequest AddHeaders(HeaderCollection headers)
        {
            Headers.AddRange(headers);
            return this;
        }
        public virtual TestRequest AddingCookies(CookieCollection cookies)
        {
            Cookies = cookies;
            return this;
        }

        public virtual TestRequest AddAuthorizationHeader(string username, string password)
        {
            HeaderAuthentication = new AuthenticationHeaderValue(
                   "Basic",
                   Convert.ToBase64String(
                       Encoding.ASCII.GetBytes($"{username}:{password}")));
            return this;
        }

        public virtual TestRequest SetQueryParamsAsHeader(ParameterCollection values)
        {
            var nameValueCollection = values.Select(kvp => new KeyValuePair<string, string>(kvp.Key, kvp.Value as string)).ToList();
            foreach (var item in nameValueCollection)
            {
                Headers.Add(new TestHeader(item.Key, item.Value));
            }
            return this;
        }
        public virtual TestRequest SetQueryParams(ParameterCollection values)
        {
            var nameValueCollection = values.Select(kvp => new KeyValuePair<string, string>(kvp.Key, kvp.Value as string)).ToList();
            var content = new FormUrlEncodedContent(nameValueCollection);
            var queryStr = content.ReadAsStringAsync().Result;

            // http://stackoverflow.com/questions/21640/net-get-protocol-host-and-port
            var schemeHostAndPortPart = Uri.GetLeftPart(UriPartial.Authority);

            var uri = new Uri(schemeHostAndPortPart + Uri.AbsolutePath + "?" + queryStr);

            QueryParams = values;
            Uri = uri;

            return this;
        }
        public TestRequest SetContent(ParameterCollection parameters)
        {
            PostParams = parameters;
            return this;
        }
        public virtual TestRequest SetJsonBody(string json)
        {
            JsonBody = json;
            return this;
        }
        public TestResponse Delete() => DeleteAsync().Result;
        public virtual async Task<TestResponse> DeleteAsync() => await SendRequestAsync(HttpMethod.Delete);
        public TestResponse Get() => GetAsync().Result;
        public virtual async Task<TestResponse> GetAsync() => await SendRequestAsync(HttpMethod.Get);
        public virtual TestResponse Download(string filePath) => DownloadAsync(filePath).Result;

        public virtual async Task<TestResponse> DownloadAsync(string filePath) =>
            await SendRequestAsync(HttpMethod.Get, requestToDownloadFile: filePath);

        public virtual TestResponse Post()
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

        public virtual async Task<TestResponse> PostAsync() => await SendRequestAsync(HttpMethod.Post);

        public async Task<TestResponse> SendRequestAsync(HttpMethod httpMethod, string requestToDownloadFile = null)
        {
            var httpClientHandler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                CookieContainer = new CookieContainer(),
                UseCookies = true
            };

            //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            httpClientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
            {
                //Log($"Problem with the Certificate: {cert.Subject}");
                //Log($"Sender: {sender}");
                //Log($"cert: {cert}");
                //Log($"chain: {chain}");
                //Log($"sslPolicyErrors: {sslPolicyErrors}");
                return true;
            };


            //if (UtilityCoreConfig.NtmlAuthentication)
            //{
            //    httpClientHandler.UseDefaultCredentials = true;
            //}

            var client = new HttpClient(httpClientHandler);

            if (HeaderAuthentication != null)
            {
                client.DefaultRequestHeaders.Authorization = HeaderAuthentication;
            }


            //if (TimeOut.Ticks > 0)
            //{
            //    client.Timeout = TimeOut;
            //}
            //else
            //{
            //    client.Timeout = TimeSpan.FromSeconds(AppConfig.DefaultWaitTimeoutPeriod);
            //}

            foreach (var kvp in Headers)
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation(kvp.Key, kvp.Value);
            }

            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            foreach (Cookie cookie in Cookies)
            {
                httpClientHandler.CookieContainer.Add(cookie);
            }

            HttpResponseMessage httpResponseMessage = null;

            if (httpMethod == HttpMethod.Post)
            {
                if (string.IsNullOrEmpty(JsonBody))
                {
                    var content = new FormUrlEncodedContent(PostParams.Select(kvp => new KeyValuePair<string, string>(kvp.Key, (string)kvp.Value)));
                    httpResponseMessage = await client.PostAsync(Uri.AbsoluteUri, content);
                }
                else
                {
                    var request = new HttpRequestMessage(httpMethod, Uri);
                    request.Content = new StringContent(JsonBody, Encoding.UTF8, "application/json");
                    httpResponseMessage = await client.SendAsync(request);
                }
            }
            else if (httpMethod == HttpMethod.Get)
            {
                Console.WriteLine($"Requesting GET: {Uri.AbsoluteUri}");
                httpResponseMessage = requestToDownloadFile.IsEmpty() ? await client.GetAsync(Uri) : await client.GetAsync(Uri, HttpCompletionOption.ResponseHeadersRead);
            }
            else if (httpMethod == HttpMethod.Delete)
            {
                if (string.IsNullOrEmpty(JsonBody))
                {
                    Console.WriteLine($"Requesting DELETE: {Uri.AbsoluteUri}");
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
                return new TestResponse()
                {
                    ResponseCode = httpResponseMessage.StatusCode,
                    Cookies = httpClientHandler.CookieContainer.GetCookies(uri),
                    ResponseBody = null,
                    HttpResponseMessage = httpResponseMessage,
                };
            }

            var result = await httpResponseMessage.Content.ReadAsStringAsync();

            var response = new TestResponse()
            {
                ResponseCode = httpResponseMessage.StatusCode,
                Cookies = httpClientHandler.CookieContainer.GetCookies(uri),
                ResponseBody = new TestBody(result),
                HttpResponseMessage = httpResponseMessage,
            };

            if (response.ResponseCode != HttpStatusCode.OK && response.ResponseCode != HttpStatusCode.Accepted)
            {
                Console.WriteLine($"Resposne status: {response.ResponseCode}, ResponseMessage: {response.ResponseBody.StringContent}");
            }

            return response;
        }
    }
}
