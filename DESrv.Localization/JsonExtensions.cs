using System.Text.Json;
using System.Text.Json.Serialization;

namespace Blusutils.DESrv.Localization;
public static class JsonExtensions {

    /// <summary>
    /// Gets string by dot-separated key
    /// </summary>
    /// <param name="element">JSON element to use</param>
    /// <param name="keyPath">Dot-separated key, path to nested value</param>
    /// <returns>Found string or null</returns>
    /// <exception cref="JsonException">If <paramref name="keyPath"/> is invalid</exception>
    /// <exception cref="KeyNotFoundException">If <paramref name="keyPath"/> is not contains in JSON</exception>
    /// <exception cref="InvalidOperationException">If <paramref name="keyPath"/> value is not a string</exception>
    public static string? GetString(this JsonElement element, string keyPath) {
        var keys = keyPath.Split('.');
        var currentElement = element;

        foreach (var key in keys) {
            if (currentElement.ValueKind != JsonValueKind.Object) {
                throw new JsonException("Invalid key path: " + keyPath);
            }

            if (!currentElement.TryGetProperty(key, out currentElement)) {
                throw new KeyNotFoundException("Key not found: " + key);
            }
        }

        if (currentElement.ValueKind != JsonValueKind.String) {
            throw new InvalidOperationException("Value is not a string: " + keyPath);
        }

        return currentElement.GetString();
    }
}
