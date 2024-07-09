using System;
using System.Collections.Generic;

namespace nor0x.Playwright.BrowserDownloader;
public static class Constants
{
	public static string HostedBrowsersJson = "https://raw.githubusercontent.com/nor0x/Playwright.BrowserDownloader/main/Playwright.BrowserDownloader/browsers.json";

	public static Dictionary<string, Dictionary<string, string>> ExecutablePaths = new Dictionary<string, Dictionary<string, string>>
		{
			{
				"chromium", new Dictionary<string, string>
				{
					{ "linux", "chrome-linux, chrome" },
					{ "mac", "chrome-mac, Chromium.app, Contents, MacOS, Chromium" },
					{ "win", "chrome-win, chrome.exe" }
				}
			},
			{
				"firefox", new Dictionary<string, string>
				{
					{ "linux", "firefox, firefox" },
					{ "mac", "firefox, Nightly.app, Contents, MacOS, firefox" },
					{ "win", "firefox, firefox.exe" }
				}
			},
			{
				"webkit", new Dictionary<string, string>
				{
					{ "linux", "pw_run.sh" },
					{ "mac", "pw_run.sh" },
					{ "win", "Playwright.exe" }
				}
			},
			{
				"ffmpeg", new Dictionary<string, string>
				{
					{ "linux", "ffmpeg-linux" },
					{ "mac", "ffmpeg-mac" },
					{ "win", "ffmpeg-win64.exe" }
				}
			}
		};

	//from https://github.com/microsoft/playwright/blob/main/packages/playwright-core/src/server/registry/index.ts
	public static Dictionary<string, Dictionary<string, string>> Paths = new() {
	{
	  "chromium",
	  new Dictionary < string,
	  string > {
		["<unknown>"] = null,
		["ubuntu18.04-x64"] = null,
		["ubuntu20.04-x64"] = "builds/chromium/{0}/chromium-linux.zip",
		["ubuntu22.04-x64"] = "builds/chromium/{0}/chromium-linux.zip",
		["ubuntu24.04-x64"] = "builds/chromium/{0}/chromium-linux.zip",
		["ubuntu18.04-arm64"] = null,
		["ubuntu20.04-arm64"] = "builds/chromium/{0}/chromium-linux-arm64.zip",
		["ubuntu22.04-arm64"] = "builds/chromium/{0}/chromium-linux-arm64.zip",
		["ubuntu24.04-arm64"] = "builds/chromium/{0}/chromium-linux-arm64.zip",
		["debian11-x64"] = "builds/chromium/{0}/chromium-linux.zip",
		["debian11-arm64"] = "builds/chromium/{0}/chromium-linux-arm64.zip",
		["debian12-x64"] = "builds/chromium/{0}/chromium-linux.zip",
		["debian12-arm64"] = "builds/chromium/{0}/chromium-linux-arm64.zip",
		["mac10.13"] = "builds/chromium/{0}/chromium-mac.zip",
		["mac10.14"] = "builds/chromium/{0}/chromium-mac.zip",
		["mac10.15"] = "builds/chromium/{0}/chromium-mac.zip",
		["mac11"] = "builds/chromium/{0}/chromium-mac.zip",
		["mac11-arm64"] = "builds/chromium/{0}/chromium-mac-arm64.zip",
		["mac12"] = "builds/chromium/{0}/chromium-mac.zip",
		["mac12-arm64"] = "builds/chromium/{0}/chromium-mac-arm64.zip",
		["mac13"] = "builds/chromium/{0}/chromium-mac.zip",
		["mac13-arm64"] = "builds/chromium/{0}/chromium-mac-arm64.zip",
		["mac14"] = "builds/chromium/{0}/chromium-mac.zip",
		["mac14-arm64"] = "builds/chromium/{0}/chromium-mac-arm64.zip",
		["win64"] = "builds/chromium/{0}/chromium-win64.zip",
	  }
	}, {
	  "chromium-tip-of-tree",
	  new Dictionary < string,
	  string > {
		["<unknown>"] = null,
		["ubuntu18.04-x64"] = null,
		["ubuntu20.04-x64"] = "builds/chromium-tip-of-tree/{0}/chromium-tip-of-tree-linux.zip",
		["ubuntu22.04-x64"] = "builds/chromium-tip-of-tree/{0}/chromium-tip-of-tree-linux.zip",
		["ubuntu24.04-x64"] = "builds/chromium-tip-of-tree/{0}/chromium-tip-of-tree-linux.zip",
		["ubuntu18.04-arm64"] = null,
		["ubuntu20.04-arm64"] = "builds/chromium-tip-of-tree/{0}/chromium-tip-of-tree-linux-arm64.zip",
		["ubuntu22.04-arm64"] = "builds/chromium-tip-of-tree/{0}/chromium-tip-of-tree-linux-arm64.zip",
		["ubuntu24.04-arm64"] = "builds/chromium-tip-of-tree/{0}/chromium-tip-of-tree-linux-arm64.zip",
		["debian11-x64"] = "builds/chromium-tip-of-tree/{0}/chromium-tip-of-tree-linux.zip",
		["debian11-arm64"] = "builds/chromium-tip-of-tree/{0}/chromium-tip-of-tree-linux-arm64.zip",
		["debian12-x64"] = "builds/chromium-tip-of-tree/{0}/chromium-tip-of-tree-linux.zip",
		["debian12-arm64"] = "builds/chromium-tip-of-tree/{0}/chromium-tip-of-tree-linux-arm64.zip",
		["mac10.13"] = "builds/chromium-tip-of-tree/{0}/chromium-tip-of-tree-mac.zip",
		["mac10.14"] = "builds/chromium-tip-of-tree/{0}/chromium-tip-of-tree-mac.zip",
		["mac10.15"] = "builds/chromium-tip-of-tree/{0}/chromium-tip-of-tree-mac.zip",
		["mac11"] = "builds/chromium-tip-of-tree/{0}/chromium-tip-of-tree-mac.zip",
		["mac11-arm64"] = "builds/chromium-tip-of-tree/{0}/chromium-tip-of-tree-mac-arm64.zip",
		["mac12"] = "builds/chromium-tip-of-tree/{0}/chromium-tip-of-tree-mac.zip",
		["mac12-arm64"] = "builds/chromium-tip-of-tree/{0}/chromium-tip-of-tree-mac-arm64.zip",
		["mac13"] = "builds/chromium-tip-of-tree/{0}/chromium-tip-of-tree-mac.zip",
		["mac13-arm64"] = "builds/chromium-tip-of-tree/{0}/chromium-tip-of-tree-mac-arm64.zip",
		["mac14"] = "builds/chromium-tip-of-tree/{0}/chromium-tip-of-tree-mac.zip",
		["mac14-arm64"] = "builds/chromium-tip-of-tree/{0}/chromium-tip-of-tree-mac-arm64.zip",
		["win64"] = "builds/chromium-tip-of-tree/{0}/chromium-tip-of-tree-win64.zip",
	  }
	}, {
	  "firefox",
	  new Dictionary < string,
	  string > {
		["<unknown>"] = null,
		["ubuntu18.04-x64"] = null,
		["ubuntu20.04-x64"] = "builds/firefox/{0}/firefox-ubuntu-20.04.zip",
		["ubuntu22.04-x64"] = "builds/firefox/{0}/firefox-ubuntu-22.04.zip",
		["ubuntu24.04-x64"] = "builds/firefox/{0}/firefox-ubuntu-24.04.zip",
		["ubuntu18.04-arm64"] = null,
		["ubuntu20.04-arm64"] = "builds/firefox/{0}/firefox-ubuntu-20.04-arm64.zip",
		["ubuntu22.04-arm64"] = "builds/firefox/{0}/firefox-ubuntu-22.04-arm64.zip",
		["ubuntu24.04-arm64"] = "builds/firefox/{0}/firefox-ubuntu-24.04-arm64.zip",
		["debian11-x64"] = "builds/firefox/{0}/firefox-debian-11.zip",
		["debian11-arm64"] = "builds/firefox/{0}/firefox-debian-11-arm64.zip",
		["debian12-x64"] = "builds/firefox/{0}/firefox-debian-12.zip",
		["debian12-arm64"] = "builds/firefox/{0}/firefox-debian-12-arm64.zip",
		["mac10.13"] = "builds/firefox/{0}/firefox-mac-13.zip",
		["mac10.14"] = "builds/firefox/{0}/firefox-mac-13.zip",
		["mac10.15"] = "builds/firefox/{0}/firefox-mac-13.zip",
		["mac11"] = "builds/firefox/{0}/firefox-mac-13.zip",
		["mac11-arm64"] = "builds/firefox/{0}/firefox-mac-13-arm64.zip",
		["mac12"] = "builds/firefox/{0}/firefox-mac-13.zip",
		["mac12-arm64"] = "builds/firefox/{0}/firefox-mac-13-arm64.zip",
		["mac13"] = "builds/firefox/{0}/firefox-mac-13.zip",
		["mac13-arm64"] = "builds/firefox/{0}/firefox-mac-13-arm64.zip",
		["mac14"] = "builds/firefox/{0}/firefox-mac-13.zip",
		["mac14-arm64"] = "builds/firefox/{0}/firefox-mac-13-arm64.zip",
		["win64"] = "builds/firefox/{0}/firefox-win64.zip",
	  }
	}, {
	  "firefox-beta",
	  new Dictionary < string,
	  string > {
		["<unknown>"] = null,
		["ubuntu18.04-x64"] = null,
		["ubuntu20.04-x64"] = "builds/firefox-beta/{0}/firefox-beta-ubuntu-20.04.zip",
		["ubuntu22.04-x64"] = "builds/firefox-beta/{0}/firefox-beta-ubuntu-22.04.zip",
		["ubuntu24.04-x64"] = "builds/firefox-beta/{0}/firefox-beta-ubuntu-24.04.zip",
		["ubuntu18.04-arm64"] = null,
		["ubuntu20.04-arm64"] = null,
		["ubuntu22.04-arm64"] = "builds/firefox-beta/{0}/firefox-beta-ubuntu-22.04-arm64.zip",
		["ubuntu24.04-arm64"] = "builds/firefox-beta/{0}/firefox-beta-ubuntu-24.04-arm64.zip",
		["debian11-x64"] = "builds/firefox-beta/{0}/firefox-beta-debian-11.zip",
		["debian11-arm64"] = "builds/firefox-beta/{0}/firefox-beta-debian-11-arm64.zip",
		["debian12-x64"] = "builds/firefox-beta/{0}/firefox-beta-debian-12.zip",
		["debian12-arm64"] = "builds/firefox-beta/{0}/firefox-beta-debian-12-arm64.zip",
		["mac10.13"] = "builds/firefox-beta/{0}/firefox-beta-mac-13.zip",
		["mac10.14"] = "builds/firefox-beta/{0}/firefox-beta-mac-13.zip",
		["mac10.15"] = "builds/firefox-beta/{0}/firefox-beta-mac-13.zip",
		["mac11"] = "builds/firefox-beta/{0}/firefox-beta-mac-13.zip",
		["mac11-arm64"] = "builds/firefox-beta/{0}/firefox-beta-mac-13-arm64.zip",
		["mac12"] = "builds/firefox-beta/{0}/firefox-beta-mac-13.zip",
		["mac12-arm64"] = "builds/firefox-beta/{0}/firefox-beta-mac-13-arm64.zip",
		["mac13"] = "builds/firefox-beta/{0}/firefox-beta-mac-13.zip",
		["mac13-arm64"] = "builds/firefox-beta/{0}/firefox-beta-mac-13-arm64.zip",
		["mac14"] = "builds/firefox-beta/{0}/firefox-beta-mac-13.zip",
		["mac14-arm64"] = "builds/firefox-beta/{0}/firefox-beta-mac-13-arm64.zip",
		["win64"] = "builds/firefox-beta/{0}/firefox-beta-win64.zip",
	  }
	}, {
	  "webkit",
	  new Dictionary < string,
	  string > {
		["<unknown>"] = null,
		["ubuntu18.04-x64"] = null,
		["ubuntu20.04-x64"] = "builds/webkit/{0}/webkit-ubuntu-20.04.zip",
		["ubuntu22.04-x64"] = "builds/webkit/{0}/webkit-ubuntu-22.04.zip",
		["ubuntu24.04-x64"] = "builds/webkit/{0}/webkit-ubuntu-24.04.zip",
		["ubuntu18.04-arm64"] = null,
		["ubuntu20.04-arm64"] = "builds/webkit/{0}/webkit-ubuntu-20.04-arm64.zip",
		["ubuntu22.04-arm64"] = "builds/webkit/{0}/webkit-ubuntu-22.04-arm64.zip",
		["ubuntu24.04-arm64"] = "builds/webkit/{0}/webkit-ubuntu-24.04-arm64.zip",
		["debian11-x64"] = "builds/webkit/{0}/webkit-debian-11.zip",
		["debian11-arm64"] = "builds/webkit/{0}/webkit-debian-11-arm64.zip",
		["debian12-x64"] = "builds/webkit/{0}/webkit-debian-12.zip",
		["debian12-arm64"] = "builds/webkit/{0}/webkit-debian-12-arm64.zip",
		["mac10.13"] = null,
		["mac10.14"] = "builds/deprecated-webkit-mac-10.14/{0}/deprecated-webkit-mac-10.14.zip",
		["mac10.15"] = "builds/deprecated-webkit-mac-10.15/{0}/deprecated-webkit-mac-10.15.zip",
		["mac11"] = "builds/webkit/{0}/webkit-mac-11.zip",
		["mac11-arm64"] = "builds/webkit/{0}/webkit-mac-11-arm64.zip",
		["mac12"] = "builds/webkit/{0}/webkit-mac-12.zip",
		["mac12-arm64"] = "builds/webkit/{0}/webkit-mac-12-arm64.zip",
		["mac13"] = "builds/webkit/{0}/webkit-mac-13.zip",
		["mac13-arm64"] = "builds/webkit/{0}/webkit-mac-13-arm64.zip",
		["mac14"] = "builds/webkit/{0}/webkit-mac-14.zip",
		["mac14-arm64"] = "builds/webkit/{0}/webkit-mac-14-arm64.zip",
		["win64"] = "builds/webkit/{0}/webkit-win64.zip",
	  }
	}, {
	  "ffmpeg",
	  new Dictionary < string,
	  string > {
		["<unknown>"] = null,
		["ubuntu18.04-x64"] = null,
		["ubuntu20.04-x64"] = "builds/ffmpeg/{0}/ffmpeg-linux.zip",
		["ubuntu22.04-x64"] = "builds/ffmpeg/{0}/ffmpeg-linux.zip",
		["ubuntu24.04-x64"] = "builds/ffmpeg/{0}/ffmpeg-linux.zip",
		["ubuntu18.04-arm64"] = null,
		["ubuntu20.04-arm64"] = "builds/ffmpeg/{0}/ffmpeg-linux-arm64.zip",
		["ubuntu22.04-arm64"] = "builds/ffmpeg/{0}/ffmpeg-linux-arm64.zip",
		["ubuntu24.04-arm64"] = "builds/ffmpeg/{0}/ffmpeg-linux-arm64.zip",
		["debian11-x64"] = "builds/ffmpeg/{0}/ffmpeg-linux.zip",
		["debian11-arm64"] = "builds/ffmpeg/{0}/ffmpeg-linux-arm64.zip",
		["debian12-x64"] = "builds/ffmpeg/{0}/ffmpeg-linux.zip",
		["debian12-arm64"] = "builds/ffmpeg/{0}/ffmpeg-linux-arm64.zip",
		["mac10.13"] = "builds/ffmpeg/{0}/ffmpeg-mac.zip",
		["mac10.14"] = "builds/ffmpeg/{0}/ffmpeg-mac.zip",
		["mac10.15"] = "builds/ffmpeg/{0}/ffmpeg-mac.zip",
		["mac11"] = "builds/ffmpeg/{0}/ffmpeg-mac.zip",
		["mac11-arm64"] = "builds/ffmpeg/{0}/ffmpeg-mac-arm64.zip",
		["mac12"] = "builds/ffmpeg/{0}/ffmpeg-mac.zip",
		["mac12-arm64"] = "builds/ffmpeg/{0}/ffmpeg-mac-arm64.zip",
		["mac13"] = "builds/ffmpeg/{0}/ffmpeg-mac.zip",
		["mac13-arm64"] = "builds/ffmpeg/{0}/ffmpeg-mac-arm64.zip",
		["mac14"] = "builds/ffmpeg/{0}/ffmpeg-mac.zip",
		["mac14-arm64"] = "builds/ffmpeg/{0}/ffmpeg-mac-arm64.zip",
		["win64"] = "builds/ffmpeg/{0}/ffmpeg-win64.zip",
	  }
	}, {
	  "android",
	  new Dictionary < string,
	  string > {
		["<unknown>"] = "builds/android/{0}/android.zip",
		["ubuntu18.04-x64"] = null,
		["ubuntu20.04-x64"] = "builds/android/{0}/android.zip",
		["ubuntu22.04-x64"] = "builds/android/{0}/android.zip",
		["ubuntu24.04-x64"] = "builds/android/{0}/android.zip",
		["ubuntu18.04-arm64"] = null,
		["ubuntu20.04-arm64"] = "builds/android/{0}/android.zip",
		["ubuntu22.04-arm64"] = "builds/android/{0}/android.zip",
		["ubuntu24.04-arm64"] = "builds/android/{0}/android.zip",
		["debian11-x64"] = "builds/android/{0}/android.zip",
		["debian11-arm64"] = "builds/android/{0}/android.zip",
		["debian12-x64"] = "builds/android/{0}/android.zip",
		["debian12-arm64"] = "builds/android/{0}/android.zip",
		["mac10.13"] = "builds/android/{0}/android.zip",
		["mac10.14"] = "builds/android/{0}/android.zip",
		["mac10.15"] = "builds/android/{0}/android.zip",
		["mac11"] = "builds/android/{0}/android.zip",
		["mac11-arm64"] = "builds/android/{0}/android.zip",
		["mac12"] = "builds/android/{0}/android.zip",
		["mac12-arm64"] = "builds/android/{0}/android.zip",
		["mac13"] = "builds/android/{0}/android.zip",
		["mac13-arm64"] = "builds/android/{0}/android.zip",
		["mac14"] = "builds/android/{0}/android.zip",
		["mac14-arm64"] = "builds/android/{0}/android.zip",
		["win64"] = "builds/android/{0}/android.zip",
	  }
	}
  };
}

public enum TargetPlatform
{
	Unknown,
	Ubuntu1804x64,
	Ubuntu2004x64,
	Ubuntu2204x64,
	Ubuntu2404x64,
	Ubuntu1804arm64,
	Ubuntu2004arm64,
	Ubuntu2204arm64,
	Ubuntu2404arm64,
	Debian11x64,
	Debian11arm64,
	Debian12x64,
	Debian12arm64,
	Mac1013,
	Mac1014,
	Mac1015,
	Mac11,
	Mac11arm64,
	Mac12,
	Mac12arm64,
	Mac13,
	Mac13arm64,
	Mac14,
	Mac14arm64,
	Win64,
}

public enum BrowserInfo
{
	Chromium,
	ChromiumTipOfTree,
	Firefox,
	FirefoxBeta,
	Webkit,
	Ffmpeg,
	Android
}

public static class Extensions
{
	public static string ToReadableString(this TargetPlatform platform)
	{
		switch (platform)
		{
			case TargetPlatform.Unknown:
				return "<unknown>";
			case TargetPlatform.Ubuntu1804x64:
				return "ubuntu18.04-x64";
			case TargetPlatform.Ubuntu2004x64:
				return "ubuntu20.04-x64";
			case TargetPlatform.Ubuntu2204x64:
				return "ubuntu22.04-x64";
			case TargetPlatform.Ubuntu1804arm64:
				return "ubuntu18.04-arm64";
			case TargetPlatform.Ubuntu2004arm64:
				return "ubuntu20.04-arm64";
			case TargetPlatform.Ubuntu2204arm64:
				return "ubuntu22.04-arm64";
			case TargetPlatform.Ubuntu2404x64:
				return "ubuntu24.04-x64";
			case TargetPlatform.Ubuntu2404arm64:
				return "ubuntu24.04-arm64";
			case TargetPlatform.Debian11x64:
				return "debian11-x64";
			case TargetPlatform.Debian11arm64:
				return "debian11-arm64";
			case TargetPlatform.Debian12x64:
				return "debian12-x64";
			case TargetPlatform.Debian12arm64:
				return "debian12-arm64";
			case TargetPlatform.Mac1013:
				return "mac10.13";
			case TargetPlatform.Mac1014:
				return "mac10.14";
			case TargetPlatform.Mac1015:
				return "mac10.15";
			case TargetPlatform.Mac11:
				return "mac11";
			case TargetPlatform.Mac11arm64:
				return "mac11-arm64";
			case TargetPlatform.Mac12:
				return "mac12";
			case TargetPlatform.Mac12arm64:
				return "mac12-arm64";
			case TargetPlatform.Mac13:
				return "mac13";
			case TargetPlatform.Mac13arm64:
				return "mac13-arm64";
			case TargetPlatform.Mac14:
				return "mac14";
			case TargetPlatform.Mac14arm64:
				return "mac14-arm64";
			case TargetPlatform.Win64:
				return "win64";
			default:
				throw new ArgumentOutOfRangeException(nameof(platform), platform, null);
		}

	}

	public static string ToReadableString(this BrowserInfo browserType)
	{
		switch (browserType)
		{
			case BrowserInfo.Chromium:
				return "chromium";
			case BrowserInfo.ChromiumTipOfTree:
				return "chromium-tip-of-tree";
			case BrowserInfo.Firefox:
				return "firefox";
			case BrowserInfo.FirefoxBeta:
				return "firefox-beta";
			case BrowserInfo.Webkit:
				return "webkit";
			case BrowserInfo.Ffmpeg:
				return "ffmpeg";
			case BrowserInfo.Android:
				return "android";
			default:
				throw new ArgumentOutOfRangeException(nameof(browserType), browserType, null);
		}
	}
}
