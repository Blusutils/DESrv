namespace Blusutils.DESrv.PDK {
    /// <summary>
    /// This attribute tags class as PDK extension
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class PDKExtensionAttribute : Attribute {
        /// <summary>
        /// Extension metadata
        /// </summary>
        public ExtensionMetadata Metadata { get; private set; }
        /// <summary>
        /// Create a new instance of attribute
        /// </summary>
        /// <param name="ID">ID of the extension, must be unique and contain only A-Z, a-z, 0-9, _ symbols</param>
        /// <param name="ExtensionType">Type of the extension, means how it will be loaded</param>
        /// <param name="Name">Readable name of the extension</param>
        /// <param name="Author">Who was authored this extension</param>
        /// <param name="Version">Version of the extension</param>
        /// <param name="TargetDESrvVersion">Minimum DESrv version with which the extension is compatible;
        /// checks according to the SemVer principle
        /// (major - incompatible, minor - compatible to a greater extent, minor and patch - fully compatible)</param>
        /// <param name="Description">Description of the extension</param>
        /// <param name="AllowedPorts">What ports should be allowed for this extension (behavior is controlled by the DESrv config)</param>
        /// <param name="Dependencies">List of extensions on which this extension depends</param>
        /// <param name="RefersTo">Parent extension ID (if the current extension is an addon)</param>
        public PDKExtensionAttribute(string ID,
            ExtensionType ExtensionType,
            string Name,
            string Author,
            string Version,
            string TargetDESrvVersion,
            string? Description = null,
            int[]? AllowedPorts = null,
            string[]? Dependencies = null,
            string? RefersTo = null) {

            Metadata = new() {
                ID = ID,
                ExtensionType = ExtensionType,
                Name = Name,
                Author = Author,
                Version = new Version(Version),
                TargetDESrvVersion = new Version(TargetDESrvVersion),
                Description = Description,
                AllowedPorts = AllowedPorts,
                Dependencies = Dependencies,
                RefersTo = RefersTo
            };
        }
    }
    /// <summary>
    /// This attribute tags method as extension entrypoint that calls once on start
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ExtensionEntrypointAttribute : Attribute { }
    /// <summary>
    /// This attribute tags method as event that calls every time when extension loads
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ExtensionOnLoadAttribute : Attribute { }
    /// <summary>
    /// This attribute tags method as event that calls every time when extension unloads
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ExtensionOnUnloadAttribute : Attribute { }
    /// <summary>
    /// This attribute tags method as extension addon loader that calls every time when DESrv PDK tries to load addon to this extension
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ExtensionAddonLoaderAttribute : Attribute { }
}