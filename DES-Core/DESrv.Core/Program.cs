using System.Reflection;
namespace DESrv {
    public class Program {
        public static void Main(string[] args) {
            // create runner
            var coreRunner = new DESCoreRunner();

            // get logging level
            var loglevel =
#if !DEBUG
                DESCEnd.Logging.LogLevel.Info;
#else
                DESCEnd.Logging.LogLevel.Debug;
#endif
            // create config in any way
            if (!File.Exists("config.json")) DESrv.Config.Program.Main(Array.Empty<string>());
            // setup runtime and config
            coreRunner.SetupRuntime(args, DESCEnd.Config.ConfigReader.Read<DESrv.Config.OurConfig>());
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