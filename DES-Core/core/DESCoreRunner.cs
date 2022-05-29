using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DESCore
{
    class DESCoreRunner
    {
        public static DESCEnd.Logging.CEndLog CEndLog;
        public DESCEnd.CEnd cEnd;
        private Dictionary<string, string> config;
        public DESCEnd.Logging.CEndLog GetLogger()
        {
            return CEndLog;
        }
        public DESCoreRunner ()
        {

        }
        public void Setup(string[] args, Dictionary<string, string> config)
        {
            var parsed = Utils.ArgParser.Parse(args);
            foreach (var key in parsed.Keys)
            {
                config[key] = parsed[key];
            }
            this.config = config;
        }
        public void SetupCEnd(DESCEnd.CEnd cend)
        {
            CEndLog = cend.logger;
            CEndLog.LogSource = "DES Runner";
            CEndLog.Success("CEnd Setup");
            cEnd = cend;
        }
        public void Go()
        {
            string servermode;
            servermode = config.TryGetValue("ipaddress", out servermode) ? servermode : "127.0.0.1";
            switch (servermode)
            {
                case "websocket":
                    var websockproc = new DESConnections.DESWebSocketsProcessor(config);
                    cEnd.Run(websockproc.Runner);
                    break;
                case "tcpsock":
                    var tcpsockproc = new DESConnections.DESTCPProcessor(config);
                    cEnd.Run(tcpsockproc.Runner);
                    break;
                case "udpsock":
                    throw new NotImplementedException("UDP is not implemented for now!");
            }
        }
    }
}
