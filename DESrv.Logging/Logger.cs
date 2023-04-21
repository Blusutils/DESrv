using Blusutils.DESrv.Logging.Utils;
using static Blusutils.DESrv.Logging.Utils.AdvFormat;
namespace Blusutils.DESrv.Logging {
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
        /// <param name="source">Log source</param>
        public void Log(string message, LogLevel level, string? source = null) {
            source ??= LogSource;
            OnLogEvent?.Invoke(level, message, source);
            var lvlString = level.ToString().ToUpper();
            var formatForConsole = new Dictionary<string, object> {
                ["Source"] = source ?? "Null",
                ["SourceThread"] = Thread.CurrentThread.Name ?? "Null",
                ["Date"] = DateTime.Now,
                ["Level"] = lvlString.PadRight(8),
                ["Message"] = message ?? "*message isn't provided*"
            };
            var msg = LogMessageSchema.Format(formatForConsole);
            if (ConsoleLogging && level >= ConsoleLoggingLevel) {
                ConsoleService.Console.WriteLine(msg, GetConsoleColor(level));
            };
            if (FileLogger != null && level >= FileLoggingLevel)
                FileLogger.Log(msg, level, source);
        }
        /// <summary>
        /// Send Debug log to console stdout (if enabled) and file logger (if exists). Simular to: <see cref="Log(string, LogLevel, string, object[])"/> where <see cref="LogLevel"/> is <see cref="LogLevel.Debug"/>
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="source">Log source</param>
        public void Debug(string message, string? source = null) {
            Log(message, LogLevel.Debug, source ?? LogSource);
        }
        /// <summary>
        /// Send Notice log to console stdout (if enabled) and file logger (if exists). Simular to: <see cref="Log(string, LogLevel, string, object[])"/> where <see cref="LogLevel"/> is <see cref="LogLevel.Notice"/>
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="source">Log source</param>
        public void Notice(string message, string? source = null) {
            Log(message, LogLevel.Notice, source ?? LogSource);
        }
        /// <summary>
        /// Send Info log to console stdout (if enabled) and file logger (if exists). Simular to: <see cref="Log(string, LogLevel, string, object[])"/> where <see cref="LogLevel"/> is <see cref="LogLevel.Info"/>
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="source">Log source</param>
        public void Info(string message, string? source = null) {
            Log(message, LogLevel.Info, source ?? LogSource);
        }
        /// <summary>
        /// Send Sucess log to console stdout (if enabled) and file logger (if exists). Simular to: <see cref="Log(string, LogLevel, string, object[])"/> where <see cref="LogLevel"/> is <see cref="LogLevel.Success"/>
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="source">Log source</param>
        public void Success(string message, string? source = null) {
            Log(message, LogLevel.Success, source ?? LogSource);
        }
        /// <summary>
        /// Send Warn log to console stdout (if enabled) and file logger (if exists). Simular to: <see cref="Log(string, LogLevel, string, object[])"/> where <see cref="LogLevel"/> is <see cref="LogLevel.Warn"/>
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="source">Log source</param>
        public void Warn(string message, string? source = null) {
            Log(message, LogLevel.Warn, source ?? LogSource);
        }
        /// <summary>
        /// Send Error log to console stdout (if enabled) and file logger (if exists). Simular to: <see cref="Log(string, LogLevel, string, object[])"/> where <see cref="LogLevel"/> is <see cref="LogLevel.Error"/>
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="source">Log source</param>
        public void Error(string message, string? source = null) {
            Log(message, LogLevel.Error, source ?? LogSource);
        }
        /// <summary>
        /// Send Critical log to console stdout (if enabled) and file logger (if exists). Simular to: <see cref="Log(string, LogLevel, string, object[])"/> where <see cref="LogLevel"/> is <see cref="LogLevel.Critical"/>
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="source">Log source</param>
        public void Critical(string message, string? source = null) {
            Log(message, LogLevel.Critical, source ?? LogSource);
        }
        /// <summary>
        /// Send Fatal log to console stdout (if enabled) and file logger (if exists). Simular to: <see cref="Log(string, LogLevel, string, object[])"/> where <see cref="LogLevel"/> is <see cref="LogLevel.Fatal"/>
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="source">Log source</param>
        public void Fatal(string message, string? source = null) {
            Log(message, LogLevel.Fatal, source ?? LogSource);
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

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
        ~Logger() {
            Dispose(false);
        }
    }
}
