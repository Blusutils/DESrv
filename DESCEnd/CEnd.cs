using DESCEnd.Logging;

namespace DESCEnd {
    /// <summary>
    /// Represents the method that executes on <see cref="CEnd.Run(CEndTargetDelegate)"/>
    /// </summary>
    public delegate void CEndTargetDelegate();
    /// <summary>
    /// DESrv CEnd (ControlledEnd) class
    /// </summary>
    public class CEnd {
        /// <summary>
        /// Logger
        /// </summary>
        public static CEndLog Logger;
        /// <summary>
        /// For <see cref="Target(object)"/>
        /// </summary>
        private Exception? runResult = null;
        /// <summary>
        /// Create a new instance of <see cref="CEnd"/>
        /// </summary>
        public CEnd() { }
        /// <summary>
        /// Create a new instance of <see cref="CEnd"/> with logger
        /// </summary>
        /// <param name="log"><see cref="CEndLog"/> logger</param>
        public CEnd(CEndLog log) {
            Logger = log;
        }
        /// <summary>
        /// Target method runner
        /// </summary>
        /// <param name="target">Target method as <see cref="CEndTargetDelegate"/></param>
        private void Target(object target) {
            // i added this method only for handling exceptions in thread
            try {
                if (target is CEndTargetDelegate trg)
                    trg();
            } catch (Exception ex) {
                runResult = ex;
                //logger.Critical($"Error in {ex.Source}: {ex.Message}\n{ex.StackTrace}");
            }
        }
        /// <summary>
        /// Prepares <see cref="CEndTargetDelegate"/> to run in thread
        /// </summary>
        /// <param name="target">Target method</param>
        /// <param name="name">Thread name</param>
        /// <returns>Prepared thread</returns>
        private Thread PrepareThread(CEndTargetDelegate target, string name) {
            var thr = new Thread(Target);
            thr.Name = name;
            thr.Start(target);
            return thr;
        }
        /// <summary>
        /// Run method in <see cref="CEnd"/>
        /// </summary>
        /// <param name="target">Target method</param>
        public void Run(CEndTargetDelegate target) {
            var name = "DESCEnd-" + Guid.NewGuid();
            var thr = PrepareThread(target, name);
            Run(thr, target);
        }
        /// <summary>
        /// Run thread in <see cref="CEnd"/>
        /// </summary>
        /// <param name="targetThread">Target thread</param>
        private void Run(Thread targetThread, CEndTargetDelegate targetMethod, int fails = 0) {
            if (!targetThread.Name.StartsWith("DESCEnd-")) targetThread.Name = "DESCEnd-" + Guid.NewGuid() + "-CN-" + targetThread.Name.Replace(" ", "-");
            try { if (!targetThread.IsAlive) targetThread.Start(); } catch (ThreadStateException) { targetThread = PrepareThread(targetMethod, targetThread.Name); }
            Logger.Info($"Thread {targetThread.Name} started");
            targetThread.Join();
            if (runResult != null) {
                Logger.Error($"Thread {targetThread.Name} failed (from method {runResult.TargetSite}, caused by {runResult.Source}). Exception: {runResult.GetType()}: {runResult.Message}\nStack trace: \t{runResult.StackTrace}");
                if (fails < 6) {
                    Logger.Warn($"Restarting thread {targetThread.Name}");
                    Thread.Sleep(1000);
                    Run(targetThread, targetMethod, fails + 1);
                } else { Logger.Critical($"Maximum restart attempts retrieved for {targetThread.Name}. Aborting."); }
            };
        }
    }
}
