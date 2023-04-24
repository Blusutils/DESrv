using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blusutils.DESrv.PDK.ConnectionInterfaces.KCP;
/// <summary>
/// A more high-level KCP (don't know what it stands for) processor. Use this class only for own implementations. Instead, see <see cref="KcpListenerH"/>
/// </summary>
public class BaseKcpProcessor : IConnectionProcessor, IDisposable {
    public void Accept() => throw new NotImplementedException();
    public void Close() => throw new NotImplementedException();
    public void Listen() => throw new NotImplementedException();
    public void Process() => throw new NotImplementedException();
    public void Run() => throw new NotImplementedException();
    public void Dispose() => throw new NotImplementedException();
}
