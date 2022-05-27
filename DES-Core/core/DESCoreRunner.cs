using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DESCore
{
    class DESCoreRunner
    {

        public DESCoreRunner ()
        {

        }
        public void Setup(string[] args)
        {

        }
        public void SetupCEnd(DESCEnd.Logging.CEndLog cendLog)
        {
            cendLog.Success("Setup");
            cendLog.Warn("Seems you are stupid...");
            cendLog.Notice("da");
        }
    }
}
