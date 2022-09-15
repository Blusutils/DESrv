using System.Text.Json;

namespace DESCEnd.L10n {
    /// <summary>
    /// Exception what raises when key not found during getting localization string
    /// </summary>
    public class LocaleKeyException : Exception {
        /// <summary>
        /// Create new <see cref="LocaleKeyException"/>
        /// </summary>
        /// <param name="key">Key what is unable to find</param>
        /// <param name="isFromListing">Is this exception raises from translation string list (dict)?</param>
        /// <param name="fromExc">Exception what caused this exception</param>
        public LocaleKeyException(string key, bool isFromListing = true, Exception fromExc = null)
            : base($"Key `{key}` not found in {(isFromListing ? "localization keys" : "locale list")}", fromExc) { }
    }
    /// <summary>
    /// Localization string list class
    /// </summary>
    public class Localization {
        private Dictionary<string, object> translations { get; set; } = new Dictionary<string, object>();
        /// <summary>
        /// Create new instance of <see cref="Localization"/> from dict
        /// </summary>
        /// <param name="translations">Dict with translations</param>
        public Localization(Dictionary<string, object> translations = null) => this.translations = translations;
        /// <summary>
        /// Create new instance of <see cref="Localization"/> from file path
        /// </summary>
        /// <param name="path">Path to localization file</param>
        public Localization(string path = null) => LoadTranslation(path);

        public string GetTranslation(string key, bool strict = true) {
            object nextObj = translations;
            foreach (var k in key.Split(".")) {
                if (nextObj is Dictionary<string, object> dict) {
                    try {
                        nextObj = dict[k];
                    } catch (KeyNotFoundException e) {
                        if (strict)
                            throw new LocaleKeyException($"{key} iteration {k}", fromExc: e);
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
        public void LoadTranslation(string path) {
            translations = JsonSerializer.Deserialize<Dictionary<string, object>>(File.ReadAllText(path));
        }
    }

    public class Localizer {
        public bool Strict { get; set; } = false;
        public string CurrentLocale { get; set; } = "en";
        Dictionary<string, Localization> Locales { get; set; } = new Dictionary<string, Localization>();

        public void Load(string path) {
            foreach (var f in Directory.GetFiles(path))
                if (f.EndsWith(".json"))
                    Locales.Add(Path.GetFileNameWithoutExtension(f), new Localization(f));
        }

        public string Translate(string key, string defstr = null) {
            try {
                return Locales[CurrentLocale].GetTranslation(key, Strict);
            } catch (KeyNotFoundException e) {
                if (Strict)
                    throw new LocaleKeyException(key, false, e);
                else return defstr;
            }
        }
        public string Translate(string key, string defstr = null, params object[] format) {
            return string.Format(Translate(key, defstr), args: format);
        }
    }
}