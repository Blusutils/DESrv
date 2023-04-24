using System.Text.Json;

namespace Blusutils.DESrv.Localization; 

public class Localizer {
    public bool Strict { get; set; } = false;
    public string CurrentLocale { get; set; } = "en";
    public Dictionary<string, LocalizationManager> Locales { get; private set; } = new Dictionary<string, LocalizationManager>();

    public void Load(string path) {
        foreach (var f in Directory.GetFiles(path))
            if (f.EndsWith(".json"))
                Locales.Add(Path.GetFileNameWithoutExtension(f), new(f));
    }

    public void LoadFromString(string data, string locale) {
        Locales.Add(locale, new(data, false));
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