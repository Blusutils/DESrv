using System;
namespace DESPDK {
    public abstract class PDKAbstractExtension : AssemblyFieldReader {
        /// <summary>
        /// ID of extension. It mustn't contain spaces and special symbols (for example dots)
        /// </summary>
        public string ID = "";
        /// <summary>
        /// Type of extension: 1 - plugin, 2 - addon
        /// </summary>
        public int ExtType = 0;
        /// <summary>
        /// Readable name of extension
        /// </summary>
        public string Name = "";
        /// <summary>
        /// Description for extension
        /// </summary>
        public string Description = "";
        /// <summary>
        /// Version for extension
        /// </summary>
        public string Version = "-0.0.0";
        /// <summary>
        /// Version of DESrv what this extension supports
        /// </summary>
        public string DESVersion = "-0.0.0";
        /// <summary>
        /// Author of extension
        /// </summary>
        public string Author = "";
        /// <summary>
        /// Array of dependencies for extension
        /// </summary>
        public string[] Dependencies = Array.Empty<string>();
        /// <summary>
        /// ID of extension to which this extension refers (for addons)
        /// </summary>
        public string Reference = "";
        /// <summary>
        /// Main method
        /// </summary>
        public abstract void Entrypoint();
        /// <summary>
        /// Load addon to this extension (for plugins)
        /// </summary>
        /// <param name="extension">Extension needed to load</param>
        public abstract void LoadSubExtension(PDKAbstractExtension extension);
        /// <summary>
        /// Event what calls when extension loads
        /// </summary>
        public abstract void OnLoad();
        /// <summary>
        /// Event what calls when extension unloads
        /// </summary>
        public abstract void OnUnload();

        public sealed override string ToString() {
            var extype = (int)GetFieldValue("ExtType");
            var whatisthis = extype == 1 || extype == 2 ? (extype==1?"plugin":"addon") : "unknown";
            return $"EXT_{whatisthis}_{GetFieldValue("ID") as string}_v{GetFieldValue("Version")}";
        }

        public sealed override object GetFieldValue(string name) {
            return GetType().GetField(name).GetValue(this);
        }
    }
}