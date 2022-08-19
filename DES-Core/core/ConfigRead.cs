using System;

namespace DESCore.Utils {
    /// <summary>
    /// Configuration reader
    /// </summary>
    class ConfigReader {
        /// <summary>
        /// Read the config
        /// </summary>
        /// <returns><see cref="Dictionary{string, string}"/> (string, string) with readed data from JSON file</returns>
        public static Dictionary<string, string> Read() {
            var configPath = Path.Combine(".", "config.json");
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
                Dictionary<string, string> config = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                return config;
            }
        }
    }
}
