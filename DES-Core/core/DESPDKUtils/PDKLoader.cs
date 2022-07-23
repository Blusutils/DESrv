using System;
using System.Reflection;
using DESPDK;
#pragma warning disable CS8618
namespace DESPDKUtils {
    /// <summary>
    /// Extension loader class
    /// </summary>
    public class PDKLoader {
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
        /// Add an extension by its filename
        /// </summary>
        /// <param name="extname">Extension filename</param>
        public void AddExtension (string extname) {
            Assembly pdkobj = Assembly.LoadFrom(extname);
            Console.WriteLine(pdkobj.ManifestModule.Name);
            var a = pdkobj.CreateInstance($"{pdkobj.ManifestModule.Name.Replace(".dll", "").Replace(".desext", "")}.Extension") as PDKAbstractExtension;
            Console.WriteLine("1");
            //a.ID = "1";
            Console.WriteLine(a.ID == "Test" ? a.ID : "pizda");
            Console.WriteLine("2");
            pdkobjects.Add(a);
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
                    Console.WriteLine(file);
                    AddExtension(file);
                }
            }
        }
        /// <summary>
        /// Load an extension and execute it
        /// </summary>
        /// <param name="extension">Extension to load</param>
        public void LoadExtension(PDKAbstractExtension extension) {
            if (extension.ExtType == 1) extension.Entrypoint(); // plugin
            else if (extension.ExtType == 2) { // addon
                Console.WriteLine("plugin");
                foreach (var ext in pdkobjects) {
                    if (ext.ExtType == 1 && ext.ID == extension.Reference)
                        ext.LoadSubExtension(extension);
                    }
            } else { Console.WriteLine("wtf", extension.GetID(), extension.ExtType.GetType().Name); };
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
