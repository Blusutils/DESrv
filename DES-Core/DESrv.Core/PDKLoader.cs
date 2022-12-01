using System.IO;
using System.Reflection;
using DESrv.PDK;
using DESrv.PDK.Random;
#pragma warning disable CS8618
namespace DESrv {
    /// <summary>
    /// Extension loader class
    /// </summary>
    public sealed class PDKLoader {
        /// <summary>
        /// List of extensions
        /// </summary>
        List<AbstractPDKExtension> pdkobjects = new List<AbstractPDKExtension>();
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
                        var a = Activator.CreateInstance(typ) as AbstractPDKExtension;
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
        public void LoadExtension(AbstractPDKExtension extension) {
            var metadata = extension.GetPropertyValue("Metadata") as ExtensionMetadata;
            switch (metadata.ExtType) {
                case 1:  // plugin
                    extension.Load();
                    break;
                case 2: // addon
                    foreach (var ext in pdkobjects) {
                        var md = ext.GetPropertyValue("Metadata") as ExtensionMetadata;
                        if (md.ExtType == 1 && md.ID == metadata.Reference) {
                            ext.LoadSubExtension(extension);
                            break;
                        }
                    }
                    break;
                case 3: // random generator
                    RandomBase.Randoms.Add((RandomBase)extension.GetFieldValue("randomNumberGenerator"));
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
        public List<AbstractPDKExtension> GetAvailableExtensions() {
            return pdkobjects;
        }
    }
}
