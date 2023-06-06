using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;

namespace nor0x.Playwright.BrowserDownloader;

public class BrowserDownloader
{
    HttpClient _httpClient;
    List<string> _cdns;

    public BrowserDownloader(HttpClient httpClient = null)
    {
        _httpClient = httpClient ?? new HttpClient();
        _cdns = new List<string>()
        {
            "https://playwright.azureedge.net/",
            "https://playwright-akamai.azureedge.net/",
            "https://playwright-verizon.azureedge.net/"
        }.OrderBy(x => Guid.NewGuid()).ToList();
    }

    public async Task DownloadBrowserAsync(IProgress<double> progressCallback, string browser, string platform, string browsersFile = null, string version = null, string downloadPath = null)
    {
        try
        {
            _cdns = _cdns.OrderBy(x => Guid.NewGuid()).ToList();
            if(string.IsNullOrEmpty(version))
            {
                version = GetVersion(browser, browsersFile);
                if(string.IsNullOrEmpty(version))
                {
                    throw new Exception("Version not found");
                }
            }
            if(string.IsNullOrEmpty(downloadPath))
            {
                //check if current OS is windows
                if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ms-playwright", $"{browser}-{version}");

                }
                else if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    downloadPath = $"~/.cache/ms-playwright/{browser}-{version}";

                }
                else if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.InternetCache), "ms-playwright", $"{browser}-{version}");
                }
                else
                {
                    throw new PlatformNotSupportedException("Platform not supported");
                }
            }
            var fileUrl = GetDownloadPath(browser, version, platform);
            if(string.IsNullOrEmpty(fileUrl))
            {
                throw new Exception("Download path not found");
            }

            var tempDir = downloadPath.Substring(0, downloadPath.LastIndexOf(Path.DirectorySeparatorChar));
            if(!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }
            var fileName = fileUrl.Substring(fileUrl.LastIndexOf('/') + 1);
            var tempPath = Path.Combine(tempDir, fileName);
            using (HttpResponseMessage response = await _httpClient.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();

                using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                {
                    long? totalBytes = response.Content.Headers.ContentLength;

                    byte[] buffer = new byte[8192];
                    int bytesRead;
                    long downloadedBytes = 0;

                    using (FileStream fileStream = File.Create(tempPath))
                    {
                        while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            await fileStream.WriteAsync(buffer, 0, bytesRead);
                            downloadedBytes += bytesRead;

                            if (totalBytes.HasValue)
                            {
                                double progressPercentage = (double)(downloadedBytes * 100) / totalBytes.Value;
                                progressCallback.Report(progressPercentage);
                            }
                        }
                    }
                    ZipFile.ExtractToDirectory(tempPath, downloadPath);
                    File.Delete(tempPath);
                    File.Create(Path.Combine(downloadPath, "INSTALLATION_COMPLETE"));
                }
            }

            Trace.WriteLine("Download completed successfully!");
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }

    string GetVersion(string browserName, string browsersFile = null)
    {
        if(string.IsNullOrEmpty(browsersFile))
        {
            browsersFile = "browsers.json";
        }

        var json = File.ReadAllText(browsersFile);
        JsonDocument document = JsonDocument.Parse(json);

        JsonElement browsersArray = document.RootElement.GetProperty("browsers");

        JsonElement browser = browsersArray.EnumerateArray()
            .FirstOrDefault(browser => browser.GetProperty("name").GetString() == browserName);

        if (browser.ValueKind != JsonValueKind.Undefined)
        {
            string revision = browser.GetProperty("revision").GetString();
            return revision;
        }
        else
        {
            throw new FileNotFoundException($"Browser {browserName} not found in browsers.json");
        }
    }

    string GetDownloadPath(string browser, string version, string platform)
    {
        if (Constants.Paths.ContainsKey(browser) && Constants.Paths[browser].ContainsKey(platform))
        {
            return _cdns[0] + string.Format(Constants.Paths[browser][platform], version);
        }
        return null;
    }
}
