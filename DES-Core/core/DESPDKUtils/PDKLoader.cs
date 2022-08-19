using System;
using System.Reflection;
using DESPDK;
#pragma warning disable CS8618
namespace DESCore.DESPDKUtils {
    /// <summary>
    /// Extension loader class
    /// </summary>
    public sealed class PDKLoader {
        /// <summary>
        /// List of extensions
        /// </summary>
        List<PDKAbstractExtension> pdkobjects = new List<PDKAbstractExtension>();
        /// <summary>
        /// Current extensions directory
        /// </summary>
        string curdir;
        /// <summary>
        /// Create new instance of <see cref="PDKLoader"/>
        /// </summary>
        /// <param name="workDir">Directory to load extensons from it</param>
        public PDKLoader(string workDir) {
            if (!Directory.Exists(workDir)) Directory.CreateDirectory(workDir);
            curdir = workDir;
        }
        /// <summary>
        /// Writes "micro error log" to console
        /// </summary>
        /// <param name="message">Message to write</param>
        private static void Microlog(string message) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        /// <summary>
        /// Add an extension by its filename
        /// </summary>
        /// <param name="extname">Extension filename</param>
        public void AddExtension (string extname) {
            Assembly pdkobj = Assembly.LoadFrom(extname);
            try {
                var a = pdkobj.CreateInstance($"{pdkobj.ManifestModule.Name.Replace(".dll", "").Replace(".desext", "")}.Extension") as PDKAbstractExtension;
                if (a == null) { Microlog($"Extension {pdkobj.ManifestModule.Name} is invalid (entrypoint class \"Extension\" not found)"); return; }
                pdkobjects.Add(a);
            } catch (InvalidCastException) { Microlog($"Extension {pdkobj.ManifestModule.Name} is invalid (entrypoint class \"Extension\" not derived from PDKAbstractExtension)"); }
        }
        /// <summary>
        /// Read default directory and load all extensions from it
        /// </summary>
        public void AddAllExtensionsFromDir() {
            AddAllExtensionsFromDir(curdir);
        }
        /// <summary>
        /// Read directory and load all extensions from it
        /// </summary>
        /// <param name="targetDir">Target directory</param>
        public void AddAllExtensionsFromDir(string targetDir) {
            foreach (var file in Directory.GetFiles(targetDir)) {
                if (File.Exists(file) && file.EndsWith(".desext.dll")) {
                    AddExtension(file);
                }
            }
        }
        /// <summary>
        /// Load an extension and execute it
        /// </summary>
        /// <param name="extension">Extension to load</param>
        public void LoadExtension(PDKAbstractExtension extension) {
            var exttype = (int)extension.GetFieldValue("ExtType");
            switch (exttype) {
                case 1:  // plugin
                    extension.OnLoad();
                    break;
                case 2: // addon
                        foreach (var ext in pdkobjects) {
                            if (ext.ExtType == 1 && ext.ID == extension.Reference)
                                ext.LoadSubExtension(extension);
                        }
                    break;
                default: Microlog($"Found an invalid extension with unknown type {extension.GetFieldValue("ExtType")}: {extension}"); break;
            }
        }
        /// <summary>
        /// Get list of available (added) extensions
        /// </summary>
        /// <returns><see cref="List{PDKAbstractExtension}"/> of extensions</returns>
        public List<PDKAbstractExtension> GetAvailableExtensions() {
            return pdkobjects;
        }
    }
}
