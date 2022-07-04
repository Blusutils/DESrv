using System;
using System.Net.Sockets;

namespace DESCore.DESConnections {
    /// <summary>
    /// Event listeners provider for <see cref="DESCoreRunner"/>
    /// </summary>
    class DESTCPReciveEvent : IDisposable {
        /// <summary>
        /// Instance of itself
        /// </summary>
        public static DESTCPReciveEvent Instance { get; private set; }
        /// <summary>
        /// List of events
        /// </summary>
        private List<Func<TcpClient, byte[], bool>> Funcs { get; set; } = new List<Func<TcpClient, byte[], bool>>();
        /// <summary>
        /// hahahaha  private constructor
        /// </summary>
        private DESTCPReciveEvent () {
            //Instance = new DESTCPReciveEvent();
        }
        /// <summary>
        /// Callbacks accessor
        /// </summary>
        public event Func<TcpClient, byte[], bool> Callbacks {
            add {
                DESCoreRunner.CEndLog.Debug($"new listener added: {value.Method.Name}", source: "DESrv TCP Events");
                Funcs.Add(value);
            }
            remove {
                Funcs.Remove(value);
            }
        }
        /// <summary>
        /// Calls all functions in <see cref="Funcs"/>
        /// </summary>
        /// <returns>Number of sucess calls</returns>
        public uint CallAll(TcpClient arg1, byte[] arg2) {
            uint successCalls = 0;
            foreach (var func in Funcs)
                try { _=func.Invoke(arg1, arg2); successCalls += 1; } catch {  }
            return successCalls;
        }
        /// <summary>
        /// Create an instance of <see cref="DESTCPReciveEvent"/>
        /// </summary>
        public static void CreateInstance() {
            Instance = new DESTCPReciveEvent();
        }
        /// <summary>
        /// Dispose object
        /// </summary>
        public void Dispose() {
            Funcs.Clear();
        }
        ~DESTCPReciveEvent() {
            Dispose();
        }
    }
    /// <summary>
    /// Main TCP sockets processor
    /// </summary>
    class DESTCPProcessor : DESBaseTCPProcessor {
        /// <summary>
        /// Create new instance of <see cref="DESTCPProcessor"/>
        /// </summary>
        /// <param name="cfg">Configuration (dictioanry)</param>
        public DESTCPProcessor(Dictionary<string, string> cfg) : base(cfg) { }
        /// <summary>
        /// New connection handler
        /// </summary>
        /// <exception cref="Exception">Just for test...</exception>
        public override void Runner() {
            throw new Exception("1");
            while (true) { 
                var client = AcceptConnection(); 
                Log.Success("Accepted TCP connection", "DESrv TCP Processor"); 
                var thr = new Thread(() => { Process(client); }); 
                thr.Name = $"DESrvTCPProcessor-{client.Client.Handle}-{client.Client.RemoteEndPoint}"; 
                thr.Start(); 
            }
        }
        /// <summary>
        /// Process new connection
        /// </summary>
        /// <param name="client">TCP socket client object</param>
        private void Process(TcpClient client) {
            try {
                var stream = client.GetStream();
                while (true) {
                    if (!client.Connected) {
                        Log.Info("Connection closed", "DESrv TCP Processor");
                        break;
                    }
                    while (!stream.DataAvailable) ; // do nothing and wait

                    byte[] bytes = new byte[client.Available];

                    stream.Read(bytes, 0, bytes.Length);

                    DESTCPReciveEvent.Instance.CallAll(client, bytes);
                    /*string recv = Encoding.UTF8.GetString(bytes);
                    Log.Debug(recv);*/
                }
            } catch (Exception ex) { Log.Error($"Error in {ex.Source}. Exception: {ex.Message}\nStack trace: \t{ex.StackTrace}"); }
        }
    }
}
