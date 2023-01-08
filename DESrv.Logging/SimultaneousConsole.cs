using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blusutils.DESrv.Logging {
    public class ConsoleControlSequence {
        public ConsoleKey EnterKey { get; set; } = ConsoleKey.Enter;
        public ConsoleKey ExitKeyPrimary { get; set; } = ConsoleKey.F4;
        public ConsoleModifiers? ExitKeySecondary { get; set; } = ConsoleModifiers.Alt;
        public ConsoleKey PreviousCommand { get; set; } = ConsoleKey.UpArrow;
        public ConsoleKey NextCommand { get; set; } = ConsoleKey.DownArrow;
        public ConsoleKey PreviousChar { get; set; } = ConsoleKey.LeftArrow;
        public ConsoleKey NextChar { get; set; } = ConsoleKey.RightArrow;
    }
    public static class SimultaneousConsole {
        public static ConsoleColor ForegroundColor { get; set; } = ConsoleColor.White;
        public static ConsoleColor BackgroundColor { get; set; } = ConsoleColor.Black;
        public static string InputBeginChars { get; set; } = "> ";
        public static event Action<string>? OnNewCommandGotEvent;
        static StringBuilder ok = new();
        static List<string> cmds = new();
        static int lastCur = 0;
        public static void WriteLine(string msg) {
            lock (ok!) {
                var left = Console.CursorLeft;

                Console.CursorTop = lastCur;
                Console.CursorLeft = 0;

                var fg = Console.ForegroundColor;
                var bg = Console.BackgroundColor;
                Console.BackgroundColor = BackgroundColor;
                Console.ForegroundColor = ForegroundColor;
                Console.WriteLine(msg);
                Console.ForegroundColor = fg;
                Console.BackgroundColor = bg;
                Console.WriteLine($"> {ok}");

                Console.CursorTop = ++lastCur;
                Console.CursorLeft = 2 + ok.Length;
            }
        }
        public static void StartRead(ConsoleControlSequence? sequence = null, bool waitForever = false) {
            if (sequence == null) { throw new ArgumentNullException(nameof(sequence)); }
            while (true) {
                while (true) {
                    var k = Console.ReadKey();
                    if (k.Key == sequence.EnterKey) break;
                    else if (k.Key == sequence.ExitKeyPrimary && (
                        (sequence.ExitKeySecondary != null && k.Modifiers == sequence.ExitKeySecondary)
                            || sequence.ExitKeySecondary == null
                        )) {
                        waitForever = false;
                        break;
                    } else ok.Append(k.KeyChar);
                }
                cmds!.Add(ok.ToString());
                OnNewCommandGotEvent?.Invoke(ok.ToString());
                ok = new();
                Console.CursorLeft = 0;
                Console.CursorTop = ++lastCur;
                if (!waitForever) break;
                Console.Write("> ");
                Console.CursorLeft = 2;
            }
        }
        public static List<string> GetCommandHistory(int from, int to) {
            return cmds;
        }
    }
}
