using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blusutils.DESrv.Logging;
public interface IConsoleStream {
    void Write(string text, ConsoleColor fgColor = ConsoleColor.White, ConsoleColor bgColor = ConsoleColor.Black);

    void WriteLine(string text, ConsoleColor fgColor = ConsoleColor.White, ConsoleColor bgColor = ConsoleColor.Black);

    string ReadLine();

    string ReadLine(string prompt, string inputText = "");
}
