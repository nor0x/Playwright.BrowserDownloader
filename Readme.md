# Playwright.BrowserDownloader

<img src="https://raw.githubusercontent.com/nor0x/Playwright.BrowserDownloader/main/art/packageicon.png" width="320px" />

a small and simple helper library to manually download browsers for playwright usage.

[![.NET](https://github.com/nor0x/Playwright.BrowserDownloader/actions/workflows/dotnet.yml/badge.svg)](https://github.com/nor0x/Playwright.BrowserDownloader/actions/workflows/dotnet.yml)
[![](https://img.shields.io/nuget/v/nor0x.Playwright.BrowserDownloader)](https://www.nuget.org/packages/nor0x.Playwright.BrowserDownloader)
[![](https://img.shields.io/nuget/dt/nor0x.Playwright.BrowserDownloader)](https://www.nuget.org/packages/nor0x.Playwright.BrowserDownloader)




# Usage
BrowserDownloader allows to manually download browsers for Playwright usage, in a managed way from .NET. It allows injection of a custom `HttpClient` and reports download progress via `IProgress`. It ships with the `browsers.json` file from [Playwright](https://github.com/microsoft/playwright/blob/main/packages/playwright-core/browsers.json) but can also handle a custom version file.

```csharp
var downloader = new BrowserDownloader(new MyCustomHttpClient());
await downloader.DownloadBrowserAsync(new Progress<double>(progressCallback), "chromium", "mac12");

void progressCallback(double p)
{
    Console.WriteLine("download progress: " + p);
}
```

happy coding and mtfbwya ✨