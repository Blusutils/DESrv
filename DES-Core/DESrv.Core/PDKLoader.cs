using System.IO;
using System.Reflection;
using DESrv.PDK;
#pragma warning disable CS8618
namespace DESrv {
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
        /// Add an extension by its filename
        /// </summary>
        /// <param name="extToLoad">Extension filename</param>
        public void AddExtension(string extToLoad) {
            Assembly pdkobj = Assembly.LoadFrom(extToLoad);

            string extname = pdkobj.ManifestModule.Name;

            foreach (var typ in pdkobj.GetTypes()) {
                if (typ.IsClass && typ.GetCustomAttribute<PDKExtensionAttribute>() != null) {
                    try {
                        var a = Activator.CreateInstance(typ) as PDKAbstractExtension;
                        if (a == null) {
                            DESCoreRunner.GetLogger().Error(
                                DESCoreRunner.Localizer.Translate(
                                    "desrv.pdk.errors.invalidext.noentrypoint1",
                                    "Extension {0} is invalid (unable to find entrypoint class)",
                                    extname
                                )
                            );
                            return;
                        }
                        pdkobjects.Add(a);
                    } catch (InvalidCastException) {
                        DESCoreRunner.GetLogger().Error(
                            DESCoreRunner.Localizer.Translate(
                                "desrv.pdk.errors.invalidext.noentrypoint2",
                                "Extension {0} is invalid (entrypoint class \"Extension\" not derived from PDKAbstractExtension)",
                                pdkobj.ManifestModule.Name
                            )
                        );
                    }
                }
            }
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
                if (File.Exists(file) && file.EndsWith(".dll")) {
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
                    extension.Load();
                    break;
                case 2: // addon
                    foreach (var ext in pdkobjects) {
                        if (ext.ExtType == 1 && ext.ID == extension.Reference)
                            ext.LoadSubExtension(extension);
                    }
                    break;
                default:
                    DESCoreRunner.GetLogger().Error(
                        DESCoreRunner.Localizer.Translate(
                            "desrv.pdk.errors.invalidext.unknowntype",
                            "Found an invalid extension with unknown type {0}: {1}",
                            extension.GetFieldValue("ExtType"), extension
                        )
                    ); break;
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
