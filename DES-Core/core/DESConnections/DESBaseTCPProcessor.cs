using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DESCore.DESConnections {
    /// <summary>
    /// Base for TCP sockets processors
    /// </summary>
    class DESBaseTCPProcessor : IConnectionProcessor {
        /// <summary>
        /// Socket
        /// </summary>
        protected TcpListener socket;
        /// <summary>
        /// Logger
        /// </summary>
        protected DESCEnd.Logging.CEndLog Log;
        /// <summary>
        /// End of line (for WS)
        /// </summary>
        protected const string EOL = "\r\n";
        /// <summary>
        /// Create new instance of <see cref="DESBaseTCPProcessor"/>
        /// </summary>
        /// <param name="cfg">Configuration (dictioanry)</param>
        public DESBaseTCPProcessor(Dictionary<string, string> cfg) {
            Log = DESCoreRunner.CEndLog;
            string ip;
            ip = cfg.TryGetValue("ipaddress", out ip) ? ip : "127.0.0.1";
            string port;
            port = cfg.TryGetValue("ipaddress", out port) ? port : "9090";
            socket = new TcpListener(System.Net.IPAddress.Parse(ip), (int.Parse(port)));
        }
        /// <summary>
        /// Accept new connection
        /// </summary>
        /// <returns></returns>
        protected TcpClient AcceptConnection() {
            socket.Start();
            var client = socket.AcceptTcpClient();
            Log.Info($"New connection");
            return client;
        }
        /// <summary>
        /// New connection handler
        /// </summary>
        /// <exception cref="NotImplementedException">If method not overriden</exception>
        public virtual void Runner() {
            throw new NotImplementedException();
        }
    }
}
