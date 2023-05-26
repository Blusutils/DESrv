using System.Drawing;
using Blusutils.DESrv.Logging.Utils;
using static System.Net.Mime.MediaTypeNames;
using static Blusutils.DESrv.Logging.Utils.AdvFormat;

namespace Blusutils.DESrv.Logging;

/// <summary>
/// Main logger
/// </summary>
public class Logger : IDisposable, IDESrvLogService {
    bool disposed = false;
    /// <summary>
    /// Is logs sends to console stdout?
    /// </summary>
    public bool ConsoleLogging { get; set; } = false;
    /// <summary>
    /// File logger
    /// </summary>
    public FileLogger? FileLogger { get; set; } = null;
    /// <summary>
    /// Logging level filter (console stdout)
    /// </summary>
    public LogLevel ConsoleLoggingLevel { get; set; } = LogLevel.Debug;
    /// <summary>
    /// Logging level filter (file logger)
    /// </summary>
    public LogLevel FileLoggingLevel { get; set; } = LogLevel.Debug;
    /// <summary>
    /// Default logs source
    /// </summary>
    public string LogSource { get; set; } = "DESCEnd";
    /// <summary>
    /// Schema of log messages
    /// </summary>
    public string LogMessageSchema { get; set; } = "[{Date} | {Level} | {Source}/{SourceThread}] {Message}";

    /// <summary>
    /// Delegate for log events
    /// </summary>
    /// <param name="level">Level of log</param>
    /// <param name="message">Message</param>
    /// <param name="source">Source of log - where logger got log</param>
    public delegate void OnLogDelegate(LogLevel level, string message, string source);
    /// <summary>
    /// Event triggered when the log is received
    /// </summary>
    public event OnLogDelegate? OnLogEvent;

    /// <summary>
    /// Get console color by logging level
    /// </summary>
    /// <param name="level">Logging level</param>
    /// <returns>Color from <see cref="ConsoleColor"/></returns>
    static  ConsoleColor GetConsoleColor(LogLevel level) {
        return level switch {
            LogLevel.Debug => ConsoleColor.Gray,
            LogLevel.Notice => ConsoleColor.Cyan,
            LogLevel.Info => ConsoleColor.Blue,
            LogLevel.Success => ConsoleColor.Green,
            LogLevel.Warn => ConsoleColor.Yellow,
            LogLevel.Error => ConsoleColor.Red,
            LogLevel.Critical => ConsoleColor.Magenta,
            LogLevel.Fatal => ConsoleColor.DarkMagenta,
            _ => ConsoleColor.White,
        };
    }
    /// <summary>
    /// Send log to console stdout (if enabled) and file logger (if exists)
    /// </summary>
    /// <param name="message">Message to send</param>
    /// <param name="level">Log level</param>
    /// <param name="exception">Exception associated with log</param>
    /// <param name="source">Log source</param>
    public void Log(string message, LogLevel level, Exception? exception = null, string? source = null) {
        source ??= LogSource;
        OnLogEvent?.Invoke(level, message, source);
        var lvlString = level.ToString().ToUpper();
        var formatForConsole = new Dictionary<string, object> {
            ["Source"] = source ?? "Unknown source",
            ["SourceThread"] = Thread.CurrentThread.Name ?? "Main thread",
            ["Date"] = DateTime.Now,
            ["Level"] = lvlString.PadRight(8),
            ["Message"] = message ?? "*message isn't provided*"
        };
        var msg = LogMessageSchema.Format(formatForConsole);

        if (exception is not null)
            msg += $"\n\t{exception.GetType().FullName} from {exception.TargetSite} (caused by {exception.Source}): {exception.Message}\n\t{exception.StackTrace}";

        if (ConsoleLogging && level >= ConsoleLoggingLevel) {
            var fg = Console.ForegroundColor;
            Console.ForegroundColor = GetConsoleColor(level);
            Console.WriteLine(msg);
            Console.ForegroundColor = fg;
        };
        if (FileLogger != null && level >= FileLoggingLevel)
            FileLogger.Log(msg, level, exception, source);
    }
    /// <summary>
    /// Send Debug log to console stdout (if enabled) and file logger (if exists). Simular to: <see cref="Log(string, LogLevel, Exception?, string?)"/> with <see cref="LogLevel.Debug"/>
    /// </summary>
    /// <param name="message">Message to send</param>
    /// <param name="exception">Exception associated with log</param>
    /// <param name="source">Log source</param>
    public void Debug(string message, Exception? exception = null, string? source = null) {
        Log(message, LogLevel.Debug, exception, source ?? LogSource);
    }
    /// <summary>
    /// Send Notice log to console stdout (if enabled) and file logger (if exists). Simular to: <see cref="Log(string, LogLevel, Exception?, string?)"/> with <see cref="LogLevel.Notice"/>
    /// </summary>
    /// <param name="message">Message to send</param>
    /// <param name="exception">Exception associated with log</param>
    /// <param name="source">Log source</param>
    public void Notice(string message, Exception? exception = null, string? source = null) {
        Log(message, LogLevel.Notice, exception, source ?? LogSource);
    }
    /// <summary>
    /// Send Info log to console stdout (if enabled) and file logger (if exists). Simular to: <see cref="Log(string, LogLevel, Exception?, string?)"/> with <see cref="LogLevel.Info"/>
    /// </summary>
    /// <param name="message">Message to send</param>
    /// <param name="exception">Exception associated with log</param>
    /// <param name="source">Log source</param>
    public void Info(string message, Exception? exception = null, string? source = null) {
        Log(message, LogLevel.Info, exception, source ?? LogSource);
    }
    /// <summary>
    /// Send Sucess log to console stdout (if enabled) and file logger (if exists). Simular to: <see cref="Log(string, LogLevel, Exception?, string?)"/> with <see cref="LogLevel.Success"/>
    /// </summary>
    /// <param name="message">Message to send</param>
    /// <param name="exception">Exception associated with log</param>
    /// <param name="source">Log source</param>
    public void Success(string message, Exception? exception = null, string? source = null) {
        Log(message, LogLevel.Success, exception, source ?? LogSource);
    }
    /// <summary>
    /// Send Warn log to console stdout (if enabled) and file logger (if exists). Simular to: <see cref="Log(string, LogLevel, Exception?, string?)"/> with <see cref="LogLevel.Warn"/>
    /// </summary>
    /// <param name="message">Message to send</param>
    /// <param name="exception">Exception associated with log</param>
    /// <param name="source">Log source</param>
    public void Warn(string message, Exception? exception = null, string? source = null) {
        Log(message, LogLevel.Warn, exception, source ?? LogSource);
    }
    /// <summary>
    /// Send Error log to console stdout (if enabled) and file logger (if exists). Simular to: <see cref="Log(string, LogLevel, Exception?, string?)"/> with <see cref="LogLevel.Error"/>
    /// </summary>
    /// <param name="message">Message to send</param>
    /// <param name="exception">Exception associated with log</param>
    /// <param name="source">Log source</param>
    public void Error(string message, Exception? exception = null, string? source = null) {
        Log(message, LogLevel.Error, exception, source ?? LogSource);
    }
    /// <summary>
    /// Send Critical log to console stdout (if enabled) and file logger (if exists). Simular to: <see cref="Log(string, LogLevel, Exception?, string?)"/> with <see cref="LogLevel.Critical"/>
    /// </summary>
    /// <param name="message">Message to send</param>
    /// <param name="exception">Exception associated with log</param>
    /// <param name="source">Log source</param>
    public void Critical(string message, Exception? exception = null, string? source = null) {
        Log(message, LogLevel.Critical, exception, source ?? LogSource);
    }
    /// <summary>
    /// Send Fatal log to console stdout (if enabled) and file logger (if exists). Simular to: <see cref="Log(string, LogLevel, Exception?, string?)"/> with <see cref="LogLevel.Fatal"/>
    /// </summary>
    /// <param name="message">Message to send</param>
    /// <param name="exception">Exception associated with log</param>
    /// <param name="source">Log source</param>
    public void Fatal(string message, Exception? exception = null, string? source = null) {
        Log(message, LogLevel.Fatal, exception, source ?? LogSource);
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
            FileLogger?.Dispose();
            LogMessageSchema = null;
            LogSource = null;
            OnLogEvent = null;
        }
        disposed = true;
    }

    /// <inheritdoc/>
    ~Logger() {
        Dispose(false);
    }
}
