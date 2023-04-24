namespace Blusutils.DESrv.PDK {
    /// <summary>
    /// PDK extension metadata
    /// </summary>
    public class ExtensionMetadata {
        object? bindedTo = null;
        /// <summary>
        /// ID of the extension, must be unique and contain only A-Z, a-z, 0-9, _ symbols
        /// </summary>
        public required string ID { get; set; }
        /// <summary>
        /// Type of the extension, means how it will be loaded
        /// </summary>
        public required ExtensionType ExtensionType { get; set; }
        /// <summary>
        /// Readable name of the extension
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// Who was authored this extension
        /// </summary>
        public required string Author { get; set; }
        /// <summary>
        /// Version of the extension
        /// </summary>
        public required Version Version { get; set; }
        /// <summary>
        /// Minimum DESrv version with which the extension is compatible;
        /// checks according to the SemVer principle
        /// (major - incompatible, minor - compatible to a greater extent, minor and patch - fully compatible)
        /// </summary>
        public required Version TargetDESrvVersion { get; set; }
        /// <summary>
        /// Description of the extension
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// What ports should be allowed for this extension (behavior is controlled by the DESrv config)
        /// </summary>
        public int[]? AllowedPorts { get; set; }
        /// <summary>
        /// List of extensions on which this extension depends
        /// </summary>
        public string[]? Dependencies { get; set; }
        /// <summary>
        /// Parent extension ID (if the current extension is an addon)
        /// </summary>
        public string? RefersTo { get; set; }
    }
}
