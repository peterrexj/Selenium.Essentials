using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;
using Pj.Library;
using System;
using System.IO;
using System.Linq;

namespace Selenium.Essentials;

[Serializable]
internal class SeleniumDriverProviderLocal
{
    private readonly SeleniumDriverCapabilitiesProvider _browserCapabilitiesProvider = new();
    private readonly string _driverPath;

    public SeleniumDriverProviderLocal()
    {
        _driverPath = Path.Combine(PjUtility.Runtime.ExecutingFolder);
    }

    public IWebDriver GetDriver(string browserName)
    {
        Enum.TryParse(browserName, true, out BrowserType browserType);
        return GetDriver(browserType);
    }
    public IWebDriver GetDriver(BrowserType driverType)
    {
        PjUtility.Runtime.Logger.Log("[Start to generate LOCAL driver at]: " + DateTime.Now.ToString("yyMMddHHmmss"));

        return driverType switch
        {
            BrowserType.Chrome => GetChrome(),
            BrowserType.FireFox => GetFirefox(),
            BrowserType.InternetExplorer => GetInternetExplorer(),
            BrowserType.Edge => GetEdge(),
            BrowserType.Safari => GetSafari(),
            _ => throw new Exception("[Cannot match driver types :] " + driverType),
        };
    }

    private IWebDriver GetChrome()
    {
        var service = ChromeDriverService.CreateDefaultService(_driverPath);
        service.HideCommandPromptWindow = true;
        
        try
        {
            return new ChromeDriver(service, _browserCapabilitiesProvider.GetChrome(isRemote: false, null));
        }
        catch (Exception ex)
        {
            PjUtility.Runtime.Logger.Log($"Failed to create ChromeDriver: {ex.Message}");
            throw new WebDriverInitializationException("Failed to initialize Chrome WebDriver.", ex);
        }
    }

    
    private IWebDriver GetFirefox()
    {
        try
        {
            var service = FirefoxDriverService.CreateDefaultService(_driverPath);
            try
            {
                service.FirefoxBinaryPath = BrowserHelper.InstalledBrowsers.FirstOrDefault(d => d.Name.ContainsIgnoreCase("firefox"))?.InstallationPath;
            }
            catch (Exception exBrwPath)
            {
                PjUtility.Runtime.Logger.Log($"Failed to get the installed firefox path, {exBrwPath}");
            }
            
            service.HideCommandPromptWindow = true;

            return new FirefoxDriver(service, _browserCapabilitiesProvider.GetFirefox(isRemote: false, null) as FirefoxOptions);
        }
        catch (Exception ex)
        {
            PjUtility.Runtime.Logger.Log($"Failed to create Firefox: {ex.Message}");
            throw new WebDriverInitializationException("Failed to initialize Chrome WebDriver.", ex);
        }
    }

    private IWebDriver GetInternetExplorer()
    {
        try
        {
            var service = InternetExplorerDriverService.CreateDefaultService(_driverPath);
            service.HideCommandPromptWindow = true;

            return new InternetExplorerDriver(service, _browserCapabilitiesProvider.GetInternetExplorer(isRemote: false, null) as InternetExplorerOptions);
        }
        catch (Exception ex)
        {
            PjUtility.Runtime.Logger.Log($"Failed to create Internet explorer: {ex.Message}");
            throw new WebDriverInitializationException("Failed to initialize Chrome WebDriver.", ex);
        }
    }

    private IWebDriver GetEdge()
    {
        try
        {
            var service = EdgeDriverService.CreateDefaultService(_driverPath, "msedgedriver.exe");
            service.HideCommandPromptWindow = true;
            return new EdgeDriver(service, _browserCapabilitiesProvider.GetEdge(isRemote: false, null) as EdgeOptions);
        }
        catch (Exception ex)
        {
            PjUtility.Runtime.Logger.Log($"Failed to create Internet explorer: {ex.Message}");
            throw new WebDriverInitializationException("Failed to initialize Chrome WebDriver.", ex);
        }
    }

    private IWebDriver GetSafari()
    {
        var service = SafariDriverService.CreateDefaultService(_driverPath);
        service.HideCommandPromptWindow = true;
        try
        {
            return new SafariDriver(service, _browserCapabilitiesProvider.GetSafari(isRemote: false, null) as SafariOptions);
        }
        catch (Exception ex)
        {
            PjUtility.Runtime.Logger.Log($"Failed to create Safari: {ex.Message}");
            throw new WebDriverInitializationException("Failed to initialize Chrome WebDriver.", ex);
        }
    }
}
