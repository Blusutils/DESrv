namespace Blusutils.DESrv.LuaScriptingApi;
public class LuaLoader {

    string scriptsPath;

    public LuaLoader(string luaScriptsPath) {
        scriptsPath = luaScriptsPath;
    }

    public bool Load(string scriptName, string? path = null) {
        path ??= scriptsPath;
        return true;
    }

    public object? DoScript(string scriptName, string? path = null, params object[] args) {
        Load(scriptName, path);
        return null;
    }
}
