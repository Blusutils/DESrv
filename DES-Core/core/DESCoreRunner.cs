﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DESCore {
    class DESCoreRunner {
        public static DESCEnd.Logging.CEndLog CEndLog;
        public DESCEnd.CEnd cEnd;
        private Dictionary<string, string> config;
        public DESCEnd.Logging.CEndLog GetLogger() {
            return CEndLog;
        }
        public DESCoreRunner () {

        }
        public void Setup(string[] args, Dictionary<string, string> config) {
            var parsed = Utils.ArgParser.Parse(args);
            foreach (var key in parsed.Keys) {
                config[key] = parsed[key];
            }
            this.config = config;
        }
        public void SetupCEnd(DESCEnd.CEnd cend) {
            CEndLog = cend.logger;
            CEndLog.LogSource = "DESrv Runner";
            CEndLog.Success("CEnd Setup");
            cEnd = cend;
        }
        public void Go() {
            DESConnections.DESTCPReciveEvent.CreateInstance();
            string servermode;
            servermode = config.TryGetValue("servermode", out servermode) ? servermode : "notconfigured";
            switch (servermode.ToLower().Trim(' '))
            {
                case "websocket":
                    CEndLog.Notice($"Trying to run using Websocket");
                    var websockproc = new DESConnections.DESWebSocketsProcessor(config);
                    cEnd.Run(websockproc.Runner);
                    break;
                case "tcpsock":
                    CEndLog.Notice($"Trying to run using TCP socket");
                    var tcpsockproc = new DESConnections.DESTCPProcessor(config);
                    DESConnections.DESTCPReciveEvent.Instance.Callbacks += (client, data) => {
                        CEndLog.Debug(Encoding.UTF8.GetString(data));
                        return true;
                    };
                    cEnd.Run(tcpsockproc.Runner);
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
