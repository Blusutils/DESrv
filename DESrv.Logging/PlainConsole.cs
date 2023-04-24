using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blusutils.DESrv.Logging;

/// <summary>
/// Plain console stream implementation
/// </summary>
public class PlainConsole : IConsoleStream {

    /// <summary>
    /// Read next line of text from console
    /// </summary>
    /// <returns>Readed string</returns>
    public string ReadLine() {
        return Console.ReadLine()!;
    }

    /// <summary>
    /// Read next line of text from console with prompt characters
    /// </summary>
    /// <param name="prompt">Text with which to start input</param>
    /// <param name="inputText">Last read characters</param>
    /// <returns>Readed string</returns>
    public string ReadLine(string prompt, string inputText = "") {
        if (prompt != null) Console.Write(prompt);
        return Console.ReadLine()!;
    }

    /// <summary>
    /// Write some text to console
    /// </summary>
    /// <param name="text">Text to write</param>
    /// <param name="fgColor">Foreground console color</param>
    /// <param name="bgColor">Background console color</param>
    public void Write(string text, ConsoleColor fgColor = ConsoleColor.White, ConsoleColor bgColor = ConsoleColor.Black) {
        var fg = Console.ForegroundColor;
        var bg = Console.BackgroundColor;
        Console.BackgroundColor = bgColor;
        Console.ForegroundColor = fgColor;
        Console.Write(text);
        Console.ForegroundColor = fg;
        Console.BackgroundColor = bg;
    }

    /// <summary>
    /// Write some text to console and add newline character
    /// </summary>
    /// <param name="text">Text to write</param>
    /// <param name="fgColor">Foreground console color</param>
    /// <param name="bgColor">Background console color</param>
    public void WriteLine(string text, ConsoleColor fgColor = ConsoleColor.White, ConsoleColor bgColor = ConsoleColor.Black) {
        Write(text + "\n", fgColor, bgColor);
    }
}
