using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blusutils.DESrv.Logging;

namespace Blusutils.DESrv {
    public class CommandInputProcessor : ICommandInputProcessor {
        public void Process(string cmd) {
            var args = cmd.ToLower().Split(" ");
            Func<object> anon = args[0] switch {
                    "write" => () => {
                        ConsoleService.Console.WriteLine(cmd);
                        return null;
                    }
                    ,
                    _ => () => { return 0; }
                };
            anon();
        }
    }
}
