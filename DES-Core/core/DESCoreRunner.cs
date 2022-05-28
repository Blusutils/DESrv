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
            cEnd.Run(DESConnections.DESWebSockets.whiletrue);
        }
    }
}
