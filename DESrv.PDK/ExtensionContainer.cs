using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Blusutils.DESrv.PDK;

#pragma warning disable CS1591
#pragma warning disable CS8618
/// <summary>
/// Container of Metadata, Status and Assembly reference of the extension
/// </summary>
public class ExtensionContainer {
    public ExtensionMetadata Metadata { get; set; }
    public ExtensionStatus Status { get; set; }
    public Assembly Assembly { get; set; }
    public CancellationTokenSource CancellationToken { get; set; }
    public object Instance { get; set; }
}
#pragma warning restore CS8618
#pragma warning restore CS1591