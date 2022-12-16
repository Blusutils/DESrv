using DESCEnd.TextUtils;

namespace DESCEnd.Logging {
    /// <summary>
    /// Logging levels enumeration
    /// </summary>
    public enum LogLevel {
        Debug,
        Notice,
        Info,
        Success,
        Warn,
        Error,
        Critical,
        Fatal
    }
    /// <summary>
    /// <see cref="CEnd"/> File logger
    /// </summary>
    public class FileLogger {
        /// <summary>
        /// Default directory to write logs
        /// </summary>
        public string TargetDir = Path.Combine(".", "logs");
        /// <summary>
        /// Logging pattern
        /// </summary>
        public string LogNameSchema = "DESCEndLog-{Date}";
        /// <summary>
        /// Where logger got log
        /// </summary>
        public string LogSource = null;

        StreamWriter logFile;
        // todo
        Dictionary<string, object> formats = new Dictionary<string, object> {
            ["Date"] = $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}"
        };
        /// <summary>
        /// Send log to the logger
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="source">Log source</param>
        public void Log(string message) {
            if (!Directory.Exists(TargetDir)) {
                Console.Error.WriteLine("Log directory not found, creating default");
                Directory.CreateDirectory(TargetDir);
            };
            if (logFile == null) {
                logFile = new(Path.Combine(TargetDir,TextUtils.AdvFormat.Format(LogNameSchema, formats) + ".log"), append: true);
            }
            logFile.WriteLine(message);
            logFile.Flush();
        }
        // separate different runs of programm by new line
        ~FileLogger() {
            logFile.WriteLine("\r\n\r\n");
            logFile.Close();
            logFile = null;
        }
    }
    /// <summary>
    /// <see cref="CEnd"/> main logger
    /// </summary>
    public class CEndLog {
        /// <summary>
        /// Is logs sends to console stdout?
        /// </summary>
        public bool ConsoleLogging { get; set; } = false;
        /// <summary>
        /// File logger
        /// </summary>
        public FileLogger? FileLogging { get; set; } = null;
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
        /// <param name="format">Additional params for formattion</param>
        public delegate void OnLogDelegate(LogLevel level, string message, string source);
        /// <summary>
        /// Event triggered when the log is received
        /// </summary>
        public event OnLogDelegate OnLog;

        /// <summary>
        /// Get console color by logging level
        /// </summary>
        /// <param name="level">Logging level</param>
        /// <returns>Color from <see cref="ConsoleColor"/></returns>
        private ConsoleColor GetConsoleColor(LogLevel level) {
            switch (level) {
                case LogLevel.Debug: return ConsoleColor.Gray;
                case LogLevel.Notice: return ConsoleColor.Cyan;
                case LogLevel.Info: return ConsoleColor.Blue;
                case LogLevel.Success: return ConsoleColor.Green;
                case LogLevel.Warn: return ConsoleColor.Yellow;
                case LogLevel.Error: return ConsoleColor.Red;
                case LogLevel.Critical: return ConsoleColor.Magenta;
                case LogLevel.Fatal: return ConsoleColor.DarkMagenta;
                default: return ConsoleColor.White;
            }
        }
        /// <summary>
        /// Send log to console stdout (if enabled) and file logger (if exists)
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="level">Log level</param>
        /// <param name="source">Log source</param>
        public void Log(string message, LogLevel level, string source = null) {
            source = source ?? LogSource;
            OnLog?.Invoke(level, message, source);
            var lvlString = level.ToString().ToUpper();
            var formatForConsole = new Dictionary<string, object> {
                ["Source"] = source??"Null",
                ["SourceThread"] = Thread.CurrentThread.Name??"Null",
                ["Date"] = DateTime.Now,
                ["Level"] = lvlString + string.Join("", Enumerable.Repeat(" ", 8-lvlString.Length)),
                ["Message"] = message??"*message isn't provided*"
            };
            var msg = LogMessageSchema.Format(formatForConsole);
            if (ConsoleLogging && level >= ConsoleLoggingLevel) {
                Console.ForegroundColor = GetConsoleColor(level);
                Console.WriteLine(msg);
                Console.ResetColor();
            };
            if (FileLogging != null && level >= FileLoggingLevel)
                FileLogging.Log(msg);
        }
        /// <summary>
        /// Send Debug log to console stdout (if enabled) and file logger (if exists). Simular to: <see cref="Log(string, LogLevel, string, object[])"/> where <see cref="LogLevel"/> is <see cref="LogLevel.Debug"/>
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="source">Log source</param>
        public void Debug(string message, string source = null) {
            Log(message, LogLevel.Debug, source ?? LogSource);
        }
        /// <summary>
        /// Send Notice log to console stdout (if enabled) and file logger (if exists). Simular to: <see cref="Log(string, LogLevel, string, object[])"/> where <see cref="LogLevel"/> is <see cref="LogLevel.Notice"/>
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="source">Log source</param>
        public void Notice(string message, string source = null) {
            Log(message, LogLevel.Notice, source ?? LogSource);
        }
        /// <summary>
        /// Send Info log to console stdout (if enabled) and file logger (if exists). Simular to: <see cref="Log(string, LogLevel, string, object[])"/> where <see cref="LogLevel"/> is <see cref="LogLevel.Info"/>
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="source">Log source</param>
        public void Info(string message, string source = null) {
            Log(message, LogLevel.Info, source ?? LogSource);
        }
        /// <summary>
        /// Send Sucess log to console stdout (if enabled) and file logger (if exists). Simular to: <see cref="Log(string, LogLevel, string, object[])"/> where <see cref="LogLevel"/> is <see cref="LogLevel.Success"/>
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="source">Log source</param>
        public void Success(string message, string source = null) {
            Log(message, LogLevel.Success, source ?? LogSource);
        }
        /// <summary>
        /// Send Warn log to console stdout (if enabled) and file logger (if exists). Simular to: <see cref="Log(string, LogLevel, string, object[])"/> where <see cref="LogLevel"/> is <see cref="LogLevel.Warn"/>
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="source">Log source</param>
        public void Warn(string message, string source = null) {
            Log(message, LogLevel.Warn, source ?? LogSource);
        }
        /// <summary>
        /// Send Error log to console stdout (if enabled) and file logger (if exists). Simular to: <see cref="Log(string, LogLevel, string, object[])"/> where <see cref="LogLevel"/> is <see cref="LogLevel.Error"/>
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="source">Log source</param>
        public void Error(string message, string source = null) {
            Log(message, LogLevel.Error, source ?? LogSource);
        }
        /// <summary>
        /// Send Critical log to console stdout (if enabled) and file logger (if exists). Simular to: <see cref="Log(string, LogLevel, string, object[])"/> where <see cref="LogLevel"/> is <see cref="LogLevel.Critical"/>
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="source">Log source</param>
        public void Critical(string message, string source = null) {
            Log(message, LogLevel.Critical, source ?? LogSource);
        }
        /// <summary>
        /// Send Fatal log to console stdout (if enabled) and file logger (if exists). Simular to: <see cref="Log(string, LogLevel, string, object[])"/> where <see cref="LogLevel"/> is <see cref="LogLevel.Fatal"/>
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="source">Log source</param>
        public void Fatal(string message, string source = null) {
            Log(message, LogLevel.Fatal, source ?? LogSource);
        }
    }
}
