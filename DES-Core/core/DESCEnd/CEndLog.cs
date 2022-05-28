using System;

namespace DESCore.DESCEnd.Logging
{
    public enum LogLevel
    {
        Debug,
        Notice,
        Info,
        Success,
        Warn,
        Error,
        Critical,
        Fatal
    }
    public class FileLogger
    {
        public string TargetDir = Path.Combine(Environment.CurrentDirectory, "logs");
        public string LogNameSchema = "{0}-{1}";
        public string LogSource = null;
        public void Log(string message, string source = null, params object[] format)
        {
            if (!Directory.Exists(TargetDir)) {
                Console.Error.WriteLine("Log directory not found, creating default");
                Directory.CreateDirectory(TargetDir);
            };
            using StreamWriter file = new(TargetDir + "\\" + String.Format(LogNameSchema, source??LogSource, $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}") + ".log", append: true);
            file.WriteLine(message+(format.Length != 0 ? String.Join(' ', format) : ""));
        }
        ~FileLogger ()
        {
            using StreamWriter file = new(TargetDir + "\\" + String.Format(LogNameSchema, LogSource, $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}") + ".log", append: true);
            file.WriteLine("\n");
        }
    }
    public class CEndLog
    {
        public bool ConsoleLogging = false;
        public FileLogger? FileLogging = null;
        public LogLevel ConsoleLoggingLevel = LogLevel.Debug;
        public LogLevel FileLoggingLevel = LogLevel.Debug;
        public string LogSource = "CEnd";
        
        private ConsoleColor GetConsoleColor(LogLevel level)
        {
            switch (level)
            {
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
        public void Log(string message, LogLevel level, string source ="CEnd", params object[] format)
        {
            source = source ?? LogSource;
            var msg = $"[{source} | {level.ToString().ToUpper()} | {DateTime.Now}] {message}";
            if(level >= ConsoleLoggingLevel) {
              Console.ForegroundColor = GetConsoleColor(level);
              Console.WriteLine(msg, format);
              Console.ResetColor();
            };
            if (FileLogging == null) return;
            if(level >= FileLoggingLevel)
              FileLogging.Log(msg, source);
        }
        public void Debug(string message, string source = null, params object[] format)
        {
            Log(message, LogLevel.Debug, source??LogSource, format);
        }
        public void Notice(string message, string source = null, params object[] format)
        {
            Log(message, LogLevel.Notice, source??LogSource, format);
        }
        public void Info(string message, string source = null, params object[] format)
        {
            Log(message, LogLevel.Info, source??LogSource, format);
        }
        public void Success(string message, string source = null, params object[] format)
        {
            Log(message, LogLevel.Success, source??LogSource, format);
        }
        public void Warn(string message, string source = null, params object[] format)
        {
            Log(message, LogLevel.Warn, source??LogSource, format);
        }
        public void Error(string message, string source = null, params object[] format)
        {
            Log(message, LogLevel.Error, source??LogSource, format);
        }
        public void Critical(string message, string source = null, params object[] format)
        {
            Log(message, LogLevel.Critical, source??LogSource, format);
        }
        public void Fatal(string message, string source = null, params object[] format)
        {
            Log(message, LogLevel.Fatal, source??LogSource, format);
        }
    }
}
