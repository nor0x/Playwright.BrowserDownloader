# Playwright.BrowserDownloader

<img src="https://raw.githubusercontent.com/nor0x/Playwright.BrowserDownloader/main/art/packageicon.png" width="320px" />

a small and simple helper library to manually download browsers for playwright usage.

[![.NET](https://github.com/nor0x/Playwright.BrowserDownloader/actions/workflows/dotnet.yml/badge.svg)](https://github.com/nor0x/Playwright.BrowserDownloader/actions/workflows/dotnet.yml)
[![](https://img.shields.io/nuget/v/nor0x.Playwright.BrowserDownloader)](https://www.nuget.org/packages/nor0x.Playwright.BrowserDownloader)
[![](https://img.shields.io/nuget/dt/nor0x.Playwright.BrowserDownloader)](https://www.nuget.org/packages/nor0x.Playwright.BrowserDownloader)




# Usage
nor0x.Playwright.BrowserDownloader allows to manually download browsers for Playwright usage, in a managed way from .NET. It allows injection of a custom `HttpClient` and reports download progress via `IProgress`. It ships with the `browsers.json` file from [Playwright](https://github.com/microsoft/playwright/blob/main/packages/playwright-core/browsers.json) but can also handle a custom version file.

```csharp
var downloader = new BrowserDownloader(new MyCustomHttpClient());
await downloader.DownloadBrowserAsync("chromium", "mac12", new Progress<double>(progressCallback));

void progressCallback(double p)
{
    Console.WriteLine("download progress: " + p);
}
```

## specific version
specific versions can be set by passing the version string to the `DownloadBrowserAsync` method. If there is already an `IPlaywright` object available, the executable path for a browser engine can be set directly.

```csharp
using var playwright = await Playwright.CreateAsync();
var downloader = new BrowserDownloader();
await downloader.DownloadBrowserAsync("firefox", "win64", downloadPath: browserPath, executablePath: playwright.Firefox.ExecutablePath);
```


# more info
https://johnnys.news/2023/07/nor0x-Playwright-BrowserDownloader

happy coding and mtfbwya ✨
