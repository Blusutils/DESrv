namespace Blusutils.DESrv.PDK {
    public abstract class PDKExtensionAttribute : Attribute {
        public ExtensionMetadata Metadata { get; private set; }
        public PDKExtensionAttribute(ExtensionMetadata metadata) {
            Metadata = metadata;
        }
    }
    public abstract class ExtensionEntrypointAttribute : Attribute { }
    public abstract class ExtensionOnLoadAttribute : Attribute { }
    public abstract class ExtensionOnUnloadAttribute : Attribute { }
    public abstract class ExtensionAddonLoaderAttribute : Attribute { }
}