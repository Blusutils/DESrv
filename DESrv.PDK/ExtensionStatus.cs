using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blusutils.DESrv.PDK {
    /// <summary>
    /// Extension status codes
    /// </summary>
    public enum ExtensionStatus {
        /// <summary>
        /// No info about extension
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// The extension was found, but was not loaded in a session
        /// </summary>
        Found,
        /// <summary>
        /// The extension is detected as shared (is a library/used by another DESrv instance)
        /// </summary>
        Shared, // TODO use custom ReferencePath to load it!
        /// <summary>
        /// The extension was succefully loaded
        /// </summary>
        Loaded,
        /// <summary>
        /// The extension was succefully loaded as addon or child dependency
        /// </summary>
        LoadedAsChildren,
        /// <summary>
        /// The extension execution was paused
        /// </summary>
        Suspended,
        /// <summary>
        /// Extension loading failed
        /// </summary>
        Failed,
        /// <summary>
        /// Extension loading failed due to missing/error dependency
        /// </summary>
        FailedDependency,
        /// <summary>
        /// Extension loading failed due to missing/error child extension
        /// </summary>
        FailedChildren,
        /// <summary>
        /// Extension loading failed due to missing/error parent extension
        /// </summary>
        FailedParent
    }
}
