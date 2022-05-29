using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DESCore.DESConnections
{
    class DESTCPProcessor : DESBaseTCPProcessor
    {
        public DESTCPProcessor(Dictionary<string, string> cfg) : base(cfg) { }

        public override void Runner()
        {
            var client = AcceptConnection();
            var stream = client.GetStream();
            Log.Success("Accepted TCP connection");
            while (true)
            {
                while (!stream.DataAvailable) ;

                Byte[] bytes = new Byte[client.Available];

                stream.Read(bytes, 0, bytes.Length);
                string recv = Encoding.UTF8.GetString(bytes);
                Log.Debug(recv);
            }
        }
    }
}
