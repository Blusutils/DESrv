﻿using System.Runtime.InteropServices;

namespace DESrv.PDK.Connections {
    /// <summary>
    /// Interface for connection processors
    /// </summary>
    // <typeparam name="T1">A socket type</typeparam>
    /// <typeparam name="T">A client type</typeparam>
    [ComVisible(true)]
    public interface IConnectionProcessor<T> {
        /// <summary>
        /// New connection handler
        /// </summary>
        public void Runner();
        protected T AcceptConnection();
        public void Process(T client);
        public void Listen();
        public void Close();
    }
}