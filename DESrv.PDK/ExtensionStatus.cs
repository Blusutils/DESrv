using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blusutils.DESrv.PDK {
    public enum ExtensionStatus {
        Unknown = 0,
        Found,
        Shared,
        Loaded,
        LoadedAsChildren,
        Suspended,
        Failed,
        FailedDependency,
        FailedChildren,
        FailedParent
    }
}
