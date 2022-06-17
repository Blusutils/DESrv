using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DESCore.DESConnections {
    /// <summary>
    /// Interface for connection processors
    /// </summary>
    interface IConnectionProcessor {
        /// <summary>
        /// New connection handler
        /// </summary>
        public void Runner();
    }
}
