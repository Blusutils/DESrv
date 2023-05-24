using Blusutils.DESrv.Logging.Utils;
using NLua;
using NLua.Exceptions;

namespace Blusutils.DESrv.LuaScriptingApi;

/// <summary>
/// Lua scripts loader
/// </summary>
public static class LuaLoader {

    static string scriptsPath = "scripts";
    /// <summary>
    /// Scripts cache
    /// </summary>
    public static Dictionary<string, ExtensionLuaScriptModel> Cache { get; private set; } = new();

    /// <summary>
    /// Load Lua script to memory
    /// </summary>
    /// <param name="scriptName">Script file name withount extension</param>
    /// <param name="path">Path to script (pass to override)</param>
    /// <returns>Script model instance</returns>
    public static ExtensionLuaScriptModel Load(string scriptName, string? path = null) {

        if (Cache.TryGetValue(scriptName, out var value))
            return value;

        path ??= Path.Combine(scriptsPath, scriptName+".lua");

        var lua = new Lua();
        lua.DoFile(path);

        var meta = lua.GetTable("meta");
        var func = lua.GetTable("func");

        if (meta is null || func is null) {
            var msg = $"{0} is null; table '{0}' not found in Lua script '{1}'";
            throw new LuaException(
                "failed to properly load script in case of missing 'meta' or 'func' table",
                new NullReferenceException(meta is null ? msg.Format("meta", scriptName) : func is null ? msg.Format("func", scriptName) : "")
                );
        }

        var script = new ExtensionLuaScriptModel {
            Name = meta?["name"] as string ?? scriptName,
            Author = meta?["author"] as string ?? "unknown author",
            ExtensionID = meta?["extension"] as string ?? "*",
            Link = meta?["link"] as string ?? "",
            Version = new(meta?["version"] as string??"1.0.0"),
            MinimalExtensionVersion = new(meta?["minimalExtensionVersion"] as string ?? "0.0.0"),
            InfoTable = meta!,
            ExecutableSpaceTable = func!
        };

        Cache[scriptName] = script;

        return script;
    }

    /// <summary>
    /// Get Lua script from cache or return null
    /// </summary>
    /// <param name="scriptName">Script file name withount extension</param>
    /// <returns>Script model instance or null</returns>
    public static ExtensionLuaScriptModel? GetScriptOrDefault(string scriptName) {
        if (Cache.TryGetValue(scriptName, out var value))
            return value;
        else
            return null;
    }

    /// <summary>
    /// Get Lua script from cache or load it
    /// </summary>
    /// <param name="scriptName">Script file name withount extension</param>
    /// <returns>Script model instance</returns>
    public static ExtensionLuaScriptModel GetScriptOrLoad(string scriptName) {
        if (Cache.TryGetValue(scriptName, out var value))
            return value;
        else
            return Load(scriptName);
    }

}
