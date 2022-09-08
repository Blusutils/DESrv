using DESCEnd.Logging;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace DESCEnd {
    /// <summary>
    /// Represents the method that executes on <see cref="CEnd.Run(CEndTarget)"/>
    /// </summary>
    public delegate void CEndTarget();
    /// <summary>
    /// DESrv CEnd (ControlledEnd) class
    /// </summary>
    public class CEnd {
        /// <summary>
        /// Logger
        /// </summary>
        public Logging.CEndLog logger;
        /// <summary>
        /// Exit handler
        /// </summary>
        public Exceptor exitHandler;
        /// <summary>
        /// For <see cref="Target(object)"/>
        /// </summary>
        private Exception? runResult = null;
        /// <summary>
        /// Create a new instance of <see cref="CEnd"/>
        /// </summary>
        /// <param name="log"><see cref="CEndLog"/> logger</param>
        /// <param name="exitHandler"><see cref="Exceptor"/> exit handler</param>
        public CEnd(CEndLog log, Exceptor exitHandler) {
            logger = log;
            this.exitHandler = exitHandler;
        }
        /// <summary>
        /// Target method runner
        /// </summary>
        /// <param name="target">Target method as <see cref="CEndTarget"/></param>
        private void Target(object target) {
            // i added this method only for handling exceptions in thread
            try {
                if (target is CEndTarget trg)
                trg();
            } catch (Exception ex) {
                runResult = ex;
                //logger.Critical($"Error in {ex.Source}: {ex.Message}\n{ex.StackTrace}");
            }
        }
        private Thread PrepareThread(CEndTarget target, string name) {
            var thr = new Thread(Target);
            thr.Name = name;
            thr.Start(target);
            return thr;
        }
        /// <summary>
        /// Run method in <see cref="CEnd"/>
        /// </summary>
        /// <param name="target">Target method</param>
        public void Run(CEndTarget target) {
            var name = "DESCEnd-" + Guid.NewGuid();
            var thr = PrepareThread(target, name);
            Run(thr, target);
        }
        /// <summary>
        /// Run thread in CEnd
        /// </summary>
        /// <param name="targetThread">Target thread</param>
        public void Run(Thread targetThread, CEndTarget targetMethod, int fails = 0) {
            if (!targetThread.Name.StartsWith("DESCEnd-")) targetThread.Name = "DESCEnd-" + Guid.NewGuid() + "-CN-"+targetThread.Name.Replace(" ", "-");
            try { if (!targetThread.IsAlive) targetThread.Start(); } catch (ThreadStateException) { targetThread = PrepareThread(targetMethod, targetThread.Name); }
            logger.Info($"Thread {targetThread.Name} started");
            targetThread.Join();
            if (runResult != null) {
                logger.Error($"Thread {targetThread.Name} failed (from method {runResult.TargetSite}, caused by {runResult.Source}). Exception: {runResult.GetType()}: {runResult.Message}\nStack trace: \t{runResult.StackTrace}");
                if (fails < 6) {
                    logger.Warn($"Restarting thread {targetThread.Name}");
                    Thread.Sleep(1000);
                    Run(targetThread, targetMethod, fails + 1);
                } else { logger.Critical($"Maximum restart attempts retrieved for {targetThread.Name}. Aborting."); }
            };
        }
    }
}
