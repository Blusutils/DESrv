using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;


// TODO!
namespace DESrv.PDK.Connections {
    [ComVisible(true)]
    public class BaseWebSocketProcessor : IConnectionProcessor<TcpClient>, IDisposable {
        IPAddress? ip;
        int port;
        TcpListener? socket;
        public BaseWebSocketProcessor(string ip = "", int port = 0) {
            this.ip = IPAddress.Parse(ip);
            this.port = port;
            socket = new TcpListener(this.ip, this.port);
            socket.Start();
        }

        public virtual void Runner() {
            while (true) {
                var client = AcceptConnection();
                //Log.Success("Accepted TCP connection", "DESrv TCP Processor");
                new Thread(() => { Process(client); }) {
                    Name = $"DESrv-PDK-TCPProcessor-{client.Client.Handle}-{client.Client.RemoteEndPoint}"
                }.Start();
            }
        }

        protected virtual TcpClient AcceptConnection() {
            return socket.AcceptTcpClient();
        }

        public void Listen() => throw new NotImplementedException();

        public virtual void Process(TcpClient client) {
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

        TcpClient IConnectionProcessor<TcpClient>.AcceptConnection() {
            return AcceptConnection();
        }
        public void Close() {
            socket.Stop();
            socket = null;
        }
        public void Dispose() {
            Close();
            ip = null;
            port = 0;
        }

        
    }
}
