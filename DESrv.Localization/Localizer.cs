namespace Blusutils.DESrv.Localization {
    using System.Text.Json;
    
    

    public class Localizer {
        public bool Strict { get; set; } = false;
        public string CurrentLocale { get; set; } = "en";
        Dictionary<string, LocalizationManager> Locales { get; set; } = new Dictionary<string, LocalizationManager>();

        public void Load(string path) {
            foreach (var f in Directory.GetFiles(path))
                if (f.EndsWith(".json"))
                    Locales.Add(Path.GetFileNameWithoutExtension(f), new (f));
        }

        public string Translate(string key, string? defstr = null) {
            try {
                return Locales[CurrentLocale].GetTranslation(key, Strict)??defstr??"";
            } catch (KeyNotFoundException e) {
                if (Strict)
                    throw new LocaleKeyException(key, false, e);
                else return defstr ?? "";
            }
        }
        public string Translate(string key, string? defstr = null, params object[] format) {
            return string.Format(Translate(key, defstr), args: format);
        }
    }
}