//#define PROD//
using System;
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
            // setup DESCEndLib runner
            coreRunner.SetupCEnd(new DESCEnd.CEnd(new DESCEnd.Logging.CEndLog() {
                ConsoleLoggingLevel =
                #if PROD
                    DESCEnd.Logging.LogLevel.Info,
                #else
                    DESCEnd.Logging.LogLevel.Debug,
                #endif
                FileLoggingLevel =
                #if PROD
                DESCEnd.Logging.LogLevel.Info,
                #else
                    DESCEnd.Logging.LogLevel.Debug,
                #endif
                ConsoleLogging = true,
                FileLogging = new DESCEnd.Logging.FileLogger()
            }, new DESCEnd.Exceptor()));
            // setup runtime and config
            coreRunner.SetupRuntime(args, Utils.ConfigReader.Read());
            // run
            try { coreRunner.Go(); } finally { Console.WriteLine("Press Enter to close console..."); Console.Read(); }
        }
    }
}