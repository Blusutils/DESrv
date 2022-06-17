using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DESCore {
    /// <summary>
    /// DESrv main runner
    /// </summary>
    class DESCoreRunner {
        /// <summary>
        /// <see cref="DESCEnd.Logging.CEndLog"/> logger
        /// </summary>
        public static DESCEnd.Logging.CEndLog CEndLog;
        /// <summary>
        /// DESCEnd
        /// </summary>
        public DESCEnd.CEnd CEnd;
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

        }
        /// <summary>
        /// Setup configuration of runner
        /// </summary>
        /// <param name="args">Commandline args</param>
        /// <param name="config">Configuration (dictioanry)</param>
        public void Setup(string[] args, Dictionary<string, string> config) {
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
            DESConnections.DESTCPReciveEvent.CreateInstance();
            string servermode;
            servermode = config.TryGetValue("servermode", out servermode) ? servermode : "notconfigured";
            switch (servermode.ToLower().Trim(' '))
            {
                case "websocket":
                    CEndLog.Notice($"Trying to run using Websocket");
                    var websockproc = new DESConnections.DESWebSocketsProcessor(config);
                    CEnd.Run(websockproc.Runner);
                    break;
                case "tcpsock":
                    CEndLog.Notice($"Trying to run using TCP socket");
                    var tcpsockproc = new DESConnections.DESTCPProcessor(config);
                    DESConnections.DESTCPReciveEvent.Instance.Callbacks += (client, data) => {
                        CEndLog.Debug(Encoding.UTF8.GetString(data));
                        return true;
                    };
                    CEnd.Run(tcpsockproc.Runner);
                    break;
                case "udpsock":
                    CEndLog.Notice($"Trying to run using UDP socket");
                    throw new NotImplementedException("UDP is not implemented for now!");
                default:
                    throw new NotImplementedException("HEY BRO WHAT THE FUCK?! Invalid server mode!");
            }
        }
    }
}
