using Microsoft.Playwright;
using nor0x.Playwright.BrowserDownloader;

Console.WriteLine("Hello World!");
var downloader = new BrowserDownloader();


var os = Environment.OSVersion;
var osName = os.Platform.ToString().ToLower();
var platform = TargetPlatform.Ubuntu2204x64;
if (osName.Contains("win"))
{
    platform = TargetPlatform.Win64;
}
else if (osName.Contains("mac"))
{
    platform = TargetPlatform.Mac14;
}

Console.WriteLine("Downloading browser for platform: " + platform);
var playwright = await Playwright.CreateAsync();

var path = await downloader.DownloadBrowserAsync(BrowserInfo.ChromiumTipOfTree, platform, executablePath: playwright.Chromium.ExecutablePath, progressCallback:new Progress<double>(progressCallback));

Console.WriteLine("Downloaded browser to: " + path);

void progressCallback(double obj)
{
    Console.WriteLine("progress: " + obj);
}