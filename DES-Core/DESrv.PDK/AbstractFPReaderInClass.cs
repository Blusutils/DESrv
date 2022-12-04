namespace DESrv.PDK {
    /// <summary>
    /// A class that represents an interface to implement reading the fields of a class in instance from another .NET assembly
    /// </summary>
    public abstract class AbstractFPReaderInClass {
        public abstract object? GetFieldValue(string name);
        public abstract object? GetPropertyValue(string name);
    }
}