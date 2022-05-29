using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DESCore.DESConnections
{
    class DESWebSocketsProcessor : DESBaseTCPProcessor
    {
        public DESWebSocketsProcessor(Dictionary<string, string> cfg) : base(cfg) { }
        public override void Runner()
        {
            var client = AcceptConnection();
            var stream = client.GetStream();
            // TODO: work with ping (heathbeat) 
            while (true)
            {
                while (!stream.DataAvailable) ;

                Byte[] bytes = new Byte[client.Available];

                stream.Read(bytes, 0, bytes.Length);
                string recv = Encoding.UTF8.GetString(bytes);
                if (Regex.IsMatch(recv, "^GET"))
                {
                    Log.Success("Accepting connection from Websocket");
                    // handshake
                    Byte[] response = Encoding.UTF8.GetBytes("HTTP/1.1 101 Switching Protocols" + EOL
                        + "Connection: Upgrade" + EOL
                        + "Upgrade: websocket" + EOL
                        + "Sec-WebSocket-Accept: " + Convert.ToBase64String(
                            System.Security.Cryptography.SHA1.Create().ComputeHash(
                                Encoding.UTF8.GetBytes(
                                    new System.Text.RegularExpressions.Regex("Sec-WebSocket-Key: (.*)").Match(recv).Groups[1].Value.Trim() + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"
                                )
                            )
                        ) + EOL
                        + EOL);
                    stream.Write(response, 0, response.Length);
                }
                else
                {
                    bool fin = (bytes[0] & 0b10000000) != 0;
                    bool mask = (bytes[1] & 0b10000000) != 0;
                    int opcode = bytes[0] & 0b00001111;
                    ulong offset = 2;
                    ulong msglen = (ulong)(bytes[1] & 0b01111111);

                    if (msglen == 126)
                    {
                        msglen = BitConverter.ToUInt16(new byte[] { bytes[3], bytes[2] }, 0);
                        offset = 4;
                    }
                    else if (msglen == 127)
                    {
                        msglen = BitConverter.ToUInt64(new byte[] { bytes[9], bytes[8], bytes[7], bytes[6], bytes[5], bytes[4], bytes[3], bytes[2] });
                        offset = 10;
                    }

                    if (msglen == 0)
                        Log.Warn("msglen == 0");
                    else if (mask)
                    {
                        byte[] decoded = new byte[msglen];
                        byte[] masks = new byte[4] { bytes[offset], bytes[offset + 1], bytes[offset + 2], bytes[offset + 3] };
                        offset += 4;

                        for (ulong i = 0; i < msglen; ++i)
                            decoded[i] = (byte)(bytes[offset + i] ^ masks[i % 4]);

                        string text = Encoding.UTF8.GetString(decoded);
                        Log.Debug($"Recived: {text}");
                    }
                    else
                        Log.Warn("mask bit not set");

                    Console.WriteLine();
                }
            }
        }
    }
}
