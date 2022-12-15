using System.Reflection;
using DESCEnd;
using DESCEnd.L10n;
using DESCEnd.Logging;
using DESrv.PDK;
using DESrv.PDK.Random;

namespace DESrv {
    /// <summary>
    /// DESrv main runner
    /// </summary>
    sealed class DESCoreRunner {
        /// <summary>
        /// Version of this DESrv build
        /// </summary>
        public static Version? DESVersion = null;
        /// <summary>
        /// DESCEnd
        /// </summary>
        public static CEnd? CEnd;
        /// <summary>
        /// Localization
        /// </summary>
        public static Localizer? Localizer;
        /// <summary>
        /// PDK Loader
        /// </summary>
        public PDKLoader? pdkLoader;
        /// <summary>
        /// Configuration
        /// </summary>
        private Config.OurConfig? config;

        static DESCoreRunner() {
            DESVersion ??= Assembly.GetExecutingAssembly().GetName().Version ?? new Version(1, 0, 0, 0);
        }
        public static CEndLog GetLogger() {
            return CEnd.Logger;
        }
        /// <summary>
        /// Setup configuration of runner
        /// </summary>
        /// <param name="args">Commandline args</param>
        /// <param name="config">Configuration (dictioanry)</param>
        public void SetupRuntime(string[] args, Config.OurConfig config) {
            var currentAsm = Assembly.GetExecutingAssembly();
            var despath = currentAsm.Location.Replace(Path.GetFileName(currentAsm.Location), "");

            GetLogger().Debug(
                "Trying to load translations"
                );
            var translationsPath = Path.Combine(despath, PDKConstants.RELATIVE_TRANSLATIONS_PATH);
            if (!Directory.Exists(translationsPath)) {
                GetLogger().Debug(
                "Translations folder not found, creating..."
                );
                Directory.CreateDirectory(translationsPath);
            }
            Localizer = new Localizer();
            Localizer.Load(translationsPath);
            Localizer.Strict = false;
            GetLogger().Success(
                Localizer.Translate(
                            "desrv.core.loadedlangs",
                            "Loaded languages: {0}",
                            Directory.GetFiles(translationsPath).Length
                        )
                );

            var scriptsPath = Path.Combine(despath, PDKConstants.RELATIVE_SCRIPTS_PATH);
            if (!Directory.Exists(scriptsPath)) {
                GetLogger().Debug(
                "Scripts folder not found, creating..."
                );
                Directory.CreateDirectory(scriptsPath);
            }

            var dependenciesPath = Path.Combine(despath, PDKConstants.RELATIVE_NEUTRAL_DEPENDENCIES_PATH);
            if (!Directory.Exists(dependenciesPath)) {
                GetLogger().Debug(
                "Neutral dependencies folder not found, creating..."
                );
                Directory.CreateDirectory(dependenciesPath);
            }

            RandomBase.Randoms.Add(new DotnetRandom());

            pdkLoader = new PDKLoader(Path.Combine(despath, PDKConstants.RELATIVE_EXTENSIONS_PATH));
            pdkLoader.AddAllExtensionsFromDir();

            var parsed = Utils.ArgParser.Parse(args);
            /*foreach (var key in parsed.Keys) {
                config[key] = parsed[key];
            }*/
            config.Extend(parsed);
            this.config = config;

            RandomBase.Randoms.ForEach(x => {
                if (config.randomMode == x.ExtID) x.IsPreffered = true; else x.IsPreffered = false;
            });
        }
        /// <summary>
        /// Setup <see cref="DESCEnd.CEnd"/>
        /// </summary>
        /// <param name="cend">CEnd object</param>
        public static void SetupCEnd(CEnd cend) {
            CEnd.Logger.LogSource = "DESrv Runner";
            CEnd = cend;
        }
        /// <summary>
        /// Run the server
        /// </summary>
        /// <exception cref="NotImplementedException">If connection type is not implemented yet</exception>
        public void Go() {
            var countOfExts = pdkLoader.GetAvailableExtensions().ToArray().Length;
            var extsRepr = string.Join(", ", pdkLoader.GetAvailableExtensions());
            var msg = Localizer.Translate("desrv.pdk.loadedexts", "Added {0} extensions: {1}",
                countOfExts, extsRepr);
            CEnd.Logger.Debug(msg);
            foreach (var ext in pdkLoader.GetAvailableExtensions()) {
                try {
                    var metadata = ext.GetPropertyValue("Metadata") as ExtensionMetadata;
                    var extSuppDes = new Version(metadata.DESVersion);
                    if (extSuppDes.Major != DESVersion.Major && extSuppDes.Minor != DESVersion.Minor) {
                        CEnd.Logger.Error(
                            Localizer.Translate("desrv.pdk.errors.notcompatable",
                            "Error in extension {0}: major and minor versions is not same (current DESrv version is {1}; however, this extension supports only {2})",
                            ext, DESVersion, metadata.DESVersion
                         ));
                        return;
                    }
                    if (config.extensionsToLoad.Contains(metadata.ID) || config.extensionsToLoad.ToArray().Length == 0) {
                        pdkLoader.LoadExtension(ext);
                        new CEnd().Run(() => ext.Entrypoint());
                    }
                } catch (Exception ex) {
                    CEnd.Logger.Error(
                        Localizer.Translate(
                            "desrv.pdk.errors.exterror",
                            "Error {0} in {1} (from method {2}, caused by {3}). Exception: {4}\nStack trace: \n{5}",
                            ext.ToString(), ext, ex.TargetSite!, ex.Source!, ex.Message!, ex.StackTrace!
                        ));
                }
            }

            var ipadress = config.ipAdress ?? "127.0.0.1";
            //string portS; int port;
            //port = config.TryGetValue("port", out portS) ? int.Parse(portS) : 9090;
            CEnd.Logger.Notice(Localizer.Translate("desrv.core.startnotice", "Server will start on {0}", ipadress));
            if (pdkLoader.GetAvailableExtensions().ToArray().Length == 0) {
                CEnd.Logger.Fatal(Localizer.Translate("desrv.core.directsock", @"DESrv does not support work in direct socket mode. Use ""DirectSock"" extension instead."));
                Console.WriteLine(Localizer.Translate("desrv.main.closeconsole", "Press any key to close console..."));
                Console.ReadKey();
                Environment.Exit(-1);
            }
            Thread.Sleep(-1);
        }
    }
}
