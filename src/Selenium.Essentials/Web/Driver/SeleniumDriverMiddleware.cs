using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;

namespace Selenium.Essentials;

[Serializable]
internal class SeleniumDriverMiddleware
{
    private readonly SeleniumDriverProviderLocal _seleniumDriverLocalProvider = new();
    private readonly SeleniumDriverProviderRemote _seleniumDriverRemoteProvider = new();

    public IWebDriver GetDriver(string browserName, bool isRemote, RemoteDriverAccessModel? remoteDriverAccessModel = null, bool requireFileDetector = false)
    {
        if (!Enum.TryParse(browserName, true, out BrowserType browserType))
        {
            throw new DriverInitializationException($"Invalid driver type: {browserName}");
        }
        return GetDriver(browserType, isRemote, remoteDriverAccessModel, requireFileDetector);
    }
    public IWebDriver GetDriver(BrowserType browserType, bool isRemote, RemoteDriverAccessModel? remoteDriverAccessModel = null, bool requireFileDetector = false)
    {
        IWebDriver? driver = null;
        if (isRemote)
        {
            driver = _seleniumDriverRemoteProvider.GetDriver(browserType, remoteDriverAccessModel);
        }
        else
        {
            driver = _seleniumDriverLocalProvider.GetDriver(browserType);
        }

        ApplyFileDetectionIfNeeded(driver, requireFileDetector);

        return driver;
    }

    private void ApplyFileDetectionIfNeeded(IWebDriver driver, bool requireFileDetector)
    {
        if (driver == null) return;
        if (requireFileDetector && driver is IAllowsFileDetection allowsDetection)
        {
            allowsDetection.FileDetector = new LocalFileDetector();
        }
    }
}
