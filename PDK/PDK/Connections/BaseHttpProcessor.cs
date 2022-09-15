using System.Net;

namespace PDK.Connections {
    public class BaseHttpProcessor : IConnectionProcessor<HttpListenerContext>, IDisposable {
        IPAddress ip;
        int port;
        HttpListener httpServer;
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
                //Log.Success("Accepted TCP connection", "DESrv TCP Processor");
                var thr = new Thread(() => { Process(client); });
                thr.Name = $"DESrv-PDK-TCPProcessor-{client}";
                thr.Start();
            }
        }

        protected virtual HttpListenerContext AcceptConnection() {
            return httpServer.GetContext();
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