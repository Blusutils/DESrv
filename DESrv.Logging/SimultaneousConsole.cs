/* 
MIT License

Copyright (c) 2022 Stefan Horst

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Blusutils.DESrv.Logging {
    public record class Message(string text, ConsoleColor fgColor = ConsoleColor.White, ConsoleColor bgColor = ConsoleColor.Black);
    public class OutputWriter {
        // queue is used in case text is added more than once between each cycle of SimulConsoleIO calling GetText
        private Queue<Message> outputTextQueue = new();
        // text which will be at beginning of all output
        private string startText;
        public string StartText { get => startText; set => startText = value; }
        public OutputWriter(string startText = "") {
            this.startText = startText;
        }
        public void AddText(string text, ConsoleColor fgColor = ConsoleColor.White, ConsoleColor bgColor = ConsoleColor.Black) {
            outputTextQueue.Enqueue(new (startText + text, fgColor, bgColor));
        }
        public Message GetText() {
            StringBuilder s = new StringBuilder();
            Message msg = null;
            while (outputTextQueue.Count > 0) {
                msg = outputTextQueue.Dequeue();
                s.Append(msg.text);
            }
            return new Message(s.ToString(), msg?.fgColor??ConsoleColor.White, msg?.bgColor ?? ConsoleColor.Black);
        }
    }
    public static class SimultaneousConsole {

        private static List<string> history = new();
        private static StringBuilder cmdInput = new();
        private static int cursorYInit;
        private static int cursorXTotal; // like cursorleft but does not reset at new lines
        private static int cursorXOffset; // length of prompt before input
        private static int index; // index of history list

        public static OutputWriter? OutputWriter { get; set; } = new();
        public static string PromptDefault { get; set; } = "> ";
        public static int SleepTime { get; set; } = 25;

        public static void Write(string text, ConsoleColor fgColor = ConsoleColor.White, ConsoleColor bgColor = ConsoleColor.Black) {
            OutputWriter?.AddText(text, fgColor, bgColor);
        }

        public static void WriteLine(string text, ConsoleColor fgColor = ConsoleColor.White, ConsoleColor bgColor = ConsoleColor.Black) {
            Write(text, fgColor, bgColor);
            Write(Environment.NewLine, fgColor, bgColor);
        }

        public static string ReadLine() {
            return ReadLine(PromptDefault);
        }

        public static string ReadLine(string prompt, string inputText = "") {
            cmdInput = new StringBuilder();

            cursorYInit = Console.CursorTop;
            cursorXTotal = 0;
            cursorXOffset = prompt.Length;
            index = -1;
            Console.Write(prompt);

            if (inputText != "") {
                Console.Write(inputText);
                cmdInput.Append(inputText);
                cursorXTotal = cmdInput.Length;
                SetCursorEndOfInput();
            }

            ConsoleKeyInfo cki = default;

            do // while (cki.Key != ConsoleKey.Enter)
            {
                if (Console.KeyAvailable) {
                    cki = Console.ReadKey(true);

                    HandleKey(cki);
                }

                PrintText(prompt); // write text to console "while" getting user input

                cursorYInit = Console.CursorTop - (cursorXOffset + cursorXTotal) / Console.BufferWidth; // changes value relative to changes to cursortop caused by cmd window resizing

                Thread.Sleep(SleepTime);
            }
            while (cki.Key != ConsoleKey.Enter);

            cursorYInit = Console.CursorTop - (cursorXOffset + cursorXTotal) / Console.BufferWidth; // changes value relative to changes to cursortop caused by cmd window resizing

            Console.WriteLine();

            Console.CursorTop = cursorYInit + (cursorXOffset + cmdInput.Length) / Console.BufferWidth; // set cursor y to next line after last (full) line of input
            if ((cursorXOffset + cmdInput.Length) % Console.BufferWidth > 0) // move cursor y one more down if there is one last not full line of input
                Console.CursorTop++;

            history.Add(cmdInput.ToString());

            return cmdInput.ToString();
        }

        // allows printing all all queued output; useful before stopping program etc.
        public static void ForcePrintQueue() {
            var msg = OutputWriter?.GetText();
            var fg = Console.ForegroundColor;
            var bg = Console.BackgroundColor;
            Console.BackgroundColor = msg.bgColor;
            Console.ForegroundColor = msg.fgColor;
            Console.Write(msg.text);
            Console.ForegroundColor = fg;
            Console.BackgroundColor = bg;
        }

        private static void HandleKey(ConsoleKeyInfo cki) {
            // ctrl key not pressed or alt key pressed (making ctrl+alt possible which equals altgr key), prevents shortcuts like ctrl+i, but allows ones like altgr+q for @
            if (cki.Key == ConsoleKey.Enter || ((cki.Modifiers & ConsoleModifiers.Control) != 0 &&
                                                (cki.Modifiers & ConsoleModifiers.Alt) == 0))
                return;

            switch (cki.Key) {
                case ConsoleKey.Backspace:
                    Console.Write(cki.KeyChar);

                    Console.CursorLeft++; // counteracts console standard behaviour of moving cursor one to the left

                    // nested if so that backspace does not get added to cmdinput in else-part of statement
                    if (Console.CursorLeft > cursorXOffset || Console.CursorTop > cursorYInit) {
                        bool lineFlag = (cursorXOffset + cursorXTotal) % Console.BufferWidth == 0; // signals when cursor is at first pos of line

                        // only go to line above when current line is empty (otherwise would already happen when first char in line is deleted, because cursor then is at start of line)
                        if (Console.CursorLeft == 1 && Console.CursorTop > cursorYInit &&
                            (cmdInput.Length + cursorXOffset) % Console.BufferWidth == 0) {
                            Console.CursorTop--;
                            Console.CursorLeft = Console.BufferWidth - 1;
                            Console.Write(" \b");
                        } else if (cursorXTotal != 0) {
                            Console.CursorLeft--;
                            Console.Write(" \b");
                        } else // makes it possible to backspace to start of line (cursor x = 0)
                          {
                            Console.CursorLeft--;
                        }

                        if (cursorXTotal > 0) {
                            cmdInput.Remove(cursorXTotal - 1, 1);
                            cursorXTotal--;

                            // move text after backspace one to the left if there is text after cursor
                            if (cursorXTotal < cmdInput.Length) {
                                int tempPosY = Console.CursorTop;

                                if (lineFlag == true) // if cursor x at start of line
                                {
                                    Console.CursorTop = tempPosY - 1;
                                    Console.CursorLeft = Console.BufferWidth - 1;
                                }
                                Console.Write(cmdInput.ToString(cursorXTotal, cmdInput.Length - cursorXTotal) + " \b");  

                                SetCursorEndOfInput();
                            }
                        }
                    }
                    break;

                case ConsoleKey.LeftArrow:
                    if (Console.CursorLeft == 0 && Console.CursorTop > cursorYInit) // if cursor x at start of line but not start of input
                    {
                        Console.CursorTop--;
                        Console.CursorLeft = Console.BufferWidth - 1;
                        cursorXTotal--;
                    } else if (Console.CursorLeft > cursorXOffset || Console.CursorTop > cursorYInit) // if cursor x not at start of input
                      {
                        Console.CursorLeft--;
                        cursorXTotal--;
                    }
                    break;

                case ConsoleKey.RightArrow when !(Console.CursorLeft - cursorXOffset <
                                                   cmdInput.Length - (Console.CursorTop - cursorYInit) * Console.BufferWidth):
                    return; // cursor can not exeed length of input
                case ConsoleKey.RightArrow:
                    if (Console.CursorLeft == Console.BufferWidth - 1) // if cursor x at end of line
                    {
                        Console.CursorTop++;
                        Console.CursorLeft = 0;
                    } else {
                        Console.CursorLeft++;
                    }
                    cursorXTotal++;
                    break;

                case ConsoleKey.UpArrow when history.Count == 0:
                    return;
                case ConsoleKey.UpArrow: //up and down for history
                    ClearInput();

                    if (index == -1) // jumps to first element of history at first use of up/down key
                        index = 0;
                    else if (index > 0)
                        index--;

                    Console.Write(history[index]);

                    cmdInput.Clear();
                    cmdInput.Append(history[index]);

                    cursorXTotal = cmdInput.Length;

                    SetCursorEndOfInput();
                    break;

                case ConsoleKey.DownArrow when history.Count == 0:
                    return;
                case ConsoleKey.DownArrow:
                    ClearInput();

                    if (index == -1) // jumps to last element of history at first use of up/down key
                        index = history.Count - 1;
                    else if (index < history.Count - 1)
                        index++;

                    Console.Write(history[index]);

                    cmdInput.Clear();
                    cmdInput.Append(history[index]);

                    cursorXTotal = cmdInput.Length;

                    SetCursorEndOfInput();
                    break;

                case ConsoleKey.PageUp when history.Count == 0:
                    return;
                case ConsoleKey.PageUp: //page up/down for first/last history entry
                    ClearInput();

                    index = 0;

                    Console.Write(history[0]);

                    cmdInput.Clear();
                    cmdInput.Append(history[0]);

                    cursorXTotal = cmdInput.Length;

                    SetCursorEndOfInput();
                    break;

                case ConsoleKey.PageDown when history.Count == 0:
                    return;
                case ConsoleKey.PageDown:
                    ClearInput();

                    index = history.Count - 1;

                    Console.Write(history[^1]);

                    cmdInput.Clear();
                    cmdInput.Append(history[^1]);

                    cursorXTotal = cmdInput.Length;

                    SetCursorEndOfInput();
                    break;

                case ConsoleKey.End: //ende key
                    cursorXTotal = cmdInput.Length;
                    SetCursorEndOfInput();
                    break;

                case ConsoleKey.Home: //pos1 key
                    Console.CursorTop = cursorYInit;
                    Console.CursorLeft = cursorXOffset;
                    cursorXTotal = 0;
                    break;

                case ConsoleKey.Delete when !(Console.CursorLeft - cursorXOffset <
                                               cmdInput.Length - (Console.CursorTop - cursorYInit) * Console.BufferWidth):
                    return; // if not at end of input
                case ConsoleKey.Delete: //entf key
                    cmdInput.Remove(cursorXTotal, 1);

                    Console.Write(cmdInput.ToString(cursorXTotal, cmdInput.Length - cursorXTotal) + " \b");
                    
                    SetCursorEndOfInput();
                    break;

                case ConsoleKey.Escape:
                    Console.CursorTop = cursorYInit;
                    Console.CursorLeft = cursorXOffset;

                    Console.Write("0"); // control char for esc to "eat"
                    Console.Write("\b" + new string(' ', cmdInput.Length + 1)); // clear area of input

                    Console.CursorTop = cursorYInit;
                    Console.CursorLeft = cursorXOffset;

                    history.Add(cmdInput.ToString()); // feature might not be desired

                    cmdInput.Clear();
                    cursorXTotal = 0;
                    break;

                case ConsoleKey.Tab: //just ignore for now / prevent user tabbing
                    break;

                case ConsoleKey.Insert: //just ignore for now / mode switching not really necessary
                    break;

                default: // handle normal key input
                    Console.Write(cki.KeyChar);

                    if (cursorXTotal >= cmdInput.Length) // if cursor is at end of input
                    {
                        cmdInput.Append(cki.KeyChar);
                    } else {
                        cmdInput.Insert(cursorXTotal, cki.KeyChar);

                        var tempPosY = Console.CursorTop;

                        Console.Write(cmdInput.ToString(cursorXTotal + 1, cmdInput.Length - cursorXTotal - 1)); // move text after insertion one to the right

                        Console.CursorTop = tempPosY;

                        if ((cursorXOffset + cursorXTotal) % Console.BufferWidth == Console.BufferWidth - 1) // if cursor x at end of line
                        {
                            Console.CursorLeft = 0;
                        } else {
                            Console.CursorLeft = (cursorXOffset + cursorXTotal) % Console.BufferWidth + 1;
                        }
                    }
                    cursorXTotal++;
                    break;
            }
        }

        // sets cursor position to end of user input (behind last character)
        private static void SetCursorEndOfInput() {
            Console.CursorTop = cursorYInit + (cursorXTotal + cursorXOffset) / Console.BufferWidth; // '/' discards remainder
            Console.CursorLeft = (cursorXTotal + cursorXOffset) % Console.BufferWidth;
        }

        // deletes all user input and returns cursor to starting position
        private static void ClearInput() {
            Console.CursorTop = cursorYInit;
            Console.CursorLeft = cursorXOffset;

            Console.Write(new string(' ', cmdInput.Length + 1)); // clear area of input

            Console.CursorTop = cursorYInit;
            Console.CursorLeft = cursorXOffset;
        }

        // writes all output cached in the outputwriter to the console, returns true if any text was printed, otherwise returns false
        private static void PrintText(string prompt) {
            var output = OutputWriter?.GetText();

            if (output?.text.Length == 0)
                return;

            var inputCache = cmdInput.ToString();

            Console.CursorTop = cursorYInit;
            Console.CursorLeft = 0;

            Console.WriteLine(new string(' ', cursorXOffset + inputCache.Length)); // clear current user input
            
            Console.CursorTop = cursorYInit;

            var fg = Console.ForegroundColor;
            var bg = Console.BackgroundColor;
            Console.BackgroundColor = output.bgColor;
            Console.ForegroundColor = output.fgColor;
            Console.Write(output.text);
            Console.ForegroundColor = fg;
            Console.BackgroundColor = bg;

            var tempPosY = Console.CursorTop;
            var tempPosX = Console.CursorLeft;

            Console.Write(prompt + inputCache);

            Console.CursorTop = tempPosY + (cursorXTotal + cursorXOffset) / Console.BufferWidth; // '/' discards remainder
            Console.CursorLeft = tempPosX + (cursorXTotal + cursorXOffset) % Console.BufferWidth;
        }
    }
}

