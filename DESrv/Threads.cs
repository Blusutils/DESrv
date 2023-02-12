using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Blusutils.DESrv {
    public sealed class ManagedThread {
        public Action? Target { get; init; }
        public ManagedThread? ParentThread { get; set; }
        public List<ManagedThread>? ChildThreads { get; set; }
        public string? Name { get; set; }
        public string? ID { get; set; }
        public uint MaxRetryAttempts { get; set; } = 3;
        public bool RetryOnException { get; set; } = true;
        public bool EnsureAsyncRun { get; set; } = false;
        public bool Queued { get; private set; } = false;
        public void RequestRun() {

        }
        public void Release() {

        }
    }
    public sealed class Threader {
        public uint MaxThreads { get; set; } = uint.MaxValue;
        public List<ManagedThread> Threads { get; private set; } = new();
        public void QueueSingletonThread(ManagedThread thr) {
            if (Threads.Count + 1 > MaxThreads) throw new SemaphoreFullException("maximum count of managed threads reached");
                Threads.Add(thr);
        }
        public void QueueSingletonThread(Action trg, string? name = null, uint maxRetryAttempts = 3) {
            QueueSingletonThread(new ManagedThread { Target = trg, Name = name, MaxRetryAttempts = maxRetryAttempts, RetryOnException = true });
        }
    }
}
