using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DESCore.DESCEnd
{
    public class ExitCode
    {
        private uint code;
        private Exception? excepted = null;
        private string? reason = null;

        public ExitCode(uint code, string reason)
        {
            this.code = code;
            this.reason = reason;
        }
        public ExitCode(uint code, Exception reason)
        {
            this.code = code;
            this.excepted = reason;
        }
    }
    public class ExitCodes
    {
        public static ExitCode Normal = new(0, "All fine");
        public static ExitCode IOConnError = new(100, "Connection issue");
        public static ExitCode Deadcode = new(0xDEADC0DE, "Critical unhandled error, server dead!");
    }
    class ExceptionPlus
    {
    }
}
