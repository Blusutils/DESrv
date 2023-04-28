using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blusutils.DESrv.Configuration;
using Blusutils.DESrv;

namespace Blusutils.DESrv.Tests;
/// <summary>
/// Base setup class for most of tests
/// </summary>
public class Tests {
    static Tests() {
        DESrvConfig.Instance = DESrvConfig.Read<DESrvConfig>();

        if (DESrvConfig.Instance == null)
            throw new NullReferenceException(nameof(DESrvConfig.Instance) + " is null");
    }
    protected Bootstrapper bootstrapper = new() {
        DESrvVersion = new(2, 0, 0, 0),
        Threader = new(),
        Localization = new() { CurrentLocale = "en-US", Strict = false },
        Logger = new() {
            ConsoleLogging = DESrvConfig.Instance!.useConsoleLogging,
            ConsoleLoggingLevel = Blusutils.DESrv.Logging.LogLevel.Debug,
            FileLogger = DESrvConfig.Instance.useFileLogging ? new() { TargetDir = "./logs" } : null,
            FileLoggingLevel = Blusutils.DESrv.Logging.LogLevel.Debug,
            LogSource = "DESrv"
        }
    };
}