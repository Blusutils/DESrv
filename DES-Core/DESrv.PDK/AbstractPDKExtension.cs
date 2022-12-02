using System.Runtime.InteropServices;

namespace DESrv.PDK {
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
        /// Array of dependencies for extension
        /// </summary>
        public string[] Dependencies { get; set; } = Array.Empty<string>();
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
        /// <summary>
        /// Main method
        /// </summary>
        public virtual void Entrypoint() { }
        /// <summary>
        /// Load addon to this extension (for plugins)
        /// </summary>
        /// <param name="extension">Extension needed to load</param>
        public virtual void LoadSubExtension(AbstractPDKExtension extension) { extension.Load(); }
        /// <summary>
        /// Event what calls when extension loads
        /// </summary>
        public virtual void Load() { }
        /// <summary>
        /// Event what calls when extension unloads
        /// </summary>
        public virtual void Unload() { }

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

        public sealed override object GetFieldValue(string name) {
            return GetType().GetField(name).GetValue(this);
        }
        public sealed override object GetPropertyValue(string name) {
            return GetType().GetProperty(name).GetValue(this);
        }
    }
}