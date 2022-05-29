using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DESCore.DESConnections
{
    class DESBaseTCPProcessor : IConnectionProcessor
    {
        protected TcpListener socket;
        protected DESCEnd.Logging.CEndLog Log;
        protected const string EOL = "\r\n";
        public DESBaseTCPProcessor(Dictionary<string, string> cfg)
        {
            Log = DESCoreRunner.CEndLog;
            string ip;
            ip = cfg.TryGetValue("ipaddress", out ip) ? ip : "127.0.0.1";
            string port;
            port = cfg.TryGetValue("ipaddress", out port) ? port : "9090";
            socket = new TcpListener(System.Net.IPAddress.Parse(ip), (int.Parse(port)));
        }
        protected TcpClient AcceptConnection()
        {
            socket.Start();
            var client = socket.AcceptTcpClient();
            Log.Info($"New connection");
            return client;
        }
        public virtual void Runner()
        {
            throw new NotImplementedException();
        }
    }
}
