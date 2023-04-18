using Blusutils.DESrv.Configuration;

namespace Blusutils.DESrv.Tests;

public class Tests {
    static Tests() {
        DESrvConfig.Instance = DESrvConfig.Read<DESrvConfig>();

        if (DESrvConfig.Instance == null)
            throw new NullReferenceException(nameof(DESrvConfig.Instance) + " is null");
    } 
    Bootstrapper bootstrapper = new() {
        DESrvVersion = new(2, 0, 0, 0),
            Threader = new(),
            Localization = new() { CurrentLocale = "en-US", Strict = false },
            Logger = new() {
                ConsoleLogging = DESrvConfig.Instance!.useConsoleLogging,
                ConsoleLoggingLevel = Logging.LogLevel.Debug,
                FileLogger = DESrvConfig.Instance.useFileLogging ? new() { TargetDir = "./logs" } : null,
                FileLoggingLevel = Logging.LogLevel.Debug,
                LogSource = "DESrv"
            },
            CommandInputProcessor = new CommandInputProcessor()
        };
    [SetUp]
    public void Setup() {
        new Thread(bootstrapper.Start).Start();
    }

    [Test]
    public void Test1() {
        Assert.Pass();
    }
}