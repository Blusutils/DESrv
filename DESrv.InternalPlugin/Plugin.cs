using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blusutils.DESrv.PDK;

namespace Blusutils.DESrv.InternalPlugin;

[PDKExtension(
    "DESrvInternal",
    ExtensionType.Plugin,
    "DESrv Internal Plugin",
    "Blusutils",
    "1.0.0",
    "2.0.0",
    "Internal plugin for DESrv. Contains some basic utils and commands.",
    "https://github.com/Blusutils/DESrv/tree/master/DESrv.InternalPlugin"
    )]
public class Plugin {
}
