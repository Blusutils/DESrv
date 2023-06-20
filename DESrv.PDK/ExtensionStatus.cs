namespace Blusutils.DESrv.PDK;

/// <summary>
/// Extension status codes
/// </summary>
public enum ExtensionStatus {
    /// <summary>
    /// No info about extension
    /// </summary>
    Unknown = 0,
    /// <summary>
    /// The extension was found, but was not loaded in a session
    /// </summary>
    Unloaded,
    /// <summary>
    /// The extension was succefully loaded
    /// </summary>
    Loaded,
    /// <summary>
    /// The extension was succefully loaded as addon or child dependency
    /// </summary>
    LoadedAsChildren,
    /// <summary>
    /// Extension loading failed
    /// </summary>
    Failed
}