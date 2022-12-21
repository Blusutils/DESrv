using Blusutils.DESrv;

var bootstrap = new Bootstrapper () {
    DESrvVersion = new (2, 0, 0, 0),
    Localization = new() { CurrentLocale = "en-US", Strict = false },
    Logger = new() {
        ConsoleLogging= true,
        ConsoleLoggingLevel = 
    #if DEBUG
        Blusutils.DESrv.Logging.LogLevel.Debug,
    #else
        Blusutils.DESrv.Logging.LogLevel.Info,
    #endif
        FileLogger= new() { TargetDir = "./logs" },
        FileLoggingLevel =
    #if DEBUG
         Blusutils.DESrv.Logging.LogLevel.Debug,
    #else
         Blusutils.DESrv.Logging.LogLevel.Info,
    #endif
        LogSource = "DESrv"
    },
};

bootstrap.Start();