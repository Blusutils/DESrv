using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DESCore.DESPDK {
    public abstract class PDKAbstractExtension {
        /// <summary>
        /// ID of extension. It mustn't contain spaces and special symbols (for example dots)
        /// </summary>
        public string ID;
        /// <summary>
        /// Type of extension: 1 - plugin, 2 - addon
        /// </summary>
        public int ExtType;
        /// <summary>
        /// Readable name of extension
        /// </summary>
        public string Name;
        /// <summary>
        /// Description for extension
        /// </summary>
        public string Description = "";
        /// <summary>
        /// Version for extension
        /// </summary>
        public string Version;
        /// <summary>
        /// Version of DESrv what this extension supports
        /// </summary>
        public string DESVersion;
        /// <summary>
        /// Author of extension
        /// </summary>
        public string Author;
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
    }
}
