namespace Blusutils.DESrv.PDK {
    /// <summary>
    /// This attribute tags class as PDK extension
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class PDKExtensionAttribute : Attribute {
        public ExtensionMetadata Metadata { get; private set; }
        /// <summary>
        /// Create a new instance of attribute
        /// </summary>
        /// <param name="metadata">Extension metadata</param>
        public PDKExtensionAttribute(ExtensionMetadata metadata) {
            Metadata = metadata;
        }
    }
    /// <summary>
    /// This attribute tags method as extension entrypoint that calls once on start
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class ExtensionEntrypointAttribute : Attribute { }
    /// <summary>
    /// This attribute tags method as event that calls every time when extension loads
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class ExtensionOnLoadAttribute : Attribute { }
    /// <summary>
    /// This attribute tags method as event that calls every time when extension unloads
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class ExtensionOnUnloadAttribute : Attribute { }
    /// <summary>
    /// This attribute tags method as extension addon loader that calls every time when DESrv PDK tries to load addon to this extension
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class ExtensionAddonLoaderAttribute : Attribute { }
}