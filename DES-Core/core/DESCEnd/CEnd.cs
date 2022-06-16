using DESCore.DESCEnd.Logging;
using System;

namespace DESCore.DESCEnd {
    class CEnd {
        public Logging.CEndLog logger;
        public ExceptionPlus exceptionHandler;
        private Thread thr;

        public CEnd(CEndLog logger, ExceptionPlus exceptionHandler) {
            this.logger = logger;
            this.exceptionHandler = exceptionHandler;
        }
        public void Run(ThreadStart target) {
            try {
                thr = new Thread(target);
                thr.Name = "DESCend-"+Guid.NewGuid();
                thr.Start();
                logger.Info($"Thread {thr.Name} started");
                thr.Join();
            } catch (Exception ex) {
                logger.Critical($"Thread {thr.Name} failed. Exception:\n{ex.StackTrace}");
            }
         }
    }
}
