using System.Reflection;

namespace DESCore {
    /// <summary>
    /// DESrv entrypoint class
    /// </summary>
    sealed class Program {
        /// <summary>
        /// DESrv entrypoint
        /// </summary>
        static void Main(string[] args) {
            // create runner
            var coreRunner = new DESCoreRunner();

            // get logging level
            var loglevel = System.Diagnostics.FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).IsDebug ?
                    DESCEnd.Logging.LogLevel.Info : DESCEnd.Logging.LogLevel.Debug;
            // setup runtime and config
            coreRunner.SetupRuntime(args, DESCEnd.Config.ConfigReader.Read<OurConfig>());
            // setup DESCEndLib runner
            coreRunner.SetupCEnd(new DESCEnd.CEnd(new DESCEnd.Logging.CEndLog() {
                ConsoleLoggingLevel = loglevel,
                FileLoggingLevel = loglevel,
                ConsoleLogging = true,
                FileLogging = new DESCEnd.Logging.FileLogger()
            }));
            // run
            try { coreRunner.Go(); } finally { Console.WriteLine(DESCoreRunner.Localizer.Translate("desrv.main.closeconsole", "Press any key to close console...")); Console.ReadKey(); }
        }
    }
}