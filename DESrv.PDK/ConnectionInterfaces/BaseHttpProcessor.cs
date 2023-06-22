using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;

namespace Blusutils.DESrv.PDK.ConnectionInterfaces;
/// <summary>
/// A more high-level Hyper Text Transfer Protocol processor. It is recommended to use this class for your own, more feature-rich implementations.
/// </summary>
public class BaseHttpProcessor : IConnectionProcessor, IDisposable {

    public delegate void NewHttpDataDelegate(HttpListenerContext sender);
    public event NewHttpDataDelegate NewClientEvent;

    protected HttpListener listener;
    protected bool isRunning;

    public BaseHttpProcessor(string ip, int port/*, bool isSecure*/) : this(IPAddress.Parse(ip), port) { }
    public BaseHttpProcessor(IPAddress ip, int port) {
        listener = new HttpListener();
        listener.Prefixes.Add($"http://{ip}:{port}");
    }

    /// <inheritdoc/>
    public virtual void Run() {
        isRunning = true;
        Listen();
    }

    /// <inheritdoc/>
    public virtual async Task Accept() {
        if (listener != null) await ProcessAsync(await listener.GetContextAsync());
    }

    /// <inheritdoc/>
    protected virtual async Task ProcessAsync(HttpListenerContext ctx) {
        NewClientEvent?.Invoke(ctx);
    }

    /// <inheritdoc/>
    public virtual void Listen() {
        Task.Run(() => {
            listener.Start();
            while (isRunning) {
                Accept();
            }
        });
    }

    /// <inheritdoc/>
    public virtual void Close() {
        isRunning = false;
        listener?.Stop();
    }

    /// <inheritdoc/>
    public virtual void Dispose() {
        Close();
        listener = null;
        GC.SuppressFinalize(this);
    }
}