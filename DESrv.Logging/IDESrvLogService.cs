using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blusutils.DESrv.Logging; 

/// <summary>
/// Logging service interface
/// </summary>
interface IDESrvLogService {

    /// <summary>
    /// Send a log
    /// </summary>
    /// <param name="message">Message to send</param>
    /// <param name="level">Log level</param>
    /// <param name="source">Log source</param>
    public void Log(string message, LogLevel level, string? source = null);
}
