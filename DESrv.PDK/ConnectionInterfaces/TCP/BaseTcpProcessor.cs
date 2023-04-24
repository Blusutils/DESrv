using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blusutils.DESrv.PDK.ConnectionInterfaces.TCP;
/// <summary>
/// A more high-level Transmission Control Protocol processor. Use this class only for own implementations. Instead, see <see cref="TcpListenerH"/>
/// </summary>
public class BaseTcpProcessor : IConnectionProcessor, IDisposable {
    public void Accept() => throw new NotImplementedException();
    public void Close() => throw new NotImplementedException();
    public void Listen() => throw new NotImplementedException();
    public void Process() => throw new NotImplementedException();
    public void Run() => throw new NotImplementedException();
    public void Dispose() => throw new NotImplementedException();
}
