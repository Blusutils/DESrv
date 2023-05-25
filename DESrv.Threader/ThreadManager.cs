using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Blusutils.DESrv.Threader;

/// <summary>
/// <see cref="ManagedThread"/> manager
/// </summary>
public sealed class ThreadManager {

    /// <summary>
    /// Maximum amount of threads running at once
    /// </summary>
    public uint MaxThreads { get; set; } = uint.MaxValue;

    /// <summary>
    /// List of all threads. TEMPORARY SOLUTION FOR SHORT TIME OF DEVELOPMENT
    /// </summary>
    public List<ManagedThread> Threads { get; private set; } = new();

    /// <summary>
    /// Try to queue a singleton thread
    /// </summary>
    /// <param name="thr">Target thread</param>
    /// <exception cref="SemaphoreFullException">If <see cref="MaxThreads"/> amound is reached</exception>
    public void QueueSingletonThread(ManagedThread thr) {
        if (Threads.Count + 1 > MaxThreads) throw new SemaphoreFullException("maximum count of managed threads reached");
            Threads.Add(thr);
    }

    /// <summary>
    /// Try to create and queue a singleton thread
    /// </summary>
    /// <param name="trg">Target delegate to run</param>
    /// <param name="name">Thread name</param>
    /// <param name="maxRetryAttempts">Max retries on exceptions</param>
    /// <exception cref="SemaphoreFullException">If <see cref="MaxThreads"/> amound is reached</exception>
    public void QueueSingletonThread(Action trg, string? name = null, uint maxRetryAttempts = 3) {
        QueueSingletonThread(new ManagedThread { Target = trg, Name = name, MaxRetryAttempts = maxRetryAttempts, RetryOnException = maxRetryAttempts != 0 });
    }
}
