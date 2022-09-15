namespace DESCEnd.Config {
    /// <summary>
    /// Configuration reader
    /// </summary>
    public class ConfigReader {
        /// <summary>
        /// Read the config
        /// </summary>
        /// <param name="path">Path to config. If not set, path to current assembly used as default</param>
        /// <returns><see cref="ConfigurationModel"/> with readed data from JSON file</returns>
        public static T Read<T>(string path = null) {
            var configPath = path ?? System.Reflection.Assembly.GetExecutingAssembly().Location.Replace(Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location), "config.json");
            var notExists = () => {
                _ = File.Create(configPath);
                using StreamWriter file = new(configPath);
                file.WriteLine("{}");
                file.Close();
                return true;
            };
            _ = File.Exists(configPath) || notExists();

            using (StreamReader r = new StreamReader(configPath)) {
                string json = r.ReadToEnd();
                return System.Text.Json.JsonSerializer.Deserialize<T>(json);
            }
        }
    }
}
