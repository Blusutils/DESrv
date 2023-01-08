using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blusutils.DESrv.Logging {
    public interface ICommandInputProcessor {
        void Process(string cmd);
    }
}
