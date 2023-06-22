using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blusutils.DESrv.PDK.ConnectionInterfaces;
/// <summary>
/// An interface to all basic processors in <see cref="Blusutils.DESrv.PDK.ConnectionInterfaces"/>
/// </summary>
public interface IConnectionProcessor {

    /// <summary>
    /// Run connection processing
    /// </summary>
    void Run();

    /// <summary>
    /// Listen for new connections
    /// </summary>
    void Listen();

    /// <summary>
    /// Accept a new connection
    /// </summary>
    /// <returns></returns>
    Task Accept();

    /// <summary>
    /// Close processor
    /// </summary>
    void Close();
}
