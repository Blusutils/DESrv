using System.Text;
using System.Net.Sockets;
using System.Reflection;

namespace DESCore
{
    /// <summary>
    /// DESrv main runner
    /// </summary>
    sealed class DESCoreRunner {
        /// <summary>
        /// Version of this DESrv build
        /// </summary>
        public Version DESVersion;
        /// <summary>
        /// <see cref="DESCEnd.Logging.CEndLog"/> logger
        /// </summary>
        public static DESCEnd.Logging.CEndLog CEndLog;
        /// <summary>
        /// DESCEnd
        /// </summary>
        public DESCEnd.CEnd CEnd;
        /// <summary>
        /// PDK Loader
        /// </summary>
        public PDKLoader pdkLoader;
        /// <summary>
        /// List of extensions IDs to load
        /// </summary>
        private List<string> extsToLoad = new();
        /// <summary>
        /// Configuration
        /// </summary>
        private ConfigurationModel config;
        /// <summary>
        /// Get logger (idk why)
        /// </summary>
        /// <returns><see cref="DESCEnd.Logging.CEndLog"/></returns>
        public DESCEnd.Logging.CEndLog GetLogger() {
            return CEndLog;
        }
        /// <summary>
        /// Create new instance of <see cref="DESCoreRunner"/>
        /// </summary>
        public DESCoreRunner () {
            Assembly currentAsm = Assembly.GetExecutingAssembly();
            DESVersion = currentAsm.GetName().Version??new Version(0, 0, 0, 0);
            /*if (!File.Exists(Path.Combine(".", "des-run.dll"))) {
                Console.BackgroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine($"ABORTING DESrv RUN: des-run.dll NOT FOUND.\nUnable to find des-run.dll (main DESrv file). It may be happend either if you started DESrv from non-default directory or perform invalid installation. Please restart server directly from a default directory or reinstall them.");
                Console.ResetColor(); 
                Environment.Exit(1);
            }*/
        }
        /// <summary>
        /// Setup configuration of runner
        /// </summary>
        /// <param name="args">Commandline args</param>
        /// <param name="config">Configuration (dictioanry)</param>
        public void SetupRuntime(string[] args, ConfigurationModel config) {
            var currentAsm = Assembly.GetExecutingAssembly();
            var despath = currentAsm.Location.Substring(0, currentAsm.Location.Length - "des-run.dll".Length);
            pdkLoader = new PDKLoader(Path.Combine(despath, "extensions"));
            pdkLoader.AddAllExtensionsFromDir();
            var parsed = Utils.ArgParser.Parse(args);
            /*foreach (var key in parsed.Keys) {
                config[key] = parsed[key];
            }*/
            ConfigurationModel.Instance.Extend(parsed);
            this.config = config;
        }
        /// <summary>
        /// Setup <see cref="DESCEnd.CEnd"/>
        /// </summary>
        /// <param name="cend">CEnd object</param>
        public void SetupCEnd(DESCEnd.CEnd cend) {
            CEndLog = cend.logger;
            CEndLog.LogSource = "DESrv Runner";
            CEndLog.Success("CEnd Setup");
            CEnd = cend;
        }
        /// <summary>
        /// Run the server
        /// </summary>
        /// <exception cref="NotImplementedException">If connection type is not implemented yet</exception>
        public void Go() {
            CEndLog.Debug($"Added {pdkLoader.GetAvailableExtensions().ToArray().Length} extensions: {string.Join(", ", pdkLoader.GetAvailableExtensions())}");
            foreach (var ext in pdkLoader.GetAvailableExtensions()) {
                try {
                    if (new Version(ext.GetFieldValue("DESVersion").ToString()) != DESVersion) {
                        CEndLog.Error($"Error in extension {ext}: versions is not same (current DESrv version is {DESVersion}; however, this extension supports only {ext.GetFieldValue("DESVersion")} DESrv)");
                        continue;
                    }
                    if (extsToLoad.Contains(ext.GetFieldValue("ID")) || extsToLoad.ToArray().Length == 0) {
                        pdkLoader.LoadExtension(ext);
                        new DESCEnd.CEnd(CEndLog, new DESCEnd.Exceptor()).Run(() => ext.Entrypoint());
                    }
                    /*} catch (System.ArgumentNullException) {
                        CEndLog.Error($"Extension {ext} is null");*/
                } catch (Exception ex) {
                    CEndLog.Error($"Error {ex.GetType()} in extension {ext} (from method {ex.TargetSite}, caused by {ex.Source}). Exception: {ex.Message}\nStack trace: \n{ex.StackTrace}");
                }
            }

            var ipadress = config.IpAdress??"127.0.0.1";
            //string portS; int port;
            //port = config.TryGetValue("port", out portS) ? int.Parse(portS) : 9090;
            CEndLog.Notice($"Server will start on {ipadress}");
            CEndLog.Fatal(@"DESrv does not support work in direct socket mode. Use ""DirectSock"" extension instead.");
        }
    }
}
