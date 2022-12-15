using System.Runtime.InteropServices;
using System.Text;
using NLua;

namespace DESrv.PDK {
    public class SubExtensionLoadsNotImplementedException : Exception {
        public AbstractPDKExtension ext;
        public SubExtensionLoadsNotImplementedException(AbstractPDKExtension ext) : base("Sub extension loading is not supported by {0}") {
            this.ext = ext;
        }
    }
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PDKExtensionAttribute : Attribute {
        public PDKExtensionAttribute() { }
    }
    public class ExtensionMetadata {
        /// <summary>
        /// ID of extension. It mustn't contain spaces and special symbols (for example dots)
        /// </summary>
        public string ID { get; set; } = "";
        /// <summary>
        /// Type of extension: 1 - plugin, 2 - addon
        /// </summary>
        public int ExtType { get; set; } = 0;
        /// <summary>
        /// Readable name of extension
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// Description for extension
        /// </summary>
        public string Description { get; set; } = "";
        /// <summary>
        /// Version of extension
        /// </summary>
        public string Version { get; set; } = "-0.0.0";
        /// <summary>
        /// Version of DESrv what this extension supports
        /// </summary>
        public string DESVersion { get; set; } = "-0.0.0";
        /// <summary>
        /// Author of extension
        /// </summary>
        public string Author { get; set; } = "";
        /// <summary>
        /// ID of extension to which this extension refers (for addons)
        /// </summary>
        public string Reference { get; set; } = "";
    }
    /// <summary>
    /// An abstract class that provides an interface to implement the information and functionality of an extension
    /// </summary>
    [ComVisible(true)]
    public abstract class AbstractPDKExtension : AbstractFPReaderInClass {
        /// <summary>
        /// Metadata of extension
        /// </summary>
        public abstract ExtensionMetadata Metadata { get; set; }
        protected virtual Lua? LuaState { get; set; }
        /// <summary>
        /// Main method
        /// </summary>
        public abstract void Entrypoint();
        /// <summary>
        /// Load addon to this extension (for plugins)
        /// </summary>
        /// <param name="extension">Extension needed to load</param>
        public virtual void LoadSubExtension(AbstractPDKExtension extension) { throw new SubExtensionLoadsNotImplementedException(this); }
        /// <summary>
        /// Event what calls when extension loads
        /// </summary>
        public abstract void Load();
        /// <summary>
        /// Event what calls when extension unloads
        /// </summary>
        public abstract void Unload();

        public virtual void ProceedLuaScript(Lua? luaState = null, string? script = null) {
            if (LuaState != luaState) LuaState ??= luaState;
            LuaState.State.Encoding = Encoding.UTF8;
            if (script is not null) LuaState.DoExtensionScript(script);
        }

        public sealed override string ToString() {
            var metadata = GetPropertyValue("Metadata") as ExtensionMetadata;
            var whatisthis = metadata.ExtType switch {
                1 => "plugin",
                2 => "addon",
                3 => "random",
                _ => "unknown",
            };
            return $"Extension {{type={whatisthis} id={metadata.ID} version={metadata.Version}}}";
        }

        public sealed override object? GetFieldValue(string name) => GetType().GetField(name).GetValue(this);
        public sealed override object? GetPropertyValue(string name) => GetType().GetProperty(name).GetValue(this);
    }
}