using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blusutils.DESrv.PDK.ConnectionInterfaces.FTP;
/// <summary>
/// A more high-level File Transfer Protocol processor. Use this class only for own implementations. Instead, see <see cref="FtpListenerH"/>
/// </summary>
public class BaseFtpProcessor : IConnectionProcessor, IDisposable {
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
