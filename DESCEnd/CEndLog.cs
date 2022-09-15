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
        /// Default directory to write logs here
        /// </summary>
        public string TargetDir = Path.Combine(".", "logs");
        /// <summary>
        /// Logging pattern (useless, because <see cref="String.Format(string, object?[])"/> is not as flexible as i wish)
        /// </summary>
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
            file.WriteLine("\n");
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
        /// Additional file logger
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

        public delegate void OnLogDelegate(LogLevel level, string message, string source, object[] format);
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
        /// Send log to console stdout and file logger (if anything of it passed)
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="level">Log level</param>
        /// <param name="source">Log source</param>
        /// <param name="format">Formattion for console stdio (<see cref="Console.WriteLine(string, object?[]?)"/></param>
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
        /// Send Debug log to console stdout and file logger (if anything of it passed). Simular to: Log(message, LogLevel.Debug, source, object[])
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="source">Log source</param>
        /// <param name="format">Formattion for console stdio (<see cref="Console.WriteLine(string, object?[]?)"/></param>
        public void Debug(string message, string source = null, params object[] format) {
            Log(message, LogLevel.Debug, source ?? LogSource, format);
        }
        /// <summary>
        /// Send Notice log to console stdout and file logger (if anything of it passed). Simular to: Log(message, LogLevel.Notice, source, object[])
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="source">Log source</param>
        /// <param name="format">Formattion for console stdio (<see cref="Console.WriteLine(string, object?[]?)"/></param>
        public void Notice(string message, string source = null, params object[] format) {
            Log(message, LogLevel.Notice, source ?? LogSource, format);
        }
        /// <summary>
        /// Send Info log to console stdout and file logger (if anything of it passed). Simular to: Log(message, LogLevel.Info, source, object[])
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="source">Log source</param>
        /// <param name="format">Formattion for console stdio (<see cref="Console.WriteLine(string, object?[]?)"/></param>
        public void Info(string message, string source = null, params object[] format) {
            Log(message, LogLevel.Info, source ?? LogSource, format);
        }
        /// <summary>
        /// Send Sucess log to console stdout and file logger (if anything of it passed). Simular to: Log(message, LogLevel.Sucess, source, object[])
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="source">Log source</param>
        /// <param name="format">Formattion for console stdio (<see cref="Console.WriteLine(string, object?[]?)"/></param>
        public void Success(string message, string source = null, params object[] format) {
            Log(message, LogLevel.Success, source ?? LogSource, format);
        }
        /// <summary>
        /// Send Warn log to console stdout and file logger (if anything of it passed). Simular to: Log(message, LogLevel.Warn, source, object[])
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="source">Log source</param>
        /// <param name="format">Formattion for console stdio (<see cref="Console.WriteLine(string, object?[]?)"/></param>
        public void Warn(string message, string source = null, params object[] format) {
            Log(message, LogLevel.Warn, source ?? LogSource, format);
        }
        /// <summary>
        /// Send Error log to console stdout and file logger (if anything of it passed). Simular to: Log(message, LogLevel.Error, source, object[])
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="source">Log source</param>
        /// <param name="format">Formattion for console stdio (<see cref="Console.WriteLine(string, object?[]?)"/></param>
        public void Error(string message, string source = null, params object[] format) {
            Log(message, LogLevel.Error, source ?? LogSource, format);
        }
        /// <summary>
        /// Send Critical log to console stdout and file logger (if anything of it passed). Simular to: Log(message, LogLevel.Critical, source, object[])
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="source">Log source</param>
        /// <param name="format">Formattion for console stdio (<see cref="Console.WriteLine(string, object?[]?)"/></param>
        public void Critical(string message, string source = null, params object[] format) {
            Log(message, LogLevel.Critical, source ?? LogSource, format);
        }
        /// <summary>
        /// Send Fatal log to console stdout and file logger (if anything of it passed). Simular to: Log(message, LogLevel.Fatal, source, object[])
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="source">Log source</param>
        /// <param name="format">Formattion for console stdio (<see cref="Console.WriteLine(string, object?[]?)"/></param>
        public void Fatal(string message, string source = null, params object[] format) {
            Log(message, LogLevel.Fatal, source ?? LogSource, format);
        }
    }
}
