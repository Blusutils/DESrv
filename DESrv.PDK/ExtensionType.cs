﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blusutils.DESrv.PDK {
    public enum ExtensionType {
        Plugin,
        Addon,
        SharedLibrary // TODO use custom ReferencePath to load it!
    }
}
