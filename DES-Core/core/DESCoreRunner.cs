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
            var parsed = ArgParser.Parse(args);
            Console.WriteLine($"{String.Join(", ", parsed.Keys.ToList<string>())}\n{String.Join(", ", parsed.Values.ToList<string>())}");
        }
        public void SetupCEnd(DESCEnd.Logging.CEndLog cendLog)
        {
            CEndLog = cendLog;
            CEndLog.LogSource = "DES Runner";
            cendLog.Success("CEnd Setup");
        }
    }
}
