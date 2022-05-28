﻿using DESCore.DESCEnd.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DESCore.DESCEnd
{
    class CEnd
    {
        public Logging.CEndLog logger;
        public ExceptionPlus exceptionHandler;
        private Thread thr;

        public CEnd(CEndLog logger, ExceptionPlus exceptionHandler)
        {
            this.logger = logger;
            this.exceptionHandler = exceptionHandler;
        }
        public void A() { }
        public void Run(ThreadStart target)
        {
            thr = new Thread(target);
            thr.Name = "DESCend-"+Guid.NewGuid();
            thr.Start();
            logger.Info($"Thread {thr.Name} started");
        }
    }
}