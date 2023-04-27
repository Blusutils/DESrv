using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Blusutils.DESrv.PDK;

namespace Blusutils.DESrv;

/// <summary>
/// Container of Metadata, Status and Assembly reference of the extension
/// </summary>
public class ExtensionContainer {
    public ExtensionMetadata Metadata { get; set; }
    public ExtensionStatus Status { get; set; }
    public Assembly Assembly { get; set; }
    public CancellationToken CancellationToken { get; set; }
    public object Instance { get; set; }
}

/// <summary>
/// Extension loader for Plugin DeVelopment Kit (PDK)
/// </summary>
public class PdkLoader { // TODO implement pdkloader

    /// <summary>
    /// Default location with extensions
    /// </summary>
    public string? LoadFrom { get; set; }

    /// <summary>
    /// List of extensions
    /// </summary>
    public Dictionary<string, ExtensionContainer> Extensions { get; } = new();

    /// <summary>
    /// Add an extension from file
    /// </summary>
    /// <param name="pathToExtension">Path to extension</param>
    public void AddExtension(string pathToExtension) { }

    /// <summary>
    /// Adds all extensions from default directory
    /// </summary>
    public void AddExtensionsFromDirectory() { }

    /// <summary>
    /// Adds all extensions from specified directory
    /// </summary>
    /// <param name="pathToExtension">Path to directory</param>
    public void AddExtensionsFromDirectory(string pathToExtension) { }

    /// <summary>
    /// Load extension by ID
    /// </summary>
    /// <param name="id"></param>
    public ExtensionContainer? LoadExtension(string id) { return null; }

    /// <summary>
    /// Unoad extension by ID
    /// </summary>
    /// <param name="id"></param>
    public void UnloadExtension(string id) { }

    /// <summary>
    /// Suspend extension by ID
    /// </summary>
    /// <param name="id"></param>
    public void SuspendExtension(string id) { }

    /// <summary>
    /// Remove extension by ID
    /// </summary>
    /// <param name="id"></param>
    public void RemoveExtension(string id) { }
}
