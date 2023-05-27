using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Blusutils.DESrv.PDK.ConnectionInterfaces;

/// <summary>
/// A more high-level User Datagram Protocol processor. It is recommended to use this class for your own, more feature-rich implementations.
/// </summary>
public class BaseUdpProcessor : IConnectionProcessor, IDisposable {

    public delegate void NewUdpDataDelegate(UdpReceiveResult sender);
    public event NewUdpDataDelegate NewDataEvent;

    protected UdpClient listener;
    protected bool isRunning;

    public BaseUdpProcessor(string ip, int port) : this(new(IPAddress.Parse(ip), port)) { }
    public BaseUdpProcessor(IPAddress ip, int port) : this(new(ip, port)) { }
    public BaseUdpProcessor(IPEndPoint endp) {
        listener = new UdpClient(endp);
    }

    /// <inheritdoc/>
    public virtual void Run() {
        isRunning = true;
        Accept();
    }

    /// <inheritdoc/>
    public virtual async Task Accept() {
        if (listener != null) {
            var task = Task.Run(async () => {
                while (isRunning) {
                    var result = await listener.ReceiveAsync();
                    NewDataEvent?.Invoke(result);
                }
            });
            await task;
        }
    }

    /// <inheritdoc/>
    public virtual void Listen() {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public virtual void Close() {
        isRunning = false;
        listener?.Close();
    }

    /// <inheritdoc/>
    public virtual void Dispose() {
        Close();
        listener = null;
        GC.SuppressFinalize(this);
    }
}