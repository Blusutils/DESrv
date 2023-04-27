namespace Blusutils.DESrv.Configuration;

/// <summary>
/// Interface for configs
/// </summary>
public interface IConfig {
    /// <summary>
    /// Extend config class fields using dict
    /// </summary>
    /// <param name="config">Dict to use for extending</param>
    abstract void Extend(Dictionary<string, object> config);
}

/// <summary>
/// Base configuration model
/// </summary>
public class ConfigurationModel : IConfig {

    /// <inheritdoc/>
    public void Extend(Dictionary<string, object> config) {
        var flist = new List<string>();
        foreach (var finfo in GetType().GetFields()) {
            flist.Add(finfo.Name);
        }
        foreach (var key in config.Keys) {
            if (flist.Contains(key)) {
                var field = GetType().GetField(key);
                field?.SetValue(this, field.FieldType == typeof(string) ? config[key] : Convert.ChangeType(config[key], field.FieldType));
            }
        }
    }
    /// <summary>
    /// Reads the config
    /// </summary>
    /// <param name="path">Path to config. If not set, path to current assembly used as default</param>
    /// <returns><see cref="ConfigurationModel"/> with readed data from JSON file</returns>
    public static T? Read<T>(string? path = null) where T : ConfigurationModel {
        path ??= Path.Combine(
            Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? "." + Path.DirectorySeparatorChar,
            "config.json");
        if (!File.Exists(path)) {
            using StreamWriter file = new(File.Create(path));
            file.WriteLine("{}");
            file.Close();
        }
        using StreamReader r = new StreamReader(path);
        return System.Text.Json.JsonSerializer.Deserialize<T>(r.ReadToEnd());
    }
    /// <summary>
    /// Dumps the config to string
    /// </summary>
    /// <returns>Serialized config in JSON format</returns>
    public string? Dump() {
        return System.Text.Json.JsonSerializer.Serialize(this);
    }
    /// <summary>
    /// Dumps the config to file
    /// </summary>
    /// <param name="path">Path to config. If not set, path to current assembly used as default</param>
    public void Dump(string? path = null) {
        path ??= Path.Combine(
            Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? "." + Path.DirectorySeparatorChar,
            "config.json");
        StreamWriter? sw = null;

        if (!File.Exists(path))
            sw = new(File.Create(path));

        sw ??= new (path);
        sw.Write(Dump());
    }
}
