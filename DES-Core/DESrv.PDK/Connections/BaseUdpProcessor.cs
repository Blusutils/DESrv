using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace DESrv.PDK.Connections {
    [ComVisible(true)]
    public class BaseUdpProcessor : IConnectionProcessor<UdpClient>, IDisposable {
        IPAddress ip;
        int port;
        Socket socket;
        List<UdpClient> clients = new List<UdpClient>();

        public delegate void NewClientConnectedDelegate(UdpClient client);
        public event NewClientConnectedDelegate NewClientConnectedEvent;

        public delegate void ClientGotDataDelegate(UdpClient client, string data, byte[] bytes);
        public event ClientGotDataDelegate ClientGotDataEvent;

        public BaseUdpProcessor(string ip = "", int port = 0) {
            this.ip = IPAddress.Parse(ip);
            this.port = port;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(new IPEndPoint(this.ip, this.port));
            //socket.Start();
        }

        public virtual void Runner() {
            throw new NotImplementedException();
            //while (true) {
            //    var client = AcceptConnection();
            //    //Log.Success("Accepted TCP connection", "DESrv TCP Processor");
            //    var thr = new Thread(() => { Process(client); });
            //    thr.Name = $"DESrv-PDK-UDPProcessor-{client.Client.Handle}-{client.Client.RemoteEndPoint}";
            //    thr.Start();
            //}
        }
        public void Listen() => throw new NotImplementedException();
        protected virtual UdpClient AcceptConnection() {
            throw new NotImplementedException();
            //return socket.AcceptUdpClient();
        }

        public virtual void Process(UdpClient client) {
            //try {
            //    while (true) {
            //        StringBuilder builder = new StringBuilder();
            //        int bytes = 0;
            //        byte[] data = new byte[256];

            //        EndPoint remoteIp = new IPEndPoint(IPAddress.Any, 0);

            //        do {
            //            bytes = socket.ReceiveFrom(data, ref remoteIp);
            //            builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            //        } while (socket.Available > 0);
            //        IPEndPoint remoteFullIp = remoteIp as IPEndPoint;

            //        // выводим сообщение
            //        Console.WriteLine("{0}:{1} - {2}", remoteFullIp.Address.ToString(),
            //                                        remoteFullIp.Port, builder.ToString());
            //    }
            //} catch (Exception ex) {
            //    Console.WriteLine(ex.Message);
            //} finally {
            //    Close();
            //}
            throw new NotImplementedException();
        }

        UdpClient IConnectionProcessor<UdpClient>.AcceptConnection() {
            return AcceptConnection();
        }
        public void Close() {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            socket = null;
        }
        public void Dispose() {
            Close();
            ip = null;
            port = 0;
        }
    }
}
