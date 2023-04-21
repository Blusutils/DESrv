using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blusutils.DESrv.Logging;
internal class PlainConsole : IConsoleStream {
    public string ReadLine() {
        return Console.ReadLine()!;
    }
    public string ReadLine(string prompt, string inputText = "") {
        if (prompt != null) Console.Write(prompt);
        return Console.ReadLine()!;
    }
    public void Write(string text, ConsoleColor fgColor = ConsoleColor.White, ConsoleColor bgColor = ConsoleColor.Black) {
        var fg = Console.ForegroundColor;
        var bg = Console.BackgroundColor;
        Console.BackgroundColor = bgColor;
        Console.ForegroundColor = fgColor;
        Console.Write(text);
        Console.ForegroundColor = fg;
        Console.BackgroundColor = bg;
    }
    public void WriteLine(string text, ConsoleColor fgColor = ConsoleColor.White, ConsoleColor bgColor = ConsoleColor.Black) {
        Write(text + "\n", fgColor, bgColor);
    }
}
