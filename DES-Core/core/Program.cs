namespace DESCore
{
    class Program
    {
        static void Main(string[] args)
        {
            var coreRunner = new DESCoreRunner();
            coreRunner.SetupCEnd(new DESCEnd.CEnd(new DESCEnd.Logging.CEndLog() {
                ConsoleLoggingLevel =
                //#if PROD
                    //DESCEnd.LogLevel.Info,
                //#else
                    DESCEnd.Logging.LogLevel.Debug,
                //#endif
                FileLoggingLevel =
                //#if PROD
                    //DESCEnd.LogLevel.Info,
                //#else
                    DESCEnd.Logging.LogLevel.Debug,
                //#endif
                ConsoleLogging = true,
                FileLogging = new DESCEnd.Logging.FileLogger()
            }, new DESCEnd.ExceptionPlus()));
            
            coreRunner.Setup(args, Utils.ConfigReader.Read());
            coreRunner.Go();
        }
    }
}