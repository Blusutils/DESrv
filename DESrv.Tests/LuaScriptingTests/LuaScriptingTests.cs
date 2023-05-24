using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blusutils.DESrv.LuaScriptingApi;
using NLua;
using NLua.Exceptions;

namespace Blusutils.DESrv.Tests.LuaScriptingTests;
public class LuaScriptingTests : Tests {
    
    [OneTimeSetUp]
    public void Setup() {
        if (!Directory.Exists("scripts"))
            Directory.CreateDirectory("scripts");
        // empty script
        File.WriteAllText(Path.Combine("scripts", "empty_script.lua"), "");
        // not full script
        File.WriteAllText(Path.Combine("scripts", "only_meta.lua"), "meta = {}");
        // unfilled meta
        File.WriteAllText(Path.Combine("scripts", "unfilled.lua"), "meta = {} func = {}");
        // partial meta
        File.WriteAllText(Path.Combine("scripts", "partial_meta.lua"), "meta = {name = 'test', extension = 'DESrvInternal'} func = {}");
        // with function
        File.WriteAllText(Path.Combine("scripts", "with_func.lua"), "meta = {name = 'test', extension = 'DESrvInternal'} func = {} function func.abc() return 1 end");
    }

    [Test(Description = "Load empty script")]
    public void EmptyScript() {
        Assert.Throws<LuaException>(() => LuaLoader.Load("empty_script"));
    }

    [Test(Description = "Load not full script")]
    public void NotFullScript() {
        Assert.Throws<LuaException>(() => LuaLoader.Load("only_meta"));
    }

    [Test(Description = "Load script with unfilled meta")]
    public void UnfilledScript() {
        Assert.Multiple(() => {
            Assert.That(LuaLoader.Load("unfilled").Name, Is.EqualTo("unfilled"));
            Assert.That(LuaLoader.Load("unfilled").ExtensionID, Is.EqualTo("*"));
            Assert.That(LuaLoader.Load("unfilled").Author, Is.EqualTo("unknown author"));
        });
    }

    [Test(Description = "Load script with partially filled meta")]
    public void ScriptWithPartialMeta() {
        Assert.Multiple(() => {
            Assert.That(LuaLoader.Load("partial_meta").Name, Is.EqualTo("test"));
            Assert.That(LuaLoader.Load("partial_meta").ExtensionID, Is.EqualTo("DESrvInternal"));
            Assert.That(LuaLoader.Load("partial_meta").Author, Is.EqualTo("unknown author"));
        });
    }

    [Test(Description = "Load script with function and run it")]
    public void ScriptWithFunctions() {
        Assert.That((long)(LuaLoader.Load("with_func").ExecutableSpaceTable["abc"] as LuaFunction)?.Call()?.FirstOrDefault(0), Is.EqualTo(1));
    }
}
