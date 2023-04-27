using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blusutils.DESrv.PDK.ConnectionInterfaces.UDP;
/// <summary>
/// A more high-level User Datagram Protocol processor. Use this class only for own implementations. Instead, see <see cref="UdpListenerH"/>
/// </summary>
public class BaseUdpProcessor : IConnectionProcessor, IDisposable {
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
