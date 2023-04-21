using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blusutils.DESrv.Logging;
public static class ConsoleService {
    public static IConsoleStream Console { get; private set; } = System.Console.In is StreamReader ? new PlainConsole() : new SimultaneousConsole();
}
