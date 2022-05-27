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
        public void Log(string message)
        {

        }
    }
    public class CEndLog
    {
        public bool ConsoleLogging = false;
        public FileLogger? FileLogging = null;
        public LogLevel LoggingLevel = LogLevel.Debug;
        public string LogSource = "CEnd";
        public CEndLog ()
        {

        }
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
        public void Log(string message, LogLevel level, params object[] format)
        {
            var msg = $"[{LogSource} | {level.ToString().ToUpper()} | {System.DateTime.Now}] {message}";
            Console.ForegroundColor = GetConsoleColor(level);
            Console.WriteLine(msg, format);
            Console.ResetColor();
            FileLogging.Log(msg);
        }
        public void Debug(string message, params object[] format)
        {
            Log(message, LogLevel.Debug, format);
        }
        public void Notice(string message, params object[] format)
        {
            Log(message, LogLevel.Notice, format);
        }
        public void Info(string message, params object[] format)
        {
            Log(message, LogLevel.Info, format);
        }
        public void Success(string message, params object[] format)
        {
            Log(message, LogLevel.Success, format);
        }
        public void Warn(string message, params object[] format)
        {
            Log(message, LogLevel.Warn, format);
        }
        public void Error(string message, params object[] format)
        {
            Log(message, LogLevel.Error, format);
        }
        public void Critical(string message, params object[] format)
        {
            Log(message, LogLevel.Critical, format);
        }
        public void Fatal(string message, params object[] format)
        {
            Log(message, LogLevel.Fatal, format);
        }
    }
}
