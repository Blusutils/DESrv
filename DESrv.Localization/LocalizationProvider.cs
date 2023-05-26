using System.Text.Json;

namespace Blusutils.DESrv.Localization; 

/// <summary>
/// Localization provider for DESrv
/// </summary>
public class LocalizationProvider {
    /// <summary>
    /// Should an exception be thrown if no key is found
    /// </summary>
    public bool Strict { get; set; } = false;
    /// <summary>
    /// Current project locale
    /// </summary>
    public string CurrentLocale { get; set; } = "en";
    /// <summary>
    /// List of localization managers for locales
    /// </summary>
    public Dictionary<string, LocalizationManager> Locales { get; private set; } = new();

    /// <summary>
    /// Load localization data from file
    /// </summary>
    /// <param name="path">Path to file</param>
    public void Load(string path) {
        foreach (var f in Directory.GetFiles(path))
            if (f.EndsWith(".json"))
                Locales.Add(Path.GetFileNameWithoutExtension(f), new(f));
    }

    /// <summary>
    /// Load localization data from JSON string
    /// </summary>
    /// <param name="data">Source JSON</param>
    /// <param name="locale">Locale language</param>
    public void LoadFromString(string data, string locale) {
        Locales.Add(locale, new(data, false));
    }

    /// <summary>
    /// Try to get translation by key
    /// </summary>
    /// <param name="key">Dot-separated key, path to nested string</param>
    /// <param name="defstr">String to return if no translation found</param>
    /// <returns>Translated string, or <paramref name="defstr"/>, or null</returns>
    /// <exception cref="LocaleKeyException">If something went wrong when trying to get string by key and strict mode is enabled</exception>
    public string Translate(string key, string? defstr = null) {
        try {
            return Locales[CurrentLocale].GetTranslation(key, Strict)??defstr??"";
        } catch (KeyNotFoundException e) {
            if (Strict)
                throw new LocaleKeyException(key, false, e);
            else return defstr ?? "";
        }
    }

    /// <summary>
    /// Try to get translation by key and try to format
    /// </summary>
    /// <param name="key">Dot-separated key, path to nested string</param>
    /// <param name="defstr">String to return if no translation found</param>
    /// <param name="format">Params to use in formattion</param>
    /// <returns>Translated string, or <paramref name="defstr"/>, or null</returns>
    /// <exception cref="LocaleKeyException">If something went wrong when trying to get string by key and strict mode is enabled</exception>
    public string Translate(string key, string? defstr = null, params object[] format) {
        return string.Format(Translate(key, defstr), args: format);
    }
}