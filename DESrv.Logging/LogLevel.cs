namespace Blusutils.DESrv.Logging;

/// <summary>
/// Logging levels enumeration
/// </summary>
public enum LogLevel {
    /// <summary>
    /// Debug logs - logs that should be visible only for developers and provide debug information
    /// </summary>
    Debug,
    /// <summary>
    /// Notice logs - represents a small notification
    /// </summary>
    Notice,
    /// <summary>
    /// Information logs - logs with common information about what is going on in the program
    /// </summary>
    Info,
    /// <summary>
    /// Success logs - logs about success operations
    /// </summary>
    Success,
    /// <summary>
    /// Warning logs - logs with warns, non-critical situations in the program to which it is worth paying attention
    /// </summary>
    Warn,
    /// <summary>
    /// Error logs - logs about errors that do not stop the program, but interfere with it
    /// </summary>
    Error,
    /// <summary>
    /// Critical error logs - logs about errors that can harm the program's operation
    /// </summary>
    Critical,
    /// <summary>
    /// Fatal error logs - logs about errors that crash the program
    /// </summary>
    Fatal
}
