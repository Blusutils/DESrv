using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blusutils.DESrv.PDK.ConnectionInterfaces;
/// <summary>
/// A more high-level KCP (don't know what it stands for) processor. It is recommended to use this class for your own, more feature-rich implementations.
/// </summary>
/// <remarks>Warning: Not implemented, reserved for future releases</remarks>
// TODO implement KCP
public class BaseKcpProcessor : IConnectionProcessor, IDisposable {
    /// <inheritdoc/>
    public Task Accept() => throw new NotImplementedException();
    /// <inheritdoc/>
    public void Close() => throw new NotImplementedException();
    /// <inheritdoc/>
    public void Listen() => throw new NotImplementedException();
    /// <inheritdoc/>
    public void Process() => throw new NotImplementedException();
    /// <inheritdoc/>
    public void Run() => throw new NotImplementedException();
    /// <inheritdoc/>
    public void Dispose() => GC.SuppressFinalize(this);
}
