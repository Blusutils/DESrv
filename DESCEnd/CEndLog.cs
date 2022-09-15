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
        /// Logging pattern (useless, because <see cref="String.Format(string, object?[])"/> is not as flexible as i wish)
        /// </summary>
        [Obsolete]
        public string LogNameSchema = "{0}-{1}";
        /// <summary>
        /// Where logger got log
        /// </summary>
        public string LogSource = null;
        /// <summary>
        /// Send log to the logger
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="source">Log source</param>
        public void Log(string message, string source = null) {
            if (!Directory.Exists(TargetDir)) {
                Console.Error.WriteLine("Log directory not found, creating default");
                Directory.CreateDirectory(TargetDir);
            };
            using StreamWriter file = new(TargetDir + "\\" + String.Format(LogNameSchema, "DESCendLog", $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}") + ".log", append: true);
            file.WriteLine(message);
        }
        // separate different runs of programm by new line
        ~FileLogger() {
            using StreamWriter file = new(TargetDir + "\\" + String.Format(LogNameSchema, "DESCendLog", $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}") + ".log", append: true);
            file.WriteLine("\r\n\r\n");
        }
    }
    /// <summary>
    /// <see cref="CEnd"/> main logger
    /// </summary>
    public class CEndLog {
        /// <summary>
        /// Is logs sends to console stdout?
        /// </summary>
        public bool ConsoleLogging = false;
        /// <summary>
        /// File logger
        /// </summary>
        public FileLogger? FileLogging = null;
        /// <summary>
        /// Logging level filter (console stdout)
        /// </summary>
        public LogLevel ConsoleLoggingLevel = LogLevel.Debug;
        /// <summary>
        /// Logging level filter (file logger)
        /// </summary>
        public LogLevel FileLoggingLevel = LogLevel.Debug;
        /// <summary>
        /// Default logs source
        /// </summary>
        public string LogSource = "CEnd";

        /// <summary>
        /// Delegate for log events
        /// </summary>
        /// <param name="level">Level of log</param>
        /// <param name="message">Message</param>
        /// <param name="source">Source of log - where logger got log</param>
        /// <param name="format">Additional params for formattion</param>
        public delegate void OnLogDelegate(LogLevel level, string message, string source, object[] format);
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
        /// <param name="format">Formattion for <see cref="Console.WriteLine(string, object?[]?)"/> and <see cref="FileLogger"/></param>
        public void Log(string message, LogLevel level, string source = "DES CEnd", params object[] format) {
            source = source ?? LogSource;
            if (OnLog != null) OnLog(level, message, source, format);
            var msg = $"[{source} | {DateTime.Now} | {level.ToString().ToUpper()}] {message}";
            if (ConsoleLogging && level >= ConsoleLoggingLevel) {
                Console.ForegroundColor = GetConsoleColor(level);
                Console.WriteLine(msg, format);
                Console.ResetColor();
            };
            if (FileLogging != null && level >= FileLoggingLevel)
                FileLogging.Log(msg, source);
        }
        /// <summary>
        /// Send Debug log to console stdout (if enabled) and file logger (if exists). Simular to: <see cref="Log(string, LogLevel, string, object[])"/> where <see cref="LogLevel"/> is <see cref="LogLevel.Debug"/>
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="source">Log source</param>
        /// <param name="format">Formattion</param>
        public void Debug(string message, string source = null, params object[] format) {
            Log(message, LogLevel.Debug, source ?? LogSource, format);
        }
        /// <summary>
        /// Send Notice log to console stdout (if enabled) and file logger (if exists). Simular to: <see cref="Log(string, LogLevel, string, object[])"/> where <see cref="LogLevel"/> is <see cref="LogLevel.Notice"/>
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="source">Log source</param>
        /// <param name="format">Formattion</param>
        public void Notice(string message, string source = null, params object[] format) {
            Log(message, LogLevel.Notice, source ?? LogSource, format);
        }
        /// <summary>
        /// Send Info log to console stdout (if enabled) and file logger (if exists). Simular to: <see cref="Log(string, LogLevel, string, object[])"/> where <see cref="LogLevel"/> is <see cref="LogLevel.Info"/>
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="source">Log source</param>
        /// <param name="format">Formattion</param>
        public void Info(string message, string source = null, params object[] format) {
            Log(message, LogLevel.Info, source ?? LogSource, format);
        }
        /// <summary>
        /// Send Sucess log to console stdout (if enabled) and file logger (if exists). Simular to: <see cref="Log(string, LogLevel, string, object[])"/> where <see cref="LogLevel"/> is <see cref="LogLevel.Success"/>
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="source">Log source</param>
        /// <param name="format">Formattion</param>
        public void Success(string message, string source = null, params object[] format) {
            Log(message, LogLevel.Success, source ?? LogSource, format);
        }
        /// <summary>
        /// Send Warn log to console stdout (if enabled) and file logger (if exists). Simular to: <see cref="Log(string, LogLevel, string, object[])"/> where <see cref="LogLevel"/> is <see cref="LogLevel.Warn"/>
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="source">Log source</param>
        /// <param name="format">Formattion</param>
        public void Warn(string message, string source = null, params object[] format) {
            Log(message, LogLevel.Warn, source ?? LogSource, format);
        }
        /// <summary>
        /// Send Error log to console stdout (if enabled) and file logger (if exists). Simular to: <see cref="Log(string, LogLevel, string, object[])"/> where <see cref="LogLevel"/> is <see cref="LogLevel.Error"/>
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="source">Log source</param>
        /// <param name="format">Formattion</param>
        public void Error(string message, string source = null, params object[] format) {
            Log(message, LogLevel.Error, source ?? LogSource, format);
        }
        /// <summary>
        /// Send Critical log to console stdout (if enabled) and file logger (if exists). Simular to: <see cref="Log(string, LogLevel, string, object[])"/> where <see cref="LogLevel"/> is <see cref="LogLevel.Critical"/>
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="source">Log source</param>
        /// <param name="format">Formattion</param>
        public void Critical(string message, string source = null, params object[] format) {
            Log(message, LogLevel.Critical, source ?? LogSource, format);
        }
        /// <summary>
        /// Send Fatal log to console stdout (if enabled) and file logger (if exists). Simular to: <see cref="Log(string, LogLevel, string, object[])"/> where <see cref="LogLevel"/> is <see cref="LogLevel.Fatal"/>
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="source">Log source</param>
        /// <param name="format">Formattion</param>
        public void Fatal(string message, string source = null, params object[] format) {
            Log(message, LogLevel.Fatal, source ?? LogSource, format);
        }
    }
}
