using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Blusutils.DESrv.PDK.ConnectionInterfaces;
/// <summary>
/// A more high-level Transmission Control Protocol processor. It is recommended to use this class for your own, more feature-rich implementations.
/// </summary>
public class BaseTcpProcessor : IConnectionProcessor, IDisposable {

    public delegate void NewTcpDataDelegate(TcpClient sender, byte[] data);
    public delegate void NewTcpConnectionDelegate(TcpClient sender);
    public event NewTcpDataDelegate NewDataEvent;
    public event NewTcpConnectionDelegate NewClientEvent;

    protected TcpListener listener;
    protected bool isRunning;

    public BaseTcpProcessor(string ip, int port) : this(IPAddress.Parse(ip), port) { }
    public BaseTcpProcessor(IPAddress ip, int port) {
        listener = new TcpListener(ip, port);
    }

    /// <inheritdoc/>
    public virtual void Run() {
        isRunning = true;
        Listen();
    }

    /// <inheritdoc/>
    public virtual async Task Accept() {
        if (listener != null && listener.Pending()) {
            var client = await listener.AcceptTcpClientAsync();
            NewClientEvent?.Invoke(client);
            var proc = ProcessAsync(client);
            await proc;
        }
    }

    /// <inheritdoc/>
    protected virtual async Task ProcessAsync(TcpClient client) {
        var stream = client.GetStream();
        var buffer = new Memory<byte>();
        await stream.ReadAsync(buffer);
        NewDataEvent?.Invoke(client, buffer.ToArray());
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
