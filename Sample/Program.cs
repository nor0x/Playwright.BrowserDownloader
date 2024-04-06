using nor0x.Playwright.BrowserDownloader;

Console.WriteLine("Hello World!");
var downloader = new BrowserDownloader();
await downloader.DownloadBrowserAsync("chromium", "mac12", progressCallback:new Progress<double>(progressCallback));

void progressCallback(double obj)
{
    Console.WriteLine("progress: " + obj);
}