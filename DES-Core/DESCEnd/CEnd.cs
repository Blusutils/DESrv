using DESCEnd.Logging;
using System;

namespace DESCEnd {
    /// <summary>
    /// Represents the method that executes on <see cref="CEnd.Run(CEndTarget)"/>
    /// </summary>
    public delegate void CEndTarget();
    /// <summary>
    /// DESrv ControlledEnd class
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
        /// CEnd thread
        /// </summary>
        private Thread? thr;
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
        /// <summary>
        /// Run method in <see cref="CEnd"/>
        /// </summary>
        /// <param name="target">Target method</param>
        public void Run(CEndTarget target) {
            thr = new Thread(Target);
            thr.Name = "DESCend-"+Guid.NewGuid();
            thr.Start(target);
            logger.Info($"Thread {thr.Name} started");
            thr.Join();
            if (runResult != null) logger.Critical($"Thread {thr.Name} (in {runResult.Source}) failed. Exception: {runResult.Message}\nStack trace: \t{runResult.StackTrace}");
        }
    }
}
