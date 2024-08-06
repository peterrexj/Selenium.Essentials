using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;
using Pj.Library;
using System;

namespace Selenium.Essentials;

[Serializable]
public class SeleniumDriverCapabilitiesProvider
{
    private readonly SeleniumDriverProxyProvider _seleniumDriverProxyProvider = new();

    public DriverOptions GetCapability(BrowserType browserType, bool isRemote, RemoteDriverAccessModel? browserGridCapability)
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

    public T GetCapability<T>(BrowserType browserType, bool isRemote, RemoteDriverAccessModel? browserGridCapability) where T : DriverOptions, new()
    {
        return browserType switch
        {
            BrowserType.Chrome => GetChrome(isRemote, browserGridCapability) as T,
            BrowserType.FireFox => GetFirefox(isRemote, browserGridCapability) as T,
            BrowserType.InternetExplorer => GetInternetExplorer(isRemote, browserGridCapability) as T,
            BrowserType.Edge => GetEdge(isRemote, browserGridCapability) as T,
            BrowserType.Safari => GetSafari(isRemote, browserGridCapability) as T,
            _ => throw new Exception("[Can not matching driver types :] " + browserType),
        };
    }

    public ChromeOptions GetChrome(bool isRemote, RemoteDriverAccessModel? browserGridCapability)
    {
        var options = new ChromeOptions();

        ConfigureCommonOptions(options);

        if (isRemote)
        {
            if (browserGridCapability != null)
            {
                if (browserGridCapability.Platform.HasValue())
                {
                    options.PlatformName = browserGridCapability.Platform;
                }
                if (browserGridCapability.BrowserVersion.HasValue())
                {
                    options.BrowserVersion = browserGridCapability.BrowserVersion;
                }
                if (browserGridCapability.Capabilities?.Count > 0)
                {
                    foreach (var option in browserGridCapability.Capabilities)
                    {
                        options.AddAdditionalOption(option.Key, option.Value);
                    }
                }
            }
        }
        ApplyProxySettings(options, browserGridCapability);

        return options;
    }

    public FirefoxOptions GetFirefox(bool isRemote, RemoteDriverAccessModel? browserGridCapability)
    {
        var options = new FirefoxOptions
        {
            AcceptInsecureCertificates = true
        };

        if (isRemote)
        {
            if (browserGridCapability != null)
            {
                if (browserGridCapability.Platform.HasValue())
                {
                    options.PlatformName = browserGridCapability.Platform;
                }
                if (browserGridCapability.BrowserVersion.HasValue())
                {
                    options.BrowserVersion = browserGridCapability.BrowserVersion;
                }
                if (browserGridCapability.Capabilities?.Count > 0)
                {
                    foreach (var option in browserGridCapability.Capabilities)
                    {
                        options.AddAdditionalOption(option.Key, option.Value);
                    }
                }
            }
        }
        
        ApplyProxySettings(options, browserGridCapability);

        return options;
    }

    public EdgeOptions GetEdge(bool isRemote, RemoteDriverAccessModel? browserGridCapability)
    {
        var options = new EdgeOptions
        {
            AcceptInsecureCertificates = true
        };

        if (isRemote)
        {
            if (browserGridCapability != null)
            {
                if (browserGridCapability.Platform.HasValue())
                {
                    options.PlatformName = browserGridCapability.Platform;
                }
                if (browserGridCapability.BrowserVersion.HasValue())
                {
                    options.BrowserVersion = browserGridCapability.BrowserVersion;
                }
                if (browserGridCapability.Capabilities?.Count > 0)
                {
                    foreach (var option in browserGridCapability.Capabilities)
                    {
                        options.AddAdditionalOption(option.Key, option.Value);
                    }
                }
            }
        }

        ApplyProxySettings(options, browserGridCapability);

        return options;
    }

    public SafariOptions GetSafari(bool isRemote, RemoteDriverAccessModel? browserGridCapability)
    {
        var options = new SafariOptions
        {
            AcceptInsecureCertificates = true
        };

        if (isRemote)
        {
            if (browserGridCapability != null)
            {
                if (browserGridCapability.Platform.HasValue())
                {
                    options.PlatformName = browserGridCapability.Platform;
                }
                if (browserGridCapability.BrowserVersion.HasValue())
                {
                    options.BrowserVersion = browserGridCapability.BrowserVersion;
                }
                if (browserGridCapability.Capabilities?.Count > 0)
                {
                    foreach (var option in browserGridCapability.Capabilities)
                    {
                        options.AddAdditionalOption(option.Key, option.Value);
                    }
                }
            }
        }

        ApplyProxySettings(options, browserGridCapability);

        return options;
    }

    public InternetExplorerOptions GetInternetExplorer(bool isRemote, RemoteDriverAccessModel? browserGridCapability)
    {
        var options = new InternetExplorerOptions
        {
            AcceptInsecureCertificates = true,
            IgnoreZoomLevel = false,
            IntroduceInstabilityByIgnoringProtectedModeSettings = true
        };

        if (isRemote)
        {
            if (browserGridCapability != null)
            {
                if (browserGridCapability.Platform.HasValue())
                {
                    options.PlatformName = browserGridCapability.Platform;
                }
                if (browserGridCapability.BrowserVersion.HasValue())
                {
                    options.BrowserVersion = browserGridCapability.BrowserVersion;
                }
                if (browserGridCapability.Capabilities?.Count > 0)
                {
                    foreach (var option in browserGridCapability.Capabilities)
                    {
                        options.AddAdditionalOption(option.Key, option.Value);
                    }
                }
            }
        }

        ApplyProxySettings(options, browserGridCapability);
        return options;
    }

    private void ApplyProxySettings(DriverOptions options, RemoteDriverAccessModel? browserGridCapability)
    {
        var proxy = _seleniumDriverProxyProvider.BuildSeleniumProxy(browserGridCapability);
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
