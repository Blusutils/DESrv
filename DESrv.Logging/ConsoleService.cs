using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blusutils.DESrv.Logging;

/// <summary>
/// Console streams service
/// </summary>
public static class ConsoleService {

    /// <summary>
    /// Current console stream instance
    /// </summary>
    public static IConsoleStream Console { get; private set; } = System.Console.In is StreamReader ? new PlainConsole() : new SimultaneousConsole();
}
