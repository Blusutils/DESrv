using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blusutils.DESrv.PDK {
    /// <summary>
    /// Types of PDK extensions
    /// </summary>
    public enum ExtensionType {
        /// <summary>
        /// And plugin extension - the most common extension
        /// </summary>
        Plugin,
        /// <summary>
        /// An addon - an extension that can be loaded into a plugin to extend its capabilities
        /// </summary>
        Addon
    }
}
