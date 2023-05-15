using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Blusutils.DESrv.Updater;
public static class Updater {
    static HttpClient cl = new ();
    static Updater() {
        cl.DefaultRequestHeaders.Accept.Clear();
        cl.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
        cl.DefaultRequestHeaders.Add("User-Agent", "DESrv Updater module");
    }

    public static Version? CheckVersion(Version version) {
        var d = cl.GetStringAsync("https://api.github.com/repos/Blusutils/DESrv/releases/latest").Result;
        var succ = Version.TryParse((JsonSerializer.Deserialize<JsonDocument>(d)?.RootElement.GetProperty("tag_name").GetString())?[1..], out Version? version1);
        if (!succ) version1 = null;
        return (version1??new Version(1,0,0)) > version ? version1 : null;
    }

    public static void Update() {
        var platform = Environment.OSVersion.Platform switch {
            PlatformID.Win32NT => "windows",
            PlatformID.Unix => "nix",
            PlatformID.MacOSX => "nix",
            _ => null
        };

        if (platform is null)
            throw new PlatformNotSupportedException("Your platform is currently not supported");

        var d = cl.GetAsync("https://api.github.com/repos/Blusutils/DESrv/releases/latest").Result.Content.ReadAsStringAsync().Result;
        var assets = JsonSerializer.Deserialize<Release>(d);

        string asset = "";
        foreach (var item in assets.assets) {
            if (item.name.ToLowerInvariant().Contains(platform)) {
                asset = item.browser_download_url;
                break;
            }
        }
        var stream = cl.GetStreamAsync(asset).Result;
        var filename = "update." + (platform == "nix" ? "tar.gz" : "zip");
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