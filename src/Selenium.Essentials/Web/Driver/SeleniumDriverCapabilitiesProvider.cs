using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;
using Pj.Library;
using System;
using System.Linq;

namespace Selenium.Essentials;

[Serializable]
public class SeleniumDriverCapabilitiesProvider
{
    private readonly SeleniumDriverProxyProvider _seleniumDriverProxyProvider = new();

    public dynamic GetCapability(BrowserType browserType, bool isRemote, RemoteDriverAccessModel? browserGridCapability)
    {
        return browserType switch
        {
            BrowserType.Chrome => GetChrome(isRemote, browserGridCapability),
            BrowserType.FireFox => GetFirefox(isRemote, browserGridCapability),
            BrowserType.InternetExplorer => GetInternetExplorer(isRemote, browserGridCapability),
            BrowserType.Edge => GetEdge(isRemote, browserGridCapability),
            BrowserType.Safari => GetSafari(isRemote, browserGridCapability),
            _ => throw new Exception("[Cannot match driver types:] " + browserType),
        };
    }

    private static readonly string[] RemoteCapabilityProperty = new[] { "browsername", "browserversion", "platformname" };

    private ChromeOptions? _chromeOptions;
    private FirefoxOptions? _firefoxOptions;
    private InternetExplorerOptions? _internetExplorerOptions;
    private EdgeOptions? _edgeOptions;
    private SafariOptions? _safariOptions;

    public ChromeOptions ChromeOptions
    {
        set => _chromeOptions = value;
    }
    public FirefoxOptions FirefoxOptions
    {
        set => _firefoxOptions = value;
    }
    public InternetExplorerOptions InternetExplorerOptions
    {
        set => _internetExplorerOptions = value;
    }
    public EdgeOptions EdgeOptions
    {
        set => _edgeOptions = value;
    }
    public SafariOptions SafariOptions
    {
        set => _safariOptions = value;
    }

    public ChromeOptions GetChrome(bool isRemote, RemoteDriverAccessModel? remoteDriverDetail)
    {
        if (_chromeOptions != null) return _chromeOptions;

        var options = new ChromeOptions();
        
        ConfigureCommonOptions(options);

        if (isRemote)
        {
            if (remoteDriverDetail != null)
            {
                if (remoteDriverDetail.Platform.HasValue())
                {
                    options.PlatformName = remoteDriverDetail.Platform;
                }
                if (remoteDriverDetail.BrowserVersion.HasValue())
                {
                    options.BrowserVersion = remoteDriverDetail.BrowserVersion;
                }
                if (remoteDriverDetail.Capabilities?.Any() == true)
                {
                    foreach (var option in remoteDriverDetail.Capabilities.Where(o => RemoteCapabilityProperty.ContainsIgnoreCase(o.Key) == false))
                    {
                        options.AddAdditionalOption(option.Key, option.Value);
                    }
                }
            }
        }
        ApplyProxySettings(options, remoteDriverDetail);

        return options;
    }

    public FirefoxOptions GetFirefox(bool isRemote, RemoteDriverAccessModel? remoteDriverDetail)
    {
        if (_firefoxOptions != null) return _firefoxOptions;

        var options = new FirefoxOptions
        {
            AcceptInsecureCertificates = true
        };

        if (isRemote)
        {
            if (remoteDriverDetail != null)
            {
                if (remoteDriverDetail.Platform.HasValue())
                {
                    options.PlatformName = remoteDriverDetail.Platform;
                }
                if (remoteDriverDetail.BrowserVersion.HasValue())
                {
                    options.BrowserVersion = remoteDriverDetail.BrowserVersion;
                }
                if (remoteDriverDetail.Capabilities?.Any() == true)
                {
                    foreach (var option in remoteDriverDetail.Capabilities.Where(o => RemoteCapabilityProperty.ContainsIgnoreCase(o.Key) == false))
                    {
                        options.AddAdditionalOption(option.Key, option.Value);
                    }
                }
            }
        }
        
        ApplyProxySettings(options, remoteDriverDetail);

        return options;
    }

    public EdgeOptions GetEdge(bool isRemote, RemoteDriverAccessModel? remoteDriverDetail)
    {
        if (_edgeOptions != null) return _edgeOptions;

        var options = new EdgeOptions
        {
            AcceptInsecureCertificates = true
        };

        if (isRemote)
        {
            if (remoteDriverDetail != null)
            {
                if (remoteDriverDetail.Platform.HasValue())
                {
                    options.PlatformName = remoteDriverDetail.Platform;
                }
                if (remoteDriverDetail.BrowserVersion.HasValue())
                {
                    options.BrowserVersion = remoteDriverDetail.BrowserVersion;
                }
                if (remoteDriverDetail.Capabilities?.Any() == true)
                {
                    foreach (var option in remoteDriverDetail.Capabilities.Where(o => RemoteCapabilityProperty.ContainsIgnoreCase(o.Key) == false))
                    {
                        options.AddAdditionalOption(option.Key, option.Value);
                    }
                }
            }
        }

        ApplyProxySettings(options, remoteDriverDetail);

        return options;
    }

    public SafariOptions GetSafari(bool isRemote, RemoteDriverAccessModel? remoteDriverDetail)
    {
        if (_safariOptions != null) return _safariOptions;

        var options = new SafariOptions
        {
            AcceptInsecureCertificates = true
        };
        
        if (isRemote)
        {
            if (remoteDriverDetail != null)
            {
                if (remoteDriverDetail.Platform.HasValue())
                {
                    options.PlatformName = remoteDriverDetail.Platform;
                }
                if (remoteDriverDetail.BrowserVersion.HasValue())
                {
                    options.BrowserVersion = remoteDriverDetail.BrowserVersion;
                }
                if (remoteDriverDetail.Capabilities?.Any() == true)
                {
                    foreach (var option in remoteDriverDetail.Capabilities.Where(o => RemoteCapabilityProperty.ContainsIgnoreCase(o.Key) == false))
                    {
                        options.AddAdditionalOption(option.Key, option.Value);
                    }
                }
            }
        }
        

        ApplyProxySettings(options, remoteDriverDetail);

        return options;
    }

    public InternetExplorerOptions GetInternetExplorer(bool isRemote, RemoteDriverAccessModel? remoteDriverDetail)
    {
        if (_internetExplorerOptions != null) return _internetExplorerOptions;

        var options = new InternetExplorerOptions
        {
            AcceptInsecureCertificates = true,
            IgnoreZoomLevel = false,
            IntroduceInstabilityByIgnoringProtectedModeSettings = true
        };

        if (isRemote)
        {
            if (remoteDriverDetail != null)
            {
                if (remoteDriverDetail.Platform.HasValue())
                {
                    options.PlatformName = remoteDriverDetail.Platform;
                }
                if (remoteDriverDetail.BrowserVersion.HasValue())
                {
                    options.BrowserVersion = remoteDriverDetail.BrowserVersion;
                }
                if (remoteDriverDetail.Capabilities?.Count > 0)
                {
                    foreach (var option in remoteDriverDetail.Capabilities.Where(o => RemoteCapabilityProperty.ContainsIgnoreCase(o.Key) == false))
                    {
                        options.AddAdditionalOption(option.Key, option.Value);
                    }
                }
            }
        }

        ApplyProxySettings(options, remoteDriverDetail);
        return options;
    }

    private void ApplyProxySettings(DriverOptions options, RemoteDriverAccessModel? remoteDriverDetail)
    {
        var proxy = _seleniumDriverProxyProvider.BuildSeleniumProxy(remoteDriverDetail);
        if (proxy != null)
        {
            options.Proxy = proxy;
        }
    }
    private void ConfigureCommonOptions(ChromeOptions options)
    {
        options.AddUserProfilePreference("download.default_directory", PjUtility.Runtime.ExecutingFolder);
        options.AddUserProfilePreference("download.prompt_for_download", false);
        options.AddArgument("--disable-extensions");
        options.AddArgument("no-sandbox");
        options.AddArgument("--ignore-certificate-errors");
        options.AddAdditionalOption("useAutomationExtension", false);
        options.AddUserProfilePreference("profile.content_settings.exceptions.automatic_downloads.*.setting", 1);
        options.AcceptInsecureCertificates = true;
    }
}
