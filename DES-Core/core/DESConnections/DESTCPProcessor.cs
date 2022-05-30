using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DESCore.DESConnections
{
    class DESTCPReciveEvent : IDisposable
    {
        public static DESTCPReciveEvent Instance { get; private set; }
        private List<Func<TcpClient, byte[], bool>> Funcs { get; set; } = new List<Func<TcpClient, byte[], bool>>();
        private DESTCPReciveEvent ()
        {
            //Instance = new DESTCPReciveEvent();
        }
        public event Func<TcpClient, byte[], bool> Callbacks
        {
            add
            {
                DESCoreRunner.CEndLog.Debug($"new listener added: {value.Method.Name}", source: "DES TCP Events");
                Funcs.Add(value);
            }
            remove
            {
                Funcs.Remove(value);
            }
        }
        /// <summary>
        /// Calls all functions in <see cref="Funcs"/>
        /// </summary>
        /// <returns>Number of sucess calls</returns>
        public uint CallAll(TcpClient arg1, byte[] arg2)
        {
            uint successCalls = 0;
            foreach (var func in Funcs)
                try { _=func.Invoke(arg1, arg2); successCalls += 1; } catch {  }
            return successCalls;
        }

        public static void CreateInstance()
        {
            Instance = new DESTCPReciveEvent();
        }

        public void Dispose()
        {
            Funcs.Clear();
        }
    }
    class DESTCPProcessor : DESBaseTCPProcessor
    {
        public DESTCPProcessor(Dictionary<string, string> cfg) : base(cfg) { }

        public override void Runner()
        {
            while (true) { var client = AcceptConnection(); Log.Success("Accepted TCP connection", "DES TCP Processor"); var thr = new Thread(() => { Process(client); }); thr.Name = $"TCPProcessor-{client.Client.Handle}-{client.Client.RemoteEndPoint}"; thr.Start(); }
        }
        private void Process(TcpClient client)
        {
            var stream = client.GetStream();
            while (true)
            {
                if (!client.Connected)
                {
                    Log.Info("Connection closed", "DES TCP Processor");
                    break;
                }
                while (!stream.DataAvailable) ;

                byte[] bytes = new byte[client.Available];

                stream.Read(bytes, 0, bytes.Length);

                DESTCPReciveEvent.Instance.CallAll(client, bytes);
                /*string recv = Encoding.UTF8.GetString(bytes);
                Log.Debug(recv);*/
            }
        }
    }
}
