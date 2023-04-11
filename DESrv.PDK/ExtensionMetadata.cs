namespace Blusutils.DESrv.PDK {
    public class ExtensionMetadata {
        object? bindedTo = null;
        public required string ID { get; set; }
        public required ExtensionType ExtensionType { get; set; }
        public required string Name { get; set; }
        public required string Author { get; set; }
        public required Version Version { get; set; }
        public required Version TargetDESrvVersion { get; set; }
        public string? Description { get; set; }
        public int[]? AllowedPorts { get; set; }
        public string[]? Dependencies { get; set; }
        public string? RefersTo { get; set; }
    }
}
