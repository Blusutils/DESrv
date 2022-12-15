using System.Reflection;
namespace DESrv {
    public class Program {
        public static void Main(string[] args) {

            DESCoreRunner.DESVersion = new Version(1, 3, 0);

            // create runner
            var coreRunner = new DESCoreRunner();

            // get logging level
            var loglevel =
#if DEBUG
                DESCEnd.Logging.LogLevel.Debug;
#else
                DESCEnd.Logging.LogLevel.Info;
#endif
            // create config in any way
            if (!File.Exists("config.json")) DESrv.Config.Program.Main(Array.Empty<string>());
            // setup DESCEndLib runner
            DESCoreRunner.SetupCEnd(new DESCEnd.CEnd(new DESCEnd.Logging.CEndLog() {
                ConsoleLoggingLevel = loglevel,
                FileLoggingLevel = loglevel,
                ConsoleLogging = true,
                FileLogging = new DESCEnd.Logging.FileLogger()
            }));
            // setup runtime and config
            coreRunner.SetupRuntime(args, DESCEnd.Config.ConfigReader.Read<DESrv.Config.OurConfig>());
            // run
            try { coreRunner.Go(); } finally { Console.WriteLine(DESCoreRunner.Localizer.Translate("desrv.main.closeconsole", "Press any key to close console...")); Console.ReadKey(); }
        }
    }
}