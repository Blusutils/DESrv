using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blusutils.DESrv.Logging {
    interface IDESrvLogService {
        public void Log(string message, LogLevel level, string? source = null);
    }
}
