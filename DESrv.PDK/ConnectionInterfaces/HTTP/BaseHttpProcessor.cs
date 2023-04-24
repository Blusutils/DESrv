using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blusutils.DESrv.PDK.ConnectionInterfaces.HTTP;
/// <summary>
/// A more high-level Hyper Text Transfer Protocol processor. Use this class only for own implementations. Instead, see <see cref="HttpListenerH"/>
/// </summary>
public class BaseHttpProcessor : IConnectionProcessor, IDisposable {
    public void Accept() => throw new NotImplementedException();
    public void Close() => throw new NotImplementedException();
    public void Listen() => throw new NotImplementedException();
    public void Process() => throw new NotImplementedException();
    public void Run() => throw new NotImplementedException();
    public void Dispose() => throw new NotImplementedException();
}
