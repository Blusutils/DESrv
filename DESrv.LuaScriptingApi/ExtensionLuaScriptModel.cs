using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blusutils.DESrv.LuaScriptingApi;

/// <summary>
/// Represents a Lua script for certain extension
/// </summary>
public class ExtensionLuaScriptModel {

    /// <summary>
    /// Readable name of the script
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// ID of DESrv extension to what this script refers
    /// </summary>
    public string ExtensionID { get; set; }

    /// <summary>
    /// Script author
    /// </summary>
    public string Author { get; set; }

    /// <summary>
    /// Script version
    /// </summary>
    public Version Version { get; set; }

    /// <summary>
    /// Minimum version of the extension, with which this script is compatible
    /// </summary>
    public Version MinimalExtensionVersion { get; set; }

    /// <summary>
    /// Link to resource related to script
    /// </summary>
    public string Link { get; set; }

    /// <summary>
    /// Metadata table instance
    /// </summary>
    public LuaTable InfoTable { get; set; }

    /// <summary>
    /// Table containing all functions of script
    /// </summary>
    public LuaTable ExecutableSpaceTable { get; set; }
}
