using System.Text.Json;
using System.Text.Json.Serialization;

namespace Blusutils.DESrv.Localization;
public static class JsonExtensions {
    public static string GetString(this JsonElement element, string keyPath) {
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
