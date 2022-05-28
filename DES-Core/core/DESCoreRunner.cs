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
        public DESCEnd.Logging.CEndLog GetLogger()
        {
            return CEndLog;
        }
        public DESCoreRunner ()
        {

        }
        public void Setup(string[] args)
        {

        }
        public void SetupCEnd(DESCEnd.Logging.CEndLog cendLog)
        {
            CEndLog = cendLog;
            CEndLog.LogSource = "DES Runner";
            cendLog.Success("Setup");
            cendLog.Warn("Seems you are stupid...");
            cendLog.Notice("da");
        }
    }
}
