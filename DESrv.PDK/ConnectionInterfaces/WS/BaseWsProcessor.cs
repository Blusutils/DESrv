using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blusutils.DESrv.PDK.ConnectionInterfaces.WS;
/// <summary>
/// A more high-level Web Socket protocol processor. Use this class only for own implementations. Instead, see <see cref="WsListenerH"/>
/// </summary>
public class BaseWsProcessor : IConnectionProcessor, IDisposable {
    public void Accept() => throw new NotImplementedException();
    public void Close() => throw new NotImplementedException();
    public void Listen() => throw new NotImplementedException();
    public void Process() => throw new NotImplementedException();
    public void Run() => throw new NotImplementedException();
    public void Dispose() => throw new NotImplementedException();
}
