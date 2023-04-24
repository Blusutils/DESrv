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
    void Run();
    void Accept();
    void Process();
    void Listen();
    void Close();
}
