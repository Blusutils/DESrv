using System.Text;
using System.Net.Sockets;
namespace DESCore {
    /// <summary>
    /// DESrv main runner
    /// </summary>
    sealed class DESCoreRunner {
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
        public DESPDKUtils.PDKLoader pdkLoader;
        /// <summary>
        /// List of extensions IDs to load
        /// </summary>
        private string[] extsToLoad;
        /// <summary>
        /// Configuration
        /// </summary>
        private Dictionary<string, string> config;
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
            if (!File.Exists(Path.Combine(".", "des-run.dll"))) {
                Console.BackgroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine($"ABORTING DESrv RUN: CURRENT WORKDIR ISN'T DEFAULT.\nYou started DESrv from non-default directory (i.e. a folder that does not contain the files needed to start DESrv). Please restart server from a default directory.");
                Console.ResetColor(); 
                Environment.Exit(1);
            }
        }
        /// <summary>
        /// Setup configuration of runner
        /// </summary>
        /// <param name="args">Commandline args</param>
        /// <param name="config">Configuration (dictioanry)</param>
        public void SetupRuntime(string[] args, Dictionary<string, string> config) {
            pdkLoader = new DESPDKUtils.PDKLoader(Path.Combine(".", "extensions"));
            pdkLoader.AddAllExtensionsFromDir("./extensions");
            var parsed = Utils.ArgParser.Parse(args);
            foreach (var key in parsed.Keys) {
                config[key] = parsed[key];
            }
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
                    if (extsToLoad.Contains(ext.ID) || extsToLoad.Length == 0) {
                        pdkLoader.LoadExtension(ext);
                        new DESCEnd.CEnd(CEndLog, new DESCEnd.Exceptor()).Run(() => ext.Entrypoint());
                    }
                } catch (Exception ex) { 
                    CEndLog.Error($"Error in extension (from {ex.Source}). Exception: {ex.Message}\nStack trace: \t{ex.StackTrace}"); 
                }
            }

            // Code below added just for test; later you will able to use any count of connection using extensions
            string servermode;
            servermode = config.TryGetValue("servermode", out servermode) ? servermode : "notconfigured";
            string ipadress;
            ipadress = config.TryGetValue("ipadress", out ipadress) ? ipadress : "127.0.0.1";
            string portS; int port;
            port = config.TryGetValue("ipadress", out portS) ? int.Parse(portS) : 9090;
            CEndLog.Notice($"Server will start on {ipadress}:{port}");
            CEndLog.Fatal(@"DESrv does not support work in direct socket mode. Use ""DirectSock"" extension instead.");
            //switch (servermode.ToLower().Trim(' '))
            //{
            //    case "websocket":
            //        CEndLog.Notice($"Trying to run using Websocket");
            //        var websockproc = new DESConnections.DESWebSocketsProcessor(config);
            //        CEnd.Run(websockproc.Runner);
            //        break;
            //    case "tcpsock":
            //        CEndLog.Notice($"Trying to run using TCP socket");
            //        var tcpsockproc = new DESConnections.DESTCPProcessor(config);
            //        DESConnections.DESTCPReciveEvent.Instance.Callbacks += (object[] args) => {
            //            var client = (TcpClient)args[0];
            //            CEndLog.Debug(Encoding.UTF8.GetString((byte[])args[1]));
            //            client.Close();
            //            return true;
            //        };
            //        CEnd.Run(tcpsockproc.Runner);
            //        break;
            //    case "udpsock":
            //        CEndLog.Notice($"Trying to run using UDP socket");
            //        throw new NotImplementedException("UDP is not implemented for now!");
            //    case "notconfigured":
            //        throw new NotImplementedException("Please provide a valid type of connection");
            //    default:
            //        throw new NotImplementedException("Invalid server mode. Please provide a valid type of connection");
            //}
        }
    }
}
