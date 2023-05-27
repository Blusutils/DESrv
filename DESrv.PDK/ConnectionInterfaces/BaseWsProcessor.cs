using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;

namespace Blusutils.DESrv.PDK.ConnectionInterfaces;
/// <summary>
/// A more high-level Web Socket protocol processor. It is recommended to use this class for your own, more feature-rich implementations.
/// </summary>
public class BaseWsProcessor : IConnectionProcessor, IDisposable {

    public delegate void NewWsDataDelegate(WebSocket sender, byte[] data);
    public delegate void NewWsConnectionDelegate(WebSocket sender);
    public event NewWsDataDelegate NewDataEvent;
    public event NewWsConnectionDelegate NewClientEvent;

    protected CancellationTokenSource cancellationTokenSource;

    protected HttpListener listener;
    protected bool isRunning;

    public BaseWsProcessor(string ip, int port/*, bool isSecure*/) : this(IPAddress.Parse(ip), port) { }
    public BaseWsProcessor(IPAddress ip, int port) {
        listener = new HttpListener();
        listener.Prefixes.Add($"http://{ip}:{port}");
        listener.Prefixes.Add($"ws://{ip}:{port}");
        cancellationTokenSource = new CancellationTokenSource();
    }

    /// <inheritdoc/>
    public virtual void Run() {
        isRunning = true;
        Listen();
    }

    /// <inheritdoc/>
    public virtual async Task Accept() {
        if (listener != null) {
            var context = await listener.GetContextAsync();
            if (context.Request.IsWebSocketRequest) {
                var webSocketContext = await context.AcceptWebSocketAsync(null);
                NewClientEvent?.Invoke(webSocketContext.WebSocket);
                await ProcessAsync(webSocketContext.WebSocket);
            } else {
                context.Response.StatusCode = 400;
                context.Response.Close();
            }
        }
    }

    /// <inheritdoc/>
    protected virtual async Task ProcessAsync(WebSocket client) {
        try {
            var buffer = new Memory<byte>();

            while (client.State == WebSocketState.Open) {
                var result = await client.ReceiveAsync(buffer, cancellationTokenSource.Token);

                if (result.MessageType is WebSocketMessageType.Binary or WebSocketMessageType.Text) {
                    NewDataEvent?.Invoke(client, buffer.ToArray());
                } else if (result.MessageType == WebSocketMessageType.Close) {
                    await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "WebSocket connection closed by the server.", cancellationTokenSource.Token);
                }
            }
        } catch (OperationCanceledException) {
            // Ignore cancellation request
        } finally {
            client.Dispose();
        }
        
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
        cancellationTokenSource?.Cancel();
        listener?.Stop();
        cancellationTokenSource?.Dispose();
    }

    /// <inheritdoc/>
    public virtual void Dispose() {
        Close();
        listener = null;
        GC.SuppressFinalize(this);
    }
}