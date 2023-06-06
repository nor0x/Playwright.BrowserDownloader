using nor0x.Playwright.BrowserDownloader;

Console.WriteLine("Hello World!");
var downloader = new BrowserDownloader();
await downloader.DownloadBrowserAsync(new Progress<double>(progressCallback), "chromium", "mac12");

void progressCallback(double obj)
{
    Console.WriteLine("progress: " + obj);
}