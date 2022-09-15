namespace DESCEnd.Config {
    public class ConfigurationModel {
        public string IpAdress;

        public void Extend(Dictionary<string, string> config) {
            var flist = new List<string>();
            foreach (var finfo in GetType().GetFields()) {
                flist.Add(finfo.Name);
            }
            foreach (var key in config.Keys) {
                if (flist.Contains(key)) {
                    var field = GetType().GetField(key);
                    var ftype = field.FieldType;
                    field.SetValue(this, ftype == typeof(string) ? key : key);
                }
            }
        }
    }
}
