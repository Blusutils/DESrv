using System.Reflection;
using NLua;

namespace DESrv.PDK {
    public static class DoExtScriptExtension {
        public static object[] DoExtensionScript(this Lua lua, string filename) =>
            lua.DoFile(Path.Combine(
                Assembly.GetExecutingAssembly().Location.Replace(Path.GetFileName(Assembly.GetExecutingAssembly().Location), ""),
                PDKConstants.RELATIVE_SCRIPTS_PATH,
                filename));
    }
}
