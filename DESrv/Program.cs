using Blusutils.DESrv;
using Blusutils.DESrv.Configuration;

DESrvConfig.Instance = DESrvConfig.Read<DESrvConfig>();

if (DESrvConfig.Instance == null) throw new NullReferenceException(nameof(DESrvConfig.Instance)+" is null");

var logger = new Blusutils.DESrv.Logging.Logger() {
    ConsoleLogging = DESrvConfig.Instance.UseConsoleLogging,
    ConsoleLoggingLevel =
#if DEBUG
        Blusutils.DESrv.Logging.LogLevel.Debug,
#else
        DESrvConfig.Instance.IsDevelopment ? DESrvConfig.Instance.LogLevelDevelopment : DESrvConfig.Instance.LogLevel,
#endif
    FileLogger = DESrvConfig.Instance.UseFileLogging ? new() { TargetDir = "./logs" } : null,
    FileLoggingLevel =
#if DEBUG
         Blusutils.DESrv.Logging.LogLevel.Debug,
#else
         DESrvConfig.Instance.IsDevelopment ? DESrvConfig.Instance.LogLevelDevelopment : DESrvConfig.Instance.LogLevel,
#endif
    LogSource = "DESrv"
};

var bootstrap = new Bootstrapper () {
    DESrvVersion = new (2, 0, 0, 0),
    Localization = new() { CurrentLocale = DESrvConfig.Instance.Locale?? "en-US", Strict = false },
    Logger = logger,
};

bootstrap.Start();