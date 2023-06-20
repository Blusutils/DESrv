using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Blusutils.DESrv.Configuration;
using Blusutils.DESrv.Logging;
using Blusutils.DESrv.PDK;

using static System.Net.Mime.MediaTypeNames;

namespace Blusutils.DESrv;

/// <summary>
/// Extension loader for Plugin Development Kit (PDK)
/// </summary>
public class PdkLoader { // TODO implement pdkloader

    Dictionary<string, int> MaxRetriesCount { get; set; } = new();

    /// <summary>
    /// Default location with extensions
    /// </summary>
    public string? LoadFrom { get; set; }

    /// <summary>
    /// List of extensions
    /// </summary>
    public Dictionary<string, ExtensionContainer> Extensions { get; } = new();

    /// <summary>
    /// Add an extension from file
    /// </summary>
    /// <param name="pathToExtension">Path to extension file</param>
    public void AddExtension(string pathToExtension) {
        Assembly? asm = Assembly.LoadFile(pathToExtension);
        if (asm is null) return;

        ExtensionContainer ext = new() {
            Assembly = asm,
            CancellationToken = new CancellationTokenSource(),
            Metadata = null,
            Status = ExtensionStatus.Unknown,
            Instance = null
        };

        Type[] exts = asm.GetTypes().Where(
            t => t.GetCustomAttribute(typeof(PDKExtensionAttribute)) is not null
        ).ToArray();

        foreach (var extType in exts) {
            ext.Metadata = (extType.GetCustomAttribute(typeof(PDKExtensionAttribute)) as PDKExtensionAttribute)!.Metadata;
            ext.Instance = Activator.CreateInstance(extType) ?? throw new NullReferenceException($"unable to create instance for extension {ext.Metadata.ID} (type {extType})");
            ext.Status = ExtensionStatus.Unknown;
        }
        if (ext.Instance is not null)
            Extensions.Add(pathToExtension, ext);
    }

    /// <summary>
    /// Adds all extensions from default directory
    /// </summary>
    public void AddExtensionsFromDirectory() {
        AddExtensionsFromDirectory(LoadFrom ?? DESrvConfig.Instance?.extensionsDir ?? "");
    }

    /// <summary>
    /// Adds all extensions from specified directory
    /// </summary>
    /// <param name="pathToExtensions">Path to directory</param>
    public void AddExtensionsFromDirectory(string pathToExtensions) {
        foreach (var file in  Directory.GetFiles(pathToExtensions))
            AddExtension(file);
    }

    /// <summary>
    /// Load extension by ID
    /// </summary>
    /// <param name="id"></param>
    public ExtensionContainer? LoadExtension(string id) {
        ExtensionContainer? ext = Extensions?.GetValueOrDefault(id);
        if (ext is null)
            return null;

        if (ext.Status is ExtensionStatus.Unknown) {
            Version ver = ext.Metadata.TargetDESrvVersion;

            if (DESrvConfig.Instance?.extensionsWhitelist is not null
                && !DESrvConfig.Instance.extensionsWhitelist.Contains(id)) {
                Bootstrapper.Logger.Warn($"Extension {id} is not in whitelist, skipping", source: "DESrv.PDK.Extensions.Load");
                ext.Status = ExtensionStatus.Failed;
                return ext;
            }

            if (ver.Major != Bootstrapper.DESrvVersion.Major || ver.Minor > Bootstrapper.DESrvVersion.Minor) {
                Bootstrapper.Logger.Warn($"Extension {id} ({ver}) is incompatable with current DESrv version ({Bootstrapper.DESrvVersion}), skipping", source: "DESrv.PDK.Extensions.Load");
                ext.Status = ExtensionStatus.Failed;
                return ext;
            }
        }

        // TODO process references
        //extm.Metadata.RefersTo;
        //extm.Metadata.Dependencies;

        MethodInfo? methodEntrypoint = null;
        MethodInfo? methodOnload = null;

        foreach (var type in ext.Assembly.DefinedTypes) {
            methodEntrypoint = type.DeclaredMethods.First(
                m => m.GetCustomAttributes(false).First(
                    a => a is ExtensionEntrypointAttribute
                    ) is not null
                );
            methodOnload = type.DeclaredMethods.First(
                m => m.GetCustomAttributes(false).First(
                    a => a is ExtensionOnLoadAttribute
                    ) is not null
                );
        }

        ext.CancellationToken.TryReset();

        new Thread(() => { // TODO threader
            if (methodEntrypoint is null) {
                Bootstrapper.Logger.Warn($"Extension {id} has no entrypoint. Skipping.", source: "DESrv.PDK.Extensions.Load");
                return;
            }
            MaxRetriesCount.TryAdd(id, 0);
            while (!ext.CancellationToken.Token.IsCancellationRequested) {
                try {
                    methodEntrypoint?.Invoke(ext.Instance, null);
                } catch (Exception ex) {
                    var restartsMax = DESrvConfig.Instance!.extensionRestartAttemptsCount.GetValueOrDefault();
                    if (MaxRetriesCount[id] + 1 < restartsMax) {
                        Bootstrapper.Logger.Error($"Something went wrong during {id} extension execution. Restarting entrypoint.", ex, source: "DESrv.PDK.Extensions.Load");
                        MaxRetriesCount[id]++;
                    } else {
                        Bootstrapper.Logger.Critical($"Extension {id} execution was fault. Reason: Entrypoint {methodEntrypoint?.Name} reached maximum of restats limit ({restartsMax} times).", ex, source: "DESrv.PDK.Extensions.Load");
                        ext.Status = ExtensionStatus.Failed;
                        break;
                    }
                }
            }
        }).Start();

        new Thread(() => { // TODO threader
            if (methodOnload is null) {
                Bootstrapper.Logger.Warn($"Extension {id} has no onload listeners. Skipping.", source: "DESrv.PDK.Extensions.Load");
                return;
            }
            try {
                methodOnload?.Invoke(ext.Instance, new object[] { this, new ExtensionLoadedEventArgs(ext) });
                ext.Status = ExtensionStatus.Loaded;
            } catch (Exception ex) {
                Bootstrapper.Logger.Error($"Extension {id} execution was fault. Reason: OnLoad {methodOnload?.Name} throwed an exception.", ex, source: "DESrv.PDK.Extensions.Load");
                ext.Status = ExtensionStatus.Failed;
            }
        }).Start();

        return ext;
    }

    /// <summary>
    /// Unoad extension by ID
    /// </summary>
    /// <param name="id"></param>
    public void UnloadExtension(string id) {

        ExtensionContainer? ext = Extensions?.GetValueOrDefault(id);
        if (ext is null) {
            Bootstrapper.Logger.Error($"Unable to unload extension {id}: not found.", new KeyNotFoundException(id), source: "DESrv.PDK.Extensions.Unload");
            return;
        }

        MethodInfo? methodOnUnload = null;

        foreach (var type in ext.Assembly.DefinedTypes)
            methodOnUnload = type.DeclaredMethods.First(
                m => m.GetCustomAttributes(false).First(
                    a => a is ExtensionOnUnloadAttribute
                    ) is not null
                );
        
        ext.CancellationToken.Cancel();
        ext.Status = ExtensionStatus.Unloaded;
        Bootstrapper.Logger.Success($"Successfully unloaded extension {id}.", source: "DESrv.PDK.Extensions.Unload");
    }

    /// <summary>
    /// Remove extension by ID
    /// </summary>
    /// <param name="id"></param>
    public void RemoveExtension(string id) {
        ExtensionContainer? ext = Extensions?.GetValueOrDefault(id);
        if (ext is null) {
            Bootstrapper.Logger.Error($"Unable to remove extension {id}: not found.", new KeyNotFoundException(id), source: "DESrv.PDK.Extensions.Remove");
            return;
        }
        ext!.CancellationToken.Cancel();
        ext.Status = ExtensionStatus.Unknown;
        Extensions?.Remove(id);

        Bootstrapper.Logger.Success($"Successfully removed extension {id}.", source: "DESrv.PDK.Extensions.Remove");
    }
}
