using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Blusutils.DESrv;

/// <summary>
/// Highly managed thread
/// </summary>
public sealed class ManagedThread { // TODO implement managed threads

    /// <summary>
    /// Target delegate to run
    /// </summary>
    public Action? Target { get; init; }

    /// <summary>
    /// Parent thread
    /// </summary>
    public ManagedThread? ParentThread { get; set; }

    /// <summary>
    /// List of child threads
    /// </summary>
    public List<ManagedThread>? ChildThreads { get; set; }

    /// <summary>
    /// Thread name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Unique thread ID
    /// </summary>
    public string? ID { get; set; }

    /// <summary>
    /// Max retries on exceptions
    /// </summary>
    public uint MaxRetryAttempts { get; set; } = 3;

    /// <summary>
    /// Is repetition on exceptions enabled
    /// </summary>
    public bool RetryOnException { get; set; } = true;

    /// <summary>
    /// Is delegate will be runned asynchronously
    /// </summary>
    public bool EnsureAsyncRun { get; set; } = false;

    /// <summary>
    /// Is thread queued to run
    /// </summary>
    public bool Queued { get; private set; } = false;

    /// <summary>
    /// Request thread run
    /// </summary>
    public void RequestRun() {

    }

    /// <summary>
    /// Free the thread
    /// </summary>
    public void Release() {

    }

    /// <summary>
    /// Request thread stop
    /// </summary>
    public void RequestStop() {

    }
}

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
        QueueSingletonThread(new ManagedThread { Target = trg, Name = name, MaxRetryAttempts = maxRetryAttempts, RetryOnException = true });
    }
}
