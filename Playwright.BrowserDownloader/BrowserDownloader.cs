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
        };
    }

    public string GetRequiredVersion(string browser, string executablePath)
    {
        if(executablePath.Contains($"{browser}-"))
        {
            string[] segments = executablePath.Split(System.IO.Path.DirectorySeparatorChar);
            var browserAndVersion = segments.FirstOrDefault(segment => segment.Contains($"{browser}-"));
            var version = browserAndVersion.Split('-')[1];
            return version;
        }
        return string.Empty;
    }

    public async Task DownloadBrowserAsync(
        string browser, 
        string platform, 
        string? version = null, 
        string? executablePath = null, 
        string? browsersFile = null, 
        string? downloadPath = null, 
        IProgress<double>? progressCallback = null)
    {
        try
        {
            if (!string.IsNullOrEmpty(executablePath) && string.IsNullOrEmpty(version))
            {
                version = GetRequiredVersion(browser, executablePath);
            }
            if (string.IsNullOrEmpty(version))
            {
                version = await GetVersion(browser, browsersFile);
                if (string.IsNullOrEmpty(version))
                {
                    throw new Exception("Version not found");
                }
            }

            if (string.IsNullOrEmpty(downloadPath))
            {
                //check if current OS is windows
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ms-playwright", $"{browser}-{version}");

                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    downloadPath = $"~/.cache/ms-playwright/{browser}-{version}";

                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.InternetCache), "ms-playwright", $"{browser}-{version}");
                }
                else
                {
                    throw new PlatformNotSupportedException("Platform not supported");
                }
            }
            else if(!downloadPath.Contains($"{browser}-{version}"))
            {
                downloadPath = Path.Combine(downloadPath, $"{browser}-{version}");
            }
            var fileUrl = GetDownloadPath(browser, version, platform);
            if (string.IsNullOrEmpty(fileUrl))
            {
                throw new Exception("Download path not found");
            }

            if(Directory.Exists(Path.Combine(downloadPath)) && File.Exists(Path.Combine(downloadPath, "INSTALLATION_COMPLETE")))
            {
                Trace.WriteLine("Browser already installed");
                progressCallback?.Report(100);
                return;
            }

            var tempDir = downloadPath.Substring(0, downloadPath.LastIndexOf(Path.DirectorySeparatorChar));
            if (!Directory.Exists(tempDir))
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
                                if (progressPercentage != 100)
                                {
                                    progressCallback?.Report(progressPercentage);
                                }
                            }
                        }
                    }
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    {
                        var unzip = new ProcessStartInfo
                        {
                            FileName = "unzip",
                            Arguments = $"-o {tempPath} -d {downloadPath}",
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            UseShellExecute = false,
                            CreateNoWindow = true,
                        };
                        var process = Process.Start(unzip);
                        process.StandardOutput.ReadToEnd();
                        process.StandardOutput.Close();
                        process.StandardError.ReadToEnd();
                        process.StandardError.Close();
                        process.WaitForExit();
                    }
                    else
                    {
                        if(Directory.Exists(downloadPath))
                        {
                            Directory.Delete(downloadPath, true);
                        }
                        ZipFile.ExtractToDirectory(tempPath, downloadPath);
                    }
                    File.Delete(tempPath);
                    File.Create(Path.Combine(downloadPath, "INSTALLATION_COMPLETE"));
                    progressCallback?.Report(100);
                }
            }
            Trace.WriteLine("Download completed successfully!");
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"Error: {ex.Message}");
            progressCallback?.Report(-1);
            throw;
        }
    }

    async Task<string> GetVersion(string browserName, string? browsersFile = null)
    {
        if (string.IsNullOrEmpty(browsersFile))
        {
            browsersFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "browsers.json");
        }
        if(!File.Exists(browsersFile))
        {
            browsersFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "browsers.json");
            Trace.WriteLine("Downloading browsers.json from repository");
            var content = await _httpClient.GetStringAsync(Constants.HostedBrowsersJson);
            File.WriteAllText(browsersFile, content);
        }

        var json = File.ReadAllText(browsersFile);
        JsonDocument document = JsonDocument.Parse(json);

        JsonElement browsersArray = document.RootElement.GetProperty("browsers");

        JsonElement? browser = browsersArray.EnumerateArray()
            .FirstOrDefault(browser => browser.GetProperty("name").GetString() == browserName);

        if (browser?.ValueKind != JsonValueKind.Undefined)
        {
            string? revision = browser?.GetProperty("revision").GetString() ?? string.Empty;
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
            var constant = Constants.Paths[browser][platform];
            var result = _cdns[0] + string.Format(constant, version);
            return _cdns[0] + string.Format(Constants.Paths[browser][platform], version);
        }
        return string.Empty;
    }
}
