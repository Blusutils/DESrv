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
