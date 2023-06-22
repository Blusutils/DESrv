using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blusutils.DESrv.Logging; 

/// <summary>
/// Command input processor interface
/// </summary>
public interface ICommandInputProcessor {

    /// <summary>
    /// Method that proceeds commands
    /// </summary>
    /// <param name="args">Command text</param>
    void Process(string[] args);
}
