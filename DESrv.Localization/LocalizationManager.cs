﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Blusutils.DESrv.Localization;

namespace Blusutils.DESrv.Localization; 
/// <summary>
/// LocalizationManager string list class
/// </summary>
public class LocalizationManager {

    /// <summary>
    /// List of translations
    /// </summary>
    public JsonElement? Translations { get; private set; } = null;

    /// <summary>
    /// Create new instance of <see cref="LocalizationManager"/> from dict
    /// </summary>
    /// <param name="translations">Dict with Translations</param>
    public LocalizationManager(JsonElement? translations = null) => Translations = translations;

    /// <summary>
    /// Create new instance of <see cref="LocalizationManager"/> from file path or raw string
    /// </summary>
    /// <param name="data">Path to localization file or raw JSON string</param>
    /// <param name="isFromPath">Is translation loads from path</param>
    public LocalizationManager(string? data = null, bool isFromPath = true) {
        if (isFromPath)
            LoadTranslation(data);
        else
            LoadTranslationFromString(data ??"{}");
    }

    /// <summary>
    /// Gets translation by key
    /// </summary>
    /// <param name="key">Dot-separated key, path to nested string</param>
    /// <param name="strict">Should an exception be thrown if no key is found</param>
    /// <returns>Translated string</returns>
    /// <exception cref="NullReferenceException">When translations is not loaded</exception>
    /// <exception cref="LocaleKeyException">If something went wrong when trying to get string by key</exception>
    public string? GetTranslation(string key, bool strict = true) {
        if (Translations is null)
            if (strict)
                throw new NullReferenceException("translations not loaded");
            else
                return null;
        try {
            return ((JsonElement)Translations).GetString(key);
        } catch (Exception e) {
            if (strict) throw new LocaleKeyException($"{key}", fromExc: e);
            return null;
        }
    }
    /// <summary>
    /// Load translation from file
    /// </summary>
    /// <param name="path">Path to localization file</param>
    public void LoadTranslation(string? path) {
        if (path is null)
            throw new ArgumentNullException(nameof(path), "no path provided");
        Translations = JsonDocument.Parse(File.ReadAllText(path)).RootElement;
    }
    /// <summary>
    /// Load translation from string
    /// </summary>
    /// <param name="data">Source string with translation in JSON</param>
    public void LoadTranslationFromString(string data) {
        Translations = JsonDocument.Parse(data).RootElement;
    }
}
