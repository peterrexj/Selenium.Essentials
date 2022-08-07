using Newtonsoft.Json;
using Pj.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TestAny.Essentials.Core;
using TestAny.Essentials.Core.Dtos.Api;
using static Pj.Library.PjUtility;

namespace TestAny.Essentials.Api
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
        public bool ContentTypeSetAsEntity { get; private set; }
        public bool NoCache { get; private set; }
        public bool NoTrack { get; private set; }
        public int Timeout { get; private set; } = -1;
        public bool ExtractDomainCookies { get; private set; } = false;
        public bool AutoRedirectMode { get; private set; } = true;
        public bool FilePathUploadMode { get; private set; }
        public List<X509Certificate2> Certificates { get; private set; }
        public IWebProxy Proxy { get; private set; }
        private AuthenticationHeaderValue HeaderAuthentication { get; set; }

        public TestApiRequest(Uri fullPath)
        {
            Uri = fullPath;
            Init();
        }
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
            Certificates = new List<X509Certificate2>();
        }
        private void InitUri(Uri baseUri, string path)
        {
            if (baseUri == null)
            {
                throw new Exception("The baseUri is required. Call SetEnvironment method or set EnvironmentUri property on TestApiHttp object");
            }

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
        /// Remove the header based on the key passed
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual TestApiRequest RemoveHeader(string key)
        {
            var toRemove = Headers.Where(h => h.Key == key).ToList();
            Headers = new HeaderCollection(Headers.Except(toRemove));
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
                new TestApiHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.71 Safari/537.36"),
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
        public virtual TestApiRequest AddCookies(CookieCollection cookies)
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

        private void SetInternalQueryParameters(ParameterCollection values)
        {
            var nameValueCollection = values.Select(kvp => new KeyValuePair<string, string>(kvp.Key, kvp.Value as string)).ToList();
            var content = new FormUrlEncodedContent(nameValueCollection);
            var queryStr = content.ReadAsStringAsync().Result;
            var schemeHostAndPortPart = Uri.GetLeftPart(UriPartial.Authority);
            var uri = new Uri(schemeHostAndPortPart + Uri.AbsolutePath + "?" + queryStr);

            QueryParams = values;
            Uri = uri;
        }
        /// <summary>
        /// Set the query parameters
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public virtual TestApiRequest SetQueryParams(ParameterCollection values)
        {
            SetInternalQueryParameters(values);
            return this;
        }
        /// <summary>
        /// Set the query parameters from the Dictionary
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual TestApiRequest SetQueryParams(IDictionary<string, string> value)
        {
            ParameterCollection values = new();
            foreach (var prop in value.Where(v => v.Value.HasValue()))
            {
                values.AddOrUpdate(prop.Key, prop.Value);
            }
            SetInternalQueryParameters(values);
            return this;
        }
        /// <summary>
        /// Set the query parameters from the oject parameter and using all its parameters
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual TestApiRequest SetQueryParams(object value)
        {
            if (value == null) return this;
            ParameterCollection values = new();
            value.GetPropertyValuesV2().Iter(p => values.AddOrUpdate(p.Key, p.Value));

            SetInternalQueryParameters(values);
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
        /// Set the json content from the object passed
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public virtual TestApiRequest SetJsonBody(object content)
        {
            JsonBody = JsonConvert.SerializeObject(content);
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

        public virtual TestApiRequest SetBodyAsFilePathToUpload(string path)
        {
            if (path.IsEmpty()) throw new Exception("The path should have a value");
            if (!File.Exists(path)) throw new Exception($"The specified file path does not exists [{path}]");
            Body = path.Trim();
            FilePathUploadMode = true;
            return this;
        }

        /// <summary>
        /// Set timeout on the request in seconds. 
        /// Total time to wait a request to get a successful response from the server
        /// </summary>
        /// <param name="waitTimeoutInSeconds">Total time to wait in Seconds</param>
        /// <returns></returns>
        public virtual TestApiRequest SetTimeout(int waitTimeoutInSeconds)
        {
            Timeout = waitTimeoutInSeconds;
            return this;
        }

        /// <summary>
        /// Enable or disable auto redirect request on the server.
        /// Control this if you need to stop redirection and redirect to the next endpoint manually
        /// The response header will contain details where to redirect next (Location) if the response code is 304
        /// </summary>
        /// <param name="mode">true to automatically redirect on the server</param>
        /// <returns></returns>
        public virtual TestApiRequest SetAutoRedirect(bool mode = true)
        {
            AutoRedirectMode = mode;
            return this;
        }

        /// <summary>
        /// Extract domain level cookies from the response from server
        /// </summary>
        /// <param name="extractDomainCookies">true to extract the domain cookies</param>
        /// <returns></returns>
        public virtual TestApiRequest SetDomainCookieExtraction(bool extractDomainCookies = true)
        {
            ExtractDomainCookies = extractDomainCookies;
            return this;
        }

        /// <summary>
        /// With no cache control
        /// </summary>
        /// <param name="withNoCache"></param>
        /// <returns></returns>
        public virtual TestApiRequest SetNoCache(bool withNoCache)
        {
            NoCache = withNoCache;
            return this;
        }

        #region Client Certificate
        private bool? _IsClientCertificateRequired { get; set; }
        private ClientCertificatePathData? _ClientCertificatePathData { get; set; }

        /// <summary>
        /// Set true/false based on the condition the certificate will be added
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual TestApiRequest ClientCertificateRequired(bool value)
        {
            _IsClientCertificateRequired = value;
            return this;
        }

        private bool IsCertificateRequired =>
            _IsClientCertificateRequired.HasValue ? _IsClientCertificateRequired.Value :
            PjUtility.EnvironmentConfig.AppSettingsConfigData.ContainsKey("ClientCertificate_IsCertificateRequired") &&
            PjUtility.EnvironmentConfig.AppSettingsConfigData["ClientCertificate_IsCertificateRequired"].ToBool() == true;

        /// <summary>
        /// Make sure you set the ClientCertificateRequired(true) when adding a certificate. This will help to have a single code run against multiple environments where 
        /// one may require certificate and other may not. Based on the condition the true/false can be set and drive the addition of client certificate
        /// "ClientCertificate": {
        ///   "your custom key": { //for example "StagingCertificate"
        ///     "PathToCertificate": "",
        ///     "Password": "",
        ///     "IsRelativePath": ""
        ///   }
        /// }
        /// </summary>
        /// <param name="clientCertificateKey">the custom key to the certificate which has been described in the description</param>
        /// <returns></returns>
        public virtual TestApiRequest AddClientCertificate(string clientCertificateKey)
        {
            if (_IsClientCertificateRequired.HasValue && _IsClientCertificateRequired.Value == true && clientCertificateKey.HasValue())
            {
                _ClientCertificatePathData = BuildClientCertificateModelData(clientCertificateKey);
                ValidateClientCertificate(_ClientCertificatePathData);
            }
            return this;
        }

        /// <summary>
        /// Make sure you set the ClientCertificateRequired(true) when adding a certificate. This will help to have a single code run against multiple environments where 
        /// one may require certificate and other may not. Based on the condition the true/false can be set and drive the addition of client certificate
        /// </summary>
        /// <param name="clientCertificatePathData"></param>
        /// <returns></returns>
        public virtual TestApiRequest AddClientCertificate(ClientCertificatePathData clientCertificatePathData)
        {
            if (_IsClientCertificateRequired.HasValue && _IsClientCertificateRequired.Value == true)
            {
                _ClientCertificatePathData = clientCertificatePathData;
                ValidateClientCertificate(_ClientCertificatePathData);
            }
            return this;
        }

        /// <summary>
        /// Make sure you set the ClientCertificateRequired(true) when adding a certificate. This will help to have a single code run against multiple environments where 
        /// one may require certificate and other may not. Based on the condition the true/false can be set and drive the addition of client certificate
        /// </summary>
        /// <param name="pathToCertificateFile"></param>
        /// <param name="passwordToCertificate"></param>
        /// <param name="isPathToCertificateFileRelative"></param>
        /// <returns></returns>
        public virtual TestApiRequest AddClientCertificate(string pathToCertificateFile, string passwordToCertificate, bool isPathToCertificateFileRelative)
        {
            if (_IsClientCertificateRequired.HasValue && _IsClientCertificateRequired.Value == true)
            {
                _ClientCertificatePathData = new ClientCertificatePathData
                {
                    Path = pathToCertificateFile,
                    Password = passwordToCertificate,
                    IsRelativePath = isPathToCertificateFileRelative
                };
                ValidateClientCertificate(_ClientCertificatePathData);
            }
            return this;
        }

        private void ValidateClientCertificate(ClientCertificatePathData clientCertificatePathData)
        {
            if (clientCertificatePathData == null)
                throw new Exception("You need to have a client certificate information when trying to set the client certificate");

            if (!clientCertificatePathData.IsPathValid)
                throw new Exception($"Make sure the certificate file exists in [{clientCertificatePathData.Path}]");

            if (clientCertificatePathData.Password.IsEmpty())
                throw new Exception($"You cannot have an empty password for certificate in [{clientCertificatePathData.Path}]");
        }
        private ClientCertificatePathData BuildClientCertificateModelData(string keyFromConfig)
        {
            var keyToPath = $"ClientCertificate_{keyFromConfig}_PathToCertificate";
            var keyToPassword = $"ClientCertificate_{keyFromConfig}_Password";
            var keyToRelativePath = $"ClientCertificate_{keyFromConfig}_IsRelativePath";

            var certData = new ClientCertificatePathData { IsRelativePath = false };

            if (PjUtility.EnvironmentConfig.AppSettingsConfigData.ContainsKey(keyToRelativePath))
            {
                certData.IsRelativePath = PjUtility.EnvironmentConfig.AppSettingsConfigData[keyToRelativePath].ToBool();
            }
            if (PjUtility.EnvironmentConfig.AppSettingsConfigData.ContainsKey(keyToPassword))
            {
                if (PjUtility.EnvironmentConfig.AppSettingsConfigData[keyToPassword].IsEmpty())
                {
                    throw new Exception($"You cannot have an empty password for certificate. Make sure you have password set for this {keyToPassword}");
                }
                certData.Password = PjUtility.EnvironmentConfig.AppSettingsConfigData[keyToPassword];
            }

            if (PjUtility.EnvironmentConfig.AppSettingsConfigData.ContainsKey(keyToRelativePath))
            {
                certData.Path = PjUtility.EnvironmentConfig.AppSettingsConfigData[keyToPath];

                if (certData.IsRelativePath || IoHelper.IsPathRelative(certData.Path)) certData.Path = IoHelper.CombinePath(PjUtility.Runtime.ExecutingFolder, certData.Path);
                Log($"The certificate path for the request {certData.Path}, available status is {IoHelper.Exists(certData.Path)}");
                if (!certData.IsPathValid)
                {
                    throw new Exception($"Make sure the certificate file exists in [{certData.Path}]");
                }
            }
            else
            {
                throw new Exception($"You need to set a file path for this certificate under the config key {keyToPassword}");
            }

            return certData;
        }

        #endregion

        #region Proxy

        private bool? _IsProxyRequired { get; set; }
        private ClientCertificatePathData? _ProxyPathData { get; set; }

        /// <summary>
        /// Set true/false based on the condition the certificate will be added
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual TestApiRequest ProxyRequired(bool value)
        {
            _IsProxyRequired = value;
            return this;
        }

        private bool IsProxyRequired =>
            _IsProxyRequired.HasValue ? _IsProxyRequired.Value :
            PjUtility.EnvironmentConfig.AppSettingsConfigData.ContainsKey("Proxy_IsProxyRequired") &&
            PjUtility.EnvironmentConfig.AppSettingsConfigData["Proxy_IsProxyRequired"].ToBool() == true;

        /// <summary>
        /// Usage: add this configuration to your json file
        /// Proxy {
        ///  <yourkeyname>: {
        ///   UseOnlyInPipeline: true, -- true or false to determine whether the proxy needs to be applied only when ran through a CI pipeline (Add a System Environment variable: IsPipelineExecution to true or false to control the value)
        ///   Host: "", -- host name of the proxy server
        ///   Port: "", -- port to use for the proxy server
        ///   ByPassProxyOnLocal: true, true or false to determine whether to apply the ByPassProxyOnLocal proxy settings
        ///   UseDefaultCredentials: true, true or false to use whether the use the default logged in user credentials, add false to override and apply custom username and password
        ///   UsernameEnvironmentKeyName: "", username to connect to the proxy and it needs to be stored in the environment key with this name (for security reasons not to store this value in the configuration)
        //    PasswordEnvironmentKeyName: "", password to connect to the proxy and to be stored in the environment key of the executing computer. The password can encrypted using pj.library.crypto or use a plain text password
        /// </summary>
        /// <param name="proxyKey"></param>
        /// <returns></returns>
        public virtual TestApiRequest AddProxy(string proxyKey)
        {
            if (!IsProxyRequired) return this;

            var keyToUseInTeamcity = $"Proxy_{proxyKey}_UseOnlyInPipeline";

            if (PjUtility.EnvironmentConfig.ReadConfigData(keyToUseInTeamcity,
                throwErrorWhenNotFound: false, returnNullWhenNotFound: true).ToBool() == true &&
                !PjUtility.Runtime.IsPipelineExecution)
            {
                return this;
            }

            var keyToHost = $"Proxy_{proxyKey}_Host";
            var keyToPort = $"Proxy_{proxyKey}_Port";
            var keyToByPassProxyOnLocal = $"Proxy_{proxyKey}_ByPassProxyOnLocal";
            var keyToUseDefaultCredentials = $"Proxy_{proxyKey}_UseDefaultCredentials";
            var keyToUsername = $"Proxy_{proxyKey}_UsernameEnvironmentKeyName";
            var keyToPassword = $"Proxy_{proxyKey}_PasswordEnvironmentKeyName";

            var proxyDef = new ProxyDefinitionModel();

            if (PjUtility.EnvironmentConfig.AppSettingsConfigData.ContainsKey(keyToHost))
            {
                proxyDef.Host = PjUtility.EnvironmentConfig.AppSettingsConfigData[keyToHost];
            }
            proxyDef.Port = PjUtility.EnvironmentConfig.ReadConfigData(keyToPort,
                throwErrorWhenNotFound: false, returnNullWhenNotFound: true).ToLong() > 0 ?
                PjUtility.EnvironmentConfig.AppSettingsConfigData[keyToPort].ToLong() : 8080;

            if (PjUtility.EnvironmentConfig.AppSettingsConfigData.ContainsKey(keyToByPassProxyOnLocal))
            {
                proxyDef.ByPassProxyOnLocal = PjUtility.EnvironmentConfig.AppSettingsConfigData[keyToByPassProxyOnLocal].ToBool();
            }
            if (PjUtility.EnvironmentConfig.AppSettingsConfigData.ContainsKey(keyToUseDefaultCredentials))
            {
                proxyDef.UseDefaultCredentials = PjUtility.EnvironmentConfig.AppSettingsConfigData[keyToUseDefaultCredentials].ToBool();
            }

            var username = string.Empty;
            var password = string.Empty;

            if (PjUtility.EnvironmentConfig.ReadConfigData(keyToUsername,
                throwErrorWhenNotFound: false,
                returnNullWhenNotFound: true).HasValue())
            {
                username = PjUtility.EnvironmentVariables.GetValue(PjUtility.EnvironmentConfig.ReadConfigData(keyToUsername, throwErrorWhenNotFound: false, returnNullWhenNotFound: true));
            }

            if (PjUtility.EnvironmentConfig.ReadConfigData(keyToPassword,
                throwErrorWhenNotFound: false,
                returnNullWhenNotFound: true).HasValue())
            {
                password = PjUtility.EnvironmentVariables.GetValue(
                        CryptoHelper.Decrypt(
                            PjUtility.EnvironmentConfig.ReadConfigData(keyToPassword, throwErrorWhenNotFound: false, returnNullWhenNotFound: true)));
            }

            if (username.HasValue() && password.HasValue())
            {
                proxyDef.Username = username;
                proxyDef.Password = password;
                Log("Username and Password for proxy found and set");
            }

            return AddProxy(proxyDef);
        }
        public virtual TestApiRequest AddProxy(string proxyHost, long proxyPort = 8080, bool byPassProxyOnLocal = false,
            bool useDefaultCredentials = true, string proxyUsername = null, string proxyPassword = null)
            => AddProxy(new ProxyDefinitionModel
            {
                Host = proxyHost,
                Port = proxyPort,
                ByPassProxyOnLocal = byPassProxyOnLocal,
                UseDefaultCredentials = useDefaultCredentials,
                Username = proxyUsername,
                Password = proxyPassword
            });

        public virtual TestApiRequest AddProxy(ProxyDefinitionModel proxyDefModel)
        {
            if (proxyDefModel.Host.IsEmpty())
            {
                throw new Exception("The proxy host cannot be empty");
            }

            var proxy = new WebProxy
            {
                Address = new Uri($"{proxyDefModel.Host}:{proxyDefModel.Port}"),
                BypassProxyOnLocal = proxyDefModel.ByPassProxyOnLocal,
                UseDefaultCredentials = proxyDefModel.UseDefaultCredentials
            };

            if (proxyDefModel.Username.IsEmpty() && proxyDefModel.Password.IsEmpty())
            {
                proxy.Credentials = CredentialCache.DefaultNetworkCredentials;
            }
            else
            {
                proxy.Credentials = new NetworkCredential(userName: proxyDefModel.Username, password: proxyDefModel.Password);
            }
            Proxy = proxy;

            Log($"Adding proxy settings to the request: {proxy.Address}");

            return this;
        }
        #endregion

        #region Operations

        private TestApiResponse WithRetry(
            Func<TestApiResponse> responseFactory,
            bool assertOk = false,
            int timeToSleepBetweenRetryInMilliseconds = 1000,
            int retryOption = 6,
            bool throwExceptionOnAssertFail = false,
            bool retryOnRequestTimeout = false,
            HttpStatusCode[] httpStatusCodes = null)
        {
            TestApiResponse response = null;
            for (int i = 1; i <= retryOption; i++)
            {
                try
                {
                    response = responseFactory();
                    if (assertOk)
                    {
                        if (httpStatusCodes == null)
                        {
                            if (new[]
                            {
                                HttpStatusCode.OK,
                                HttpStatusCode.Created,
                                HttpStatusCode.Accepted,
                                HttpStatusCode.NonAuthoritativeInformation,
                                HttpStatusCode.NoContent,
                                HttpStatusCode.ResetContent,
                                HttpStatusCode.PartialContent,
                                HttpStatusCode.Continue,
                                HttpStatusCode.SwitchingProtocols,
                                HttpStatusCode.MultipleChoices,
                                HttpStatusCode.MovedPermanently,
                                HttpStatusCode.Found,
                                HttpStatusCode.SeeOther,
                                HttpStatusCode.NotModified,
                                HttpStatusCode.TemporaryRedirect
                            }.Contains(response.ResponseCode) == false)
                            {
                                throw new Exception($"The response code from the server was not successful, actual code {response?.ResponseCode.ToString()}");
                            }
                        }
                        else
                        {
                            if (httpStatusCodes.Contains(response.ResponseCode))
                            {
                                throw new Exception($"The response code from the server was not successful, actual code {response?.ResponseCode.ToString()}");
                            }
                        }
                    }
                    break;
                }
                catch (Exception ex)
                {
                    if (response == null && ex.Message.ContainsIgnoreCase("A task was canceled") && retryOnRequestTimeout == false)
                    {
                        throw;
                    }
                    if (i >= retryOption && throwExceptionOnAssertFail)
                    {
                        throw;
                    }
                    Log($"Request retry option: {ex.Message}");
                    System.Threading.Thread.Sleep(timeToSleepBetweenRetryInMilliseconds);
                }
            }
            return response;
        }

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

        public virtual TestApiResponse GetWithRetry(bool assertOk = false,
            int timeToSleepBetweenRetryInMilliseconds = 1000,
            int retryOption = 6,
            bool throwExceptionOnAssertFail = false,
            bool retryOnRequestTimeout = false,
            HttpStatusCode[] httpStatusCodes = null)
            => WithRetry(() => Get(), assertOk, timeToSleepBetweenRetryInMilliseconds, retryOption,
                throwExceptionOnAssertFail, retryOnRequestTimeout, httpStatusCodes);

        /// <summary>
        /// Makes a Get request (sync)
        /// </summary>
        /// <returns></returns>
        public virtual TestApiResponse Get() => GetAsync().Result;

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

        public virtual TestApiResponse DownloadWithRetry(string filePath, bool assertOk = false,
            int timeToSleepBetweenRetryInMilliseconds = 1000,
            int retryOption = 6,
            bool throwExceptionOnAssertFail = false,
            bool retryOnRequestTimeout = false,
            HttpStatusCode[] httpStatusCodes = null)
            => WithRetry(() => Download(filePath), assertOk, timeToSleepBetweenRetryInMilliseconds, retryOption,
                throwExceptionOnAssertFail, retryOnRequestTimeout, httpStatusCodes);


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

        public virtual TestApiResponse PostWithRetry(bool assertOk = false,
            int timeToSleepBetweenRetryInMilliseconds = 1000,
            int retryOption = 6,
            bool throwExceptionOnAssertFail = false,
            bool retryOnRequestTimeout = false,
            HttpStatusCode[] httpStatusCodes = null)
            => WithRetry(() => Post(), assertOk, timeToSleepBetweenRetryInMilliseconds, retryOption,
                throwExceptionOnAssertFail, retryOnRequestTimeout, httpStatusCodes);

        public virtual async Task<TestApiResponse> PatchAsync() => await SendRequestAsync(new HttpMethod("PATCH"));
        public virtual TestApiResponse Patch()
        {
            try
            {
                return PatchAsync().Result;
            }
            catch (Exception ex)
            {
                if (!ex.ToString().Contains("A task was canceled"))
                {
                    throw;
                }
                else
                {
                    throw new Exception("The query timed out performing the action with a message 'A task was canceled from Server'");
                }
            }
        }
        public virtual TestApiResponse PatchWithRetry(bool assertOk = false,
            int timeToSleepBetweenRetryInMilliseconds = 1000,
            int retryOption = 6,
            bool throwExceptionOnAssertFail = false,
            bool retryOnRequestTimeout = false,
            HttpStatusCode[] httpStatusCodes = null)
            => WithRetry(() => Patch(), assertOk, timeToSleepBetweenRetryInMilliseconds, retryOption,
                throwExceptionOnAssertFail, retryOnRequestTimeout, httpStatusCodes);


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
        public virtual TestApiResponse PutWithRetry(bool assertOk = false,
            int timeToSleepBetweenRetryInMilliseconds = 1000,
            int retryOption = 6,
            bool throwExceptionOnAssertFail = false,
            bool retryOnRequestTimeout = false,
            HttpStatusCode[] httpStatusCodes = null)
            => WithRetry(() => Put(), assertOk, timeToSleepBetweenRetryInMilliseconds, retryOption,
                throwExceptionOnAssertFail, retryOnRequestTimeout, httpStatusCodes);

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
                        Runtime.Logger.Log($"Problem with the Certificate: {cert.Subject}");
                        Runtime.Logger.Log($"Sender: {sender}");
                        Runtime.Logger.Log($"cert: {cert}");
                        Runtime.Logger.Log($"chain: {chain}");
                        Runtime.Logger.Log($"sslPolicyErrors: {sslPolicyErrors}");
                    }
                    return true;
                };
            }

            if (NtmlAuthentication)
            {
                httpClientHandler.UseDefaultCredentials = true;
                httpClientHandler.CookieContainer = new CookieContainer();
            }

            if (Certificates.Count > 0)
            {
                Certificates.Iter(c => httpClientHandler.ClientCertificates.Add(c));
                httpClientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            }

            if (Proxy != null)
            {
                httpClientHandler.Proxy = Proxy;
            }

            var client = new HttpClient(httpClientHandler);

            if (HeaderAuthentication != null)
            {
                client.DefaultRequestHeaders.Authorization = HeaderAuthentication;
            }

            if (Timeout > 0)
            {
                client.Timeout = TimeSpan.FromSeconds(Timeout);
            }
            else if (TestAnyAppConfig.DefaultApiResponseTimeoutWaitPeriodInSeconds > 0)
            {
                client.Timeout = TimeSpan.FromSeconds(TestAnyAppConfig.DefaultApiResponseTimeoutWaitPeriodInSeconds);
            }

            foreach (var kvp in Headers)
            {
                if (kvp.Key.EqualsIgnoreCase("Content-Type") && httpMethod == HttpMethod.Get)
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(kvp.Value));
                }
                else
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation(kvp.Key, kvp.Value);
                }
            }

            if (NoCache)
            {
                client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
                {
                    NoCache = true
                };
            }

            foreach (Cookie cookie in Cookies)
            {
                httpClientHandler.CookieContainer.Add(cookie);
            }

            HttpResponseMessage httpResponseMessage = null;
            Runtime.Logger.Log($"Requesting {httpMethod} on: {Uri.AbsoluteUri}");

            if (httpMethod == HttpMethod.Post || httpMethod == HttpMethod.Put || httpMethod == new HttpMethod("PATCH"))
            {
                HttpRequestMessage httpRequest = new HttpRequestMessage(httpMethod, Uri);

                if (ContentTypeSetAsEntity && FilePathUploadMode && Body.HasValue())
                {
                    using (var stream = File.OpenRead(Body))
                    {
                        httpRequest.Content = new StreamContent(stream);
                        httpRequest.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue(Headers.First(h => h.Key == "Content-Type").Value);
                        httpResponseMessage = await client.SendAsync(httpRequest);
                    }
                }
                else
                {
                    if (ContentTypeSetAsEntity)
                    {
                        if (JsonBody.HasValue()) httpRequest.Content = new StringContent(JsonBody);
                        else if (Body.HasValue()) httpRequest.Content = new StringContent(Body);
                        else if (PostParams != null) httpRequest.Content = new FormUrlEncodedContent(PostParams.Select(kvp => new KeyValuePair<string, string>(kvp.Key, (string)kvp.Value)));

                        httpRequest.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue(Headers.First(h => h.Key == "Content-Type").Value);
                    }
                    else if (JsonBody.HasValue() || Body.HasValue())
                    {
                        var contentType = Headers.FirstOrDefault(h => h.Key == "Content-Type")?.Value;
                        if (contentType.IsEmpty())
                        {
                            contentType = JsonBody.HasValue() ? "application/json" : Body.HasValue() ? "text/xml" : String.Empty;
                        }
                        httpRequest.Content = new StringContent(JsonBody ?? Body, Encoding.UTF8, contentType);
                    }
                    else if (PostParams != null)
                    {
                        httpRequest.Content = new FormUrlEncodedContent(PostParams.Select(kvp => new KeyValuePair<string, string>(kvp.Key, (string)kvp.Value)));
                    }
                    httpResponseMessage = await client.SendAsync(httpRequest);
                }
            }
            else if (httpMethod == HttpMethod.Get)
            {
                HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Get, Uri);

                if (ContentTypeSetAsEntity)
                {
                    if (JsonBody.HasValue()) httpRequest.Content = new StringContent(JsonBody);
                    else if (Body.HasValue()) httpRequest.Content = new StringContent(Body);
                    else httpRequest.Content = new StringContent(string.Empty);

                    httpRequest.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue(Headers.First(h => h.Key == "Content-Type").Value);
                }
                httpResponseMessage = requestToDownloadFile.IsEmpty() ?
                    await client.SendAsync(httpRequest) :
                    await client.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
            }
            else if (httpMethod == HttpMethod.Delete)
            {
                if (string.IsNullOrEmpty(JsonBody ?? Body))
                {
                    httpResponseMessage = await client.DeleteAsync(Uri);
                }
                else
                {
                    var request = new HttpRequestMessage(httpMethod, Uri);
                    request.Content = new StringContent(JsonBody ?? Body, Encoding.UTF8, "application/json");
                    httpResponseMessage = await client.SendAsync(request);
                }
            }

            var responseHeaders = httpResponseMessage.Headers.Concat(httpResponseMessage.Content.Headers)
                .Select(k => new KeyValuePair<string, string>(k.Key, string.Join(",", k.Value)));

            var schemeHostAndPortPart = Uri.GetLeftPart(UriPartial.Authority);
            var uri = new Uri(schemeHostAndPortPart);

            if (requestToDownloadFile.HasValue())
            {
                IoHelper.CreateDirectory(requestToDownloadFile);
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
                    ResponseHeaders = new KeyValuePatternModel(responseHeaders)
                };
            }

            var result = await httpResponseMessage.Content.ReadAsStringAsync();

            var response = new TestApiResponse()
            {
                Request = new TestApiRequest(httpResponseMessage.RequestMessage.RequestUri),
                ResponseCode = httpResponseMessage.StatusCode,
                Cookies = (ExtractDomainCookies ?
                    httpClientHandler.CookieContainer.GetCookies(uri).Merge(httpClientHandler.CookieContainer.GetCookies(Uri)) :
                    httpClientHandler.CookieContainer.GetCookies(uri))
                    .ExtractCookies(responseHeaders, httpResponseMessage.RequestMessage.RequestUri.AbsoluteUri),
                ResponseBody = new TestApiBody(result),
                HttpResponseMessage = httpResponseMessage,
                ResponseHeaders = new KeyValuePatternModel(responseHeaders)
            };

            Runtime.Logger.Log($"Request {httpMethod} on: {Uri.AbsoluteUri} returned with: {response.ResponseCode}");
            if (response.ResponseCode != HttpStatusCode.OK &&
                response.ResponseCode != HttpStatusCode.Accepted &&
                response.ResponseCode != HttpStatusCode.Created &&
                response.ResponseCode != HttpStatusCode.Redirect)
            {
                Runtime.Logger.Log($"Request with content: {JsonBody ?? Body}");
                Runtime.Logger.Log($"Resposne on {httpMethod} with status: {response.ResponseCode} has ResponseMessage: {response.ResponseBody.ContentString}");
            }

            return response;
        }
    }
}
