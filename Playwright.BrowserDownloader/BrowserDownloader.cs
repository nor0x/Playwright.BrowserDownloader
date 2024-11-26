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

	/// <summary>
	/// gets a required version from the executable path from a playwright object
	/// </summary>
	/// <param name="browser">see nor0x.Playwright.BrowserDownloader.BrowserInfo</param>
	/// <param name="executablePath">executable path from playwright object</param>
	/// <returns>version string for the specified browser</returns>
	public string GetRequiredVersion(string browser, string executablePath)
	{
		if (executablePath.Contains($"{browser}-"))
		{
			string[] segments = executablePath.Split(System.IO.Path.DirectorySeparatorChar);
			var browserAndVersion = segments.FirstOrDefault(segment => segment.Contains($"{browser}-"));
			var version = browserAndVersion.Split('-')[1];
			return version;
		}
		return string.Empty;
	}

	/// <summary>
	/// downloads the specified browser with a specific version for the specified platform
	/// </summary>
	/// <param name="browserType">see nor0x.Playwright.BrowserDownloader.BrowserInfo</param>
	/// <param name="targetPlatform">see nor0x.Playwright.BrowserDownloader.TargetPlatform</param>
	/// <param name="version">version string see browsers.json for examples</param>
	/// <param name="executablePath">optional executable path from a playwright object</param>
	/// <param name="browsersFile">optional custom browser definition file</param>
	/// <param name="downloadPath">optional download path where the browser will be downloaded</param>
	/// <param name="progressCallback">optional progress callback</param>
	/// <returns>executable path of the downloaded browser</returns>
	public async Task<string> DownloadBrowserAsync(
		BrowserInfo browserType,
		TargetPlatform targetPlatform,
		string? version = null,
		string? executablePath = null,
		string? browsersFile = null,
		string? downloadPath = null,
		IProgress<double>? progressCallback = null)
	{
		try
		{
			var browser = browserType.ToReadableString();
			var platform = targetPlatform.ToReadableString();
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
				if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
				{
					downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ms-playwright", $"{browser}-{version}");

				}
				else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
				{
					downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".cache", "ms-playwright", $"{browser}-{version}");
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
			else if (!downloadPath.Contains($"{browser}-{version}"))
			{
				downloadPath = Path.Combine(downloadPath, $"{browser}-{version}");
			}
			var downloadLink = GetDownloadLink(browser, version, platform);
			if (string.IsNullOrEmpty(downloadLink))
			{
				throw new Exception("Download path not found");
			}

			var execPath = GetExecutablePath(browser, platform);
			var fullExecutablePath = Path.Combine(downloadPath, execPath);
			bool platformExists = false;
			if (File.Exists(Path.Combine(downloadPath, "INSTALLATION_COMPLETE")))
			{
				var content = File.ReadAllText(Path.Combine(downloadPath, "INSTALLATION_COMPLETE"));
				platformExists = content.Contains(platform);
			}

			if (Directory.Exists(Path.Combine(downloadPath)) && platformExists && File.Exists(fullExecutablePath))
			{
				Trace.WriteLine("Browser already installed");
				progressCallback?.Report(100);
				return fullExecutablePath;
			}

			var tempDir = downloadPath.Substring(0, downloadPath.LastIndexOf(Path.DirectorySeparatorChar));
			if (!Directory.Exists(tempDir))
			{
				Directory.CreateDirectory(tempDir);
			}
			var fileName = downloadLink.Substring(downloadLink.LastIndexOf('/') + 1);
			var tempPath = Path.Combine(tempDir, fileName);
			using (HttpResponseMessage response = await _httpClient.GetAsync(downloadLink, HttpCompletionOption.ResponseHeadersRead))
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
						if (Directory.Exists(downloadPath))
						{
							Directory.Delete(downloadPath, true);
						}
						ZipFile.ExtractToDirectory(tempPath, downloadPath);
					}
					File.Delete(tempPath);
					File.WriteAllText(Path.Combine(downloadPath, "INSTALLATION_COMPLETE"), platform);
					progressCallback?.Report(100);
					Trace.WriteLine("Download completed successfully!");
					return fullExecutablePath;
				}
			}
		}
		catch (Exception ex)
		{
			Trace.WriteLine($"Error: {ex.Message}");
			progressCallback?.Report(-1);
			throw;
		}
	}


	/// <summary>
	///  gets the version of the specified browser from browsers.json or specified browsers file
	/// </summary>
	/// <param name="browser">see nor0x.Playwright.BrowserDownloader.BrowserInfo</param>
	/// <param name="browsersFile">optional custom browser definition file</param>
	/// <returns>version string</returns>
	/// <exception cref="FileNotFoundException"></exception>
	async Task<string> GetVersion(string browser, string? browsersFile = null)
	{
		if (string.IsNullOrEmpty(browsersFile))
		{
			browsersFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "browsers.json");
		}
		if (!File.Exists(browsersFile))
		{
			browsersFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "browsers.json");
			Trace.WriteLine("Downloading browsers.json from repository");
			var content = await _httpClient.GetStringAsync(Constants.HostedBrowsersJson);
			File.WriteAllText(browsersFile, content);
		}

		var json = File.ReadAllText(browsersFile);
		JsonDocument document = JsonDocument.Parse(json);

		JsonElement browsersArray = document.RootElement.GetProperty("browsers");

		JsonElement? browserElement = browsersArray.EnumerateArray()
			.FirstOrDefault(b => b.GetProperty("name").GetString() == browser);

		if (browserElement?.ValueKind != JsonValueKind.Undefined)
		{
			string? revision = browserElement?.GetProperty("revision").GetString() ?? string.Empty;
			return revision;
		}
		else
		{
			throw new FileNotFoundException($"Browser {browser} not found in browsers.json");
		}
	}

	/// <summary>
	/// returns a download link for the specified browser, version and platform
	/// </summary>
	/// <param name="browser">see nor0x.Playwright.BrowserDownloader.BrowserInfo</param>
	/// <param name="version">browser version</param>
	/// <param name="platform">see nor0x.Playwright.BrowserDownloader.TargetPlatform</param>
	/// <returns>download link for the specified browser, version and platform</returns>
	string GetDownloadLink(string browser, string version, string platform)
	{
		if (Constants.Paths.ContainsKey(browser) && Constants.Paths[browser].ContainsKey(platform))
		{
			var constant = Constants.Paths[browser][platform];
			var result = _cdns[0] + string.Format(constant, version);
			return _cdns[0] + string.Format(Constants.Paths[browser][platform], version);
		}
		return string.Empty;
	}

	/// <summary>
	/// returns the relative path to the browser executable in the download folder
	/// </summary>
	/// <param name="browser">chrome, firefox, webkit</param>
	/// <param name="version">see browsers.json</param>
	/// <param name="platform"></param>
	/// <returns></returns>
	/// <exception cref="PlatformNotSupportedException"></exception>
	string GetExecutablePath(string browser, string platform)
	{
		if (platform.Contains("ubuntu") || platform.Contains("debian"))
		{
			platform = "linux";
		}
		else if (platform.Contains("mac"))
		{
			platform = "mac";
		}
		else if (platform.Contains("win"))
		{
			platform = "win";
		}
		else
		{
			throw new PlatformNotSupportedException("Platform not supported");
		}

		if (browser.Contains("-"))
		{
			browser = browser.Split('-')[0];
		}

		if (Constants.ExecutablePaths.TryGetValue(browser, out Dictionary<string, string> outPaths))
		{
			if (outPaths.TryGetValue(platform, out string execPath))
			{
				return execPath.Replace(", ", Path.DirectorySeparatorChar.ToString());
			}
		}
		return string.Empty;
	}
}
