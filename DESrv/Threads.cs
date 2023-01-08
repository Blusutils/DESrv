using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blusutils.DESrv {
    public sealed class ManagedThread {
        public ManagedThread? ParentThread { get; set; }
        public List<ManagedThread>? ChildThreads = null;
        public string? Name { get; set; }
        public string? ID { get; set; }
        public uint MaxRetryAttempts { get; set; } = 3;
        public bool RetryOnException { get; set; } = true;
        public bool EnsureAsyncRun { get; set; } = false;
        public bool Queued { get; private set; } = false;
    }
    public sealed class Threader {
        public uint MaxThreads { get; set; } = uint.MaxValue;
        public List<ManagedThread> Threads { get; private set; } = new();
        public bool QueueSingletonThread(ManagedThread thr) {
            try {
                Threads.Add(thr);
                return true;
            } catch { return false; }
        }
        public bool QueueSingletonThread(Action trg, string? name = null, uint maxRetryAttempts = 3) {
            return QueueSingletonThread(new ManagedThread());
        }
    }
}
