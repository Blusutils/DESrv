namespace Blusutils.DESrv.LuaScriptingApi;

/// <summary>
/// Lua scripts loader
/// </summary>
public class LuaLoader { // TODO docs and implementation

    string scriptsPath;

    /// <summary>
    /// Lua loader constructor
    /// </summary>
    /// <param name="luaScriptsPath">Path to Lua scripts</param>
    public LuaLoader(string luaScriptsPath) {
        scriptsPath = luaScriptsPath;
    }

    /// <summary>
    /// Load Lua script to memory
    /// </summary>
    /// <param name="scriptName">Script file name</param>
    /// <param name="path">Path to script (pass to override)</param>
    /// <returns>Were successful in loading the script</returns>
    public bool Load(string scriptName, string? path = null) {
        path ??= scriptsPath;
        return true;
    }

    /// <summary>
    /// Run loaded script or load and then run
    /// </summary>
    /// <param name="scriptName">Script file name</param>
    /// <param name="path">Path to script (pass to override)</param>
    /// <param name="args">Context to pass to script</param>
    /// <returns>Script execution result</returns>
    public object? DoScript(string scriptName, string? path = null, params object[] args) {
        Load(scriptName, path);
        return null;
    }
}
