using Blusutils.DESrv;
using Blusutils.DESrv.Configuration;

DESrvConfig.Instance = DESrvConfig.Read<DESrvConfig>();

if (DESrvConfig.Instance == null) throw new NullReferenceException(nameof(DESrvConfig.Instance)+" is null");

var logger = new Blusutils.DESrv.Logging.Logger() {
    ConsoleLogging = DESrvConfig.Instance.useConsoleLogging,
    ConsoleLoggingLevel =
#if DEBUG
        Blusutils.DESrv.Logging.LogLevel.Debug,
#else
        DESrvConfig.Instance.isDevelopment ? DESrvConfig.Instance.logLevelDevelopment : DESrvConfig.Instance.logLevel,
#endif
    FileLogger = DESrvConfig.Instance.useFileLogging ? new() { TargetDir = "./logs" } : null,
    FileLoggingLevel =
#if DEBUG
         Blusutils.DESrv.Logging.LogLevel.Debug,
#else
         DESrvConfig.Instance.isDevelopment ? DESrvConfig.Instance.logLevelDevelopment : DESrvConfig.Instance.logLevel,
#endif
    LogSource = "DESrv"
};

var bootstrap = new Bootstrapper() {
    DESrvVersion = new(2, 0, 0, 0),
    Threader = new(),
    Localization = new() { CurrentLocale = DESrvConfig.Instance.locale ?? "en-US", Strict = false },
    Logger = logger
};

var cts = new CancellationTokenSource();

bootstrap.Start(cts.Token);
