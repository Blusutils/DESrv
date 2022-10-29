using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace DESrv.PDK.Connections {
    [ComVisible(true)]
    public class BaseHttpProcessor : IConnectionProcessor<HttpListenerContext>, IDisposable {
        IPAddress ip;
        int port;
        HttpListener httpServer;
        List<HttpListenerContext> clients = new List<HttpListenerContext>();

        public delegate void NewClientConnectedDelegate(HttpListenerContext client);
        public event NewClientConnectedDelegate NewClientConnectedEvent;

        public delegate void ClientGotDataDelegate(HttpListenerContext client, string data, byte[] bytes);
        public event ClientGotDataDelegate ClientGotDataEvent;

        public BaseHttpProcessor(string ip = "", int port = 0) {
            this.ip = IPAddress.Parse(ip);
            this.port = port;
            httpServer = new HttpListener();
            httpServer.Prefixes.Add($"http://{this.ip}:/{port}");
            httpServer.Start();
        }

        public virtual void Runner() {
            while (true) {
                var client = AcceptConnection();
                //Log.Success("Accepted HTTP connection", "DESrv TCP Processor");
                
                var thr = new Thread(() => { Process(client); });
                thr.Name = $"DESrv-PDK-HTTPProcessor-{client}";
                thr.Start();
            }
        }

        protected virtual HttpListenerContext AcceptConnection() {
            var client = httpServer.GetContext();
            NewClientConnectedEvent?.Invoke(client);
            clients.Add(client);
            return client;
        }
        public void Listen() {
            while (true) {
                foreach (var c in clients) {
                    try {
                        HttpListenerRequest request = c.Request;
                        HttpListenerResponse response = c.Response;
                        response.StatusCode = (int)HttpStatusCode.NoContent;
                        response.ContentLength64 = 0;
                        response.OutputStream.Close();
                    } catch (Exception e) { Console.WriteLine(e.ToString()); continue; }
                }
            }
        }

        public virtual void Process(HttpListenerContext client) {
            //try {
            //    var stream = client.GetStream();
            //    while (true) {
            //        if (!client.Connected) {
            //            Log.Info("Connection closed", "DESrv TCP Processor");
            //            break;
            //        }
            //        while (!stream.DataAvailable) ; // do nothing and wait

            //        byte[] bytes = new byte[client.Available];

            //        stream.Read(bytes, 0, bytes.Length);

            //        DESTCPReciveEvent.Instance.CallAll(client, bytes);
            //        /*string recv = Encoding.UTF8.GetString(bytes);
            //        Log.Debug(recv);*/
            //    }
            //} catch (Exception ex) { }
            throw new NotImplementedException();
        }

        HttpListenerContext IConnectionProcessor<HttpListenerContext>.AcceptConnection() {
            return AcceptConnection();
        }
        public void Close() {
            httpServer.Stop();
            httpServer = null;
        }
        public void Dispose() {
            Close();
            ip = null;
            port = 0;
        }
    }
}