using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Blusutils.DESrv.Localization {
    /// <summary>
    /// LocalizationManager string list class
    /// </summary>
    public class LocalizationManager {
        private Dictionary<string, object>? Translations { get; set; } = new Dictionary<string, object>();
        /// <summary>
        /// Create new instance of <see cref="LocalizationManager"/> from dict
        /// </summary>
        /// <param name="translations">Dict with Translations</param>
        public LocalizationManager(Dictionary<string, object>? translations = null) => this.Translations = translations;
        /// <summary>
        /// Create new instance of <see cref="LocalizationManager"/> from file path
        /// </summary>
        /// <param name="path">Path to localization file</param>
        public LocalizationManager(string? path = null) => LoadTranslation(path);

        public string? GetTranslation(string key, bool strict = true) {
            if (Translations is null && strict) throw new NullReferenceException("translations not loaded");
            object nextObj = Translations!;
            foreach (var k in key.Split(".")) {
                if (nextObj is Dictionary<string, object> dict) {
                    try {
                        nextObj = dict[k];
                    } catch (KeyNotFoundException e) {
                        if (strict)
                            throw new LocaleKeyException($"{key}` (iteration {k})`", fromExc: e);
                        else return null;
                    }
                } else return nextObj as string;
            }
            if (strict)
                throw new LocaleKeyException($"{key}");
            else return null;
        }
        /// <summary>
        /// Load translation from file
        /// </summary>
        /// <param name="path">Path to localization file</param>
        public void LoadTranslation(string? path) {
            if (path is null) throw new ArgumentNullException(nameof(path), "no path provided");
            Translations = JsonSerializer.Deserialize<Dictionary<string, object>>(File.ReadAllText(path));
        }
    }
}
