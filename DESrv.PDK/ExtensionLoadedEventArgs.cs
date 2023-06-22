using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blusutils.DESrv.PDK;

/// <summary>
/// Class representing event args for `ExtensionOnLoad` events
/// </summary>
public class ExtensionLoadedEventArgs : EventArgs {

    public ExtensionContainer Extension { get; init; }
    public ExtensionLoadedEventArgs(ExtensionContainer extension) => Extension = extension;
}
