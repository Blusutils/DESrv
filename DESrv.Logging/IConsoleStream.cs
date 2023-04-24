using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blusutils.DESrv.Logging;

/// <summary>
/// Interface for console streams
/// </summary>
public interface IConsoleStream {

    /// <summary>
    /// Write some text to console
    /// </summary>
    /// <param name="text">Text to write</param>
    /// <param name="fgColor">Foreground console color</param>
    /// <param name="bgColor">Background console color</param>
    void Write(string text, ConsoleColor fgColor = ConsoleColor.White, ConsoleColor bgColor = ConsoleColor.Black);

    /// <summary>
    /// Write some text to console and add newline character
    /// </summary>
    /// <param name="text">Text to write</param>
    /// <param name="fgColor">Foreground console color</param>
    /// <param name="bgColor">Background console color</param>
    void WriteLine(string text, ConsoleColor fgColor = ConsoleColor.White, ConsoleColor bgColor = ConsoleColor.Black);

    /// <summary>
    /// Read next line of text from console
    /// </summary>
    /// <returns>Readed string</returns>
    string ReadLine();

    /// <summary>
    /// Read next line of text from console with prompt characters
    /// </summary>
    /// <param name="prompt">Text with which to start input</param>
    /// <param name="inputText">Last read characters</param>
    /// <returns>Readed string</returns>
    string ReadLine(string prompt, string inputText = "");
}
