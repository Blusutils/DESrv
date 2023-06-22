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
    /// <summary>
    /// Reads the config
    /// </summary>
    /// <param name="path">Path to config. If not set, path to current assembly used as default</param>
    /// <returns><see cref="IConfig"/> with readed data from file</returns>
    abstract static T? Read<T>(string? path = null) where T : IConfig;
    /// <summary>
    /// Dumps the config to string
    /// </summary>
    /// <returns>Config as string</returns>
    string? DumpToString();
    /// <summary>
    /// Dumps the config to file
    /// </summary>
    /// <param name="path">Path to config</param>
    void DumpToFile(string? path = null);
}