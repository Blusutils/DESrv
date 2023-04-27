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
    /// <inheritdoc/>
    public void Accept() => throw new NotImplementedException();
    /// <inheritdoc/>
    public void Close() => throw new NotImplementedException();
    /// <inheritdoc/>
    public void Listen() => throw new NotImplementedException();
    /// <inheritdoc/>
    public void Process() => throw new NotImplementedException();
    /// <inheritdoc/>
    public void Run() => throw new NotImplementedException();
    /// <inheritdoc/>
    public void Dispose() => throw new NotImplementedException();
}
