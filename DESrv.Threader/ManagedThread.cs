using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blusutils.DESrv.Threader;

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