using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DESCore.DESCEnd {
    /// <summary>
    /// Class for "Exit Codes" (used by <see cref="Exceptor"/>)
    /// </summary>
    public class ExitCode {
        // exit code
        private uint code;
        // cause: from exception
        private Exception? excepted = null;
        // ...or from string
        private string? reason = null;
        /// <summary>
        /// Create a new instance of <see cref="ExitCode"/>
        /// </summary>
        /// <param name="code">Exit code <see cref="int"/> representation</param>
        /// <param name="reason">Cause of exit</param>
        public ExitCode(uint code, string reason) {
            this.code = code;
            this.reason = reason;
        }
        /// <summary>
        /// Create a new instance of <see cref="ExitCode"/>
        /// </summary>
        /// <param name="code">Exit code <see cref="int"/> representation</param>
        /// <param name="reason">Cause of exit</param>
        public ExitCode(uint code, Exception reason) {
            this.code = code;
            this.excepted = reason;
        }
    }
    /// <summary>
    /// Enumeration for <see cref="ExitCode"/>
    /// </summary>
    public class ExitCodes {
        public static ExitCode Normal = new(0, "All fine");
        public static ExitCode IOConnError = new(100, "Connection issue");
        public static ExitCode Deadcode = new(0xDEADC0DE, "Critical unhandled error, program is dead!");
    }
    /// <summary>
    /// Exceptor - manager for shutting down the program what runned under <see cref="CEnd"/>
    /// </summary>
    class Exceptor {
    }
}
