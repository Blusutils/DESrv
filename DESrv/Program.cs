using Blusutils.DESrv;
using Blusutils.DESrv.Configuration;
using Blusutils.DESrv.Localization;

DESrvConfig.Instance = DESrvConfig.Read<DESrvConfig>() ?? throw new NullReferenceException("config is null");

var logLevel =
#if DEBUG
        Blusutils.DESrv.Logging.LogLevel.Debug;
#else
        DESrvConfig.Instance.isDevelopment ? DESrvConfig.Instance.logLevelDevelopment : DESrvConfig.Instance.logLevel;
#endif

var logger = new Blusutils.DESrv.Logging.Logger() {
    ConsoleLogging = DESrvConfig.Instance.useConsoleLogging,
    ConsoleLoggingLevel = logLevel,
    FileLogger = DESrvConfig.Instance.useFileLogging ? new() { TargetDir = DESrvConfig.Instance.logsDir ?? "./logs" } : null,
    FileLoggingLevel = logLevel,
    LogSource = "DESrv"
};

var localizer = new LocalizationProvider() { CurrentLocale = DESrvConfig.Instance.locale ?? "en-US", Strict = false };
if (!Directory.Exists("translations")) Directory.CreateDirectory("translations");
localizer.Load("translations");

Bootstrapper.Logger = logger;
Bootstrapper.Localization = localizer;
Bootstrapper.Threader = new();

Bootstrapper.Start(new());
