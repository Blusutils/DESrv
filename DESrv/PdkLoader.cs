using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blusutils.DESrv.PDK;

namespace Blusutils.DESrv {
    public class PdkLoader {
        public string? LoadFrom { get; init; }
        public Dictionary<ExtensionMetadata, ExtensionStatus>? Extensions { get; } = new();
        public void AddExtension(string pathToExtension) { }
        public void AddExtensionsFromDirectory() { }
        public void AddExtensionsFromDirectory(string pathToExtension) { }
        public void LoadExtension(string id) { }
        public void UnloadExtension(string id) { }
        public void SuspendExtension(string id) { }
    }
}
