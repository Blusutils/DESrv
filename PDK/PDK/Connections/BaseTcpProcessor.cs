using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace PDK.Connections {
    [ComVisible(true)]
    public class BaseTcpProcessor : IConnectionProcessor<TcpClient>, IDisposable {
        IPAddress ip;
        int port;
        TcpListener socket;
        List<TcpClient> clients = new List<TcpClient>();

        public delegate void NewClientConnectedDelegate(TcpClient client);
        public event NewClientConnectedDelegate NewClientConnectedEvent;

        public delegate void ClientGotDataDelegate(TcpClient client, string data, byte[] bytes);
        public event ClientGotDataDelegate ClientGotDataEvent;

        public BaseTcpProcessor(string ip = "", int port = 0) {
            this.ip = IPAddress.Parse(ip);
            this.port = port;
            socket = new TcpListener(this.ip, port);
            socket.Start();
        }

        public virtual void Runner() {
            while (true) {
                var client = AcceptConnection();
                //Log.Success("Accepted TCP connection", "DESrv TCP Processor");
                var thr = new Thread(() => { Process(client); });
                thr.Name = $"DESrv-PDK-TCPProcessor-{client.Client.Handle}-{client.Client.RemoteEndPoint}";
                thr.Start();
            }
        }

        protected virtual TcpClient AcceptConnection() {
            var cl = socket.AcceptTcpClient();
            NewClientConnectedEvent?.Invoke(cl);
            clients.Add(cl);
            return cl;
        }

        public virtual void Listen() {
            while (true) {
                foreach (var c in clients) {
                    try {
                        var stream = c.GetStream();
                        if (!c.Connected) {
                            c.Close();
                            clients.Remove(c);
                            continue;
                        }
                        if (!stream.DataAvailable) continue;
                        byte[] bytes = new byte[c.Available];
                        stream.Read(bytes, 0, bytes.Length);
                        string recv = Encoding.UTF8.GetString(bytes);
                        ClientGotDataEvent?.Invoke(c, recv, bytes);
                    } catch (Exception e) { Console.WriteLine(e.ToString()); continue; }
                }
            }
        }

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
