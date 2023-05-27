using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Blusutils.DESrv.Updater;

/// <summary>
/// Performs updates for DESrv
/// </summary>
public static class Updater {

    static HttpClient client = new ();

    static Updater() {
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
        client.DefaultRequestHeaders.Add("User-Agent", "DESrv Updater module");
    }

    /// <summary>
    /// Compares latest version with passed
    /// </summary>
    /// <param name="version">Version object to compare</param>
    /// <returns>Latest version, if it greater than passed, else null</returns>
    public static Version? CheckVersion(Version version) {
        var resp = client.GetStringAsync("https://api.github.com/repos/Blusutils/DESrv/releases/latest").Result;
        var succ = Version.TryParse((JsonSerializer.Deserialize<JsonDocument>(resp)?.RootElement.GetProperty("tag_name").GetString())?[1..], out Version? version1);
        if (!succ) version1 = null;
        return (version1??new Version(1,0,0)) > version ? version1 : null;
    }

    /// <summary>
    /// Downloads latest update and unarchives it
    /// </summary>
    /// <exception cref="PlatformNotSupportedException">If current .NET platform is not either Windows or *nix (Linux, macOS)</exception>
    /// <exception cref="HttpRequestException">If API call failed (status code isn't OK 200)</exception>
    public static void Update() {
        var platform = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "win" : RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "nix": RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "mac" : throw new PlatformNotSupportedException("your platform is currently not supported");

        var res = client.GetAsync("https://api.github.com/repos/Blusutils/DESrv/releases/latest").Result;

        if (res is null || res.StatusCode != System.Net.HttpStatusCode.OK)
            throw new HttpRequestException("failed to fetch update information");

        var data = res.Content.ReadAsStringAsync().Result;
        var release = JsonSerializer.Deserialize<Release>(data)!;

        string asset = "";
        foreach (var item in release.assets) {
            if (item.name.ToLowerInvariant().Contains(platform)) {
                asset = item.browser_download_url;
                break;
            }
        }
        var stream = client.GetStreamAsync(asset).Result;
        var filename = "update." + (platform == "nix" || platform == "mac" ? "tar.gz" : "zip");
        if (File.Exists(filename))
            File.Delete(filename);
        using (var fileStream = new FileStream(filename, FileMode.Create)) {
            stream.CopyTo(fileStream);
        }

        using var upd = ZipFile.Open(filename, ZipArchiveMode.Read);
        foreach (var entry in upd.Entries) {
            if (entry.Name == "" && !Directory.Exists(entry.FullName))
                Directory.CreateDirectory(entry.FullName);
            else
                entry.ExtractToFile(entry.FullName, true);
        }
    }
}

#region GitHub API classes
#pragma warning disable CS8618
public class Release {
    public string url { get; set; }
    public string html_url { get; set; }
    public string assets_url { get; set; }
    public string upload_url { get; set; }
    public string tarball_url { get; set; }
    public string zipball_url { get; set; }
    public string discussion_url { get; set; }
    public int id { get; set; }
    public string node_id { get; set; }
    public string tag_name { get; set; }
    public string target_commitish { get; set; }
    public string name { get; set; }
    public string body { get; set; }
    public bool draft { get; set; }
    public bool prerelease { get; set; }
    public DateTime created_at { get; set; }
    public DateTime published_at { get; set; }
    public object author { get; set; }
    public List<Asset> assets { get; set; }
}

public class Asset {
    public string url { get; set; }
    public string browser_download_url { get; set; }
    public int id { get; set; }
    public string node_id { get; set; }
    public string name { get; set; }
    public string label { get; set; }
    public string state { get; set; }
    public string content_type { get; set; }
    public int size { get; set; }
    public int download_count { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public object uploader { get; set; }
}
#pragma warning restore CS8618
#endregion