namespace DESCEnd.Config {
    /// <summary>
    /// Abstract class for configs
    /// </summary>
    public abstract class AbstractConfig { public abstract void Extend(Dictionary<string, object> config); }
    /// <summary>
    /// Base configuration model
    /// </summary>
    public class ConfigurationModel : AbstractConfig {
        public sealed override void Extend(Dictionary<string, object> config) {
            var flist = new List<string>();
            foreach (var finfo in GetType().GetFields()) {
                flist.Add(finfo.Name);
            }
            foreach (var key in config.Keys) {
                if (flist.Contains(key)) {
                    var field = GetType().GetField(key);
                    field.SetValue(this, field.FieldType == typeof(string) ? config[key] : Convert.ChangeType(config[key], field.FieldType));
                }
            }
        }
    }
}
