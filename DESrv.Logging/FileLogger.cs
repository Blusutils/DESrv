using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blusutils.DESrv.Logging.Utils;

namespace Blusutils.DESrv.Logging;

/// <summary>
/// File logger
/// </summary>
public class FileLogger : IDisposable, IDESrvLogService {

    bool disposed = false;

    /// <summary>
    /// Default directory to write logs
    /// </summary>
    public string? TargetDir { get; set; } = null;

    /// <summary>
    /// Logging pattern
    /// </summary>
    public string LogNameSchema { get; set; } = "DESrvLog-{Date}";

    /// <summary>
    /// Where logger got log
    /// </summary>
    public string? LogSource { get; set; } = null;
    // TODO: more variants
    Dictionary<string, object> formats = new (){
        ["Date"] = $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}"
    };

    /// <summary>
    /// Send log to the logger
    /// </summary>
    /// <param name="message">Message to send</param>
    /// <param name="level">Level of log</param>
    /// <param name="exception">Exception associated with log</param>
    /// <param name="source">Log source</param>
    public void Log(string message, LogLevel level, Exception? exception = null, string? source = null) {
        if (TargetDir is null) throw new NullReferenceException("target logging directory is not set");
        if (!Directory.Exists(TargetDir)) {
            Console.Error.WriteLine("Log directory not found, creating default");
            Directory.CreateDirectory(TargetDir);
        };
        using var logFile = new StreamWriter(Path.Combine(TargetDir, AdvFormat.Format(LogNameSchema, formats) + ".log"), append: true);
        if (exception is not null)
            message += $"\n\t{exception.GetType().FullName} from {exception.TargetSite} (caused by {exception.Source}): {exception.Message}\n\t{exception.StackTrace}";
        logFile.WriteLine(message);
        logFile.Flush();
    }

    /// <inheritdoc/>
    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    /// <param name="disposing">Is object starting disposing manually</param>
    protected virtual void Dispose(bool disposing) {
        if (disposed) return;
        if (disposing) {
            if (TargetDir is not null || Directory.Exists(TargetDir)) {
                using var logFile = new StreamWriter(Path.Combine(TargetDir, AdvFormat.Format(LogNameSchema, formats) + ".log"), append: true);
                logFile.WriteLine("\r\n\r\n");
                logFile.Flush();
                logFile.Close();
            }
            LogNameSchema = null;
            LogSource = null;
            formats = null;
            TargetDir = null;
        }
        disposed = true;
    }

    /// <inheritdoc/>
    ~FileLogger() {
        Dispose(false);
    }
}
