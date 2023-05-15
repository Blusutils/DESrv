using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Blusutils.DESrv.Configuration;
using Blusutils.DESrv.Logging;
using Blusutils.DESrv.PDK;

namespace Blusutils.DESrv;



/// <summary>
/// Extension loader for Plugin DeVelopment Kit (PDK)
/// </summary>
public class PdkLoader { // TODO implement pdkloader

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
        var asm = Assembly.LoadFile(pathToExtension);
        if (asm is null) return;

        ExtensionContainer ext = new() {
            Assembly = asm,
            CancellationToken = new CancellationTokenSource(),
            Metadata = null,
            Status = ExtensionStatus.Unknown,
            Instance = null
        };

        var exts = asm.GetTypes().Where(t => t.GetCustomAttribute(typeof(PDKExtensionAttribute)) is not null).ToArray();

        foreach (var extType in exts) {
            ext.Metadata = (extType.GetCustomAttribute(typeof(PDKExtensionAttribute)) as PDKExtensionAttribute)!.Metadata;
            ext.Instance = Activator.CreateInstance(extType);
            ext.Status = ExtensionStatus.Found;
        }
        if (ext.Instance is not null)
            Extensions.Add(pathToExtension, ext);
        else
            ext = null;
    }

    /// <summary>
    /// Adds all extensions from default directory
    /// </summary>
    public void AddExtensionsFromDirectory() {
        AddExtensionsFromDirectory(LoadFrom??DESrvConfig.Instance?.extensionsDir??"");
    }

    /// <summary>
    /// Adds all extensions from specified directory
    /// </summary>
    /// <param name="pathToExtensions">Path to directory</param>
    public void AddExtensionsFromDirectory(string pathToExtensions) {
        foreach (var file in  Directory.GetFiles(pathToExtensions)) {
            AddExtension(file);
        }
    }

    /// <summary>
    /// Load extension by ID
    /// </summary>
    /// <param name="id"></param>
    public ExtensionContainer? LoadExtension(string id) {
        var ext = Extensions?[id];
        if (ext is null) return null;

        if (ext.Status is ExtensionStatus.Unknown) {
            var ver = ext.Metadata.TargetDESrvVersion;

            if (DESrvConfig.Instance?.extensionsWhitelist is not null && !DESrvConfig.Instance.extensionsWhitelist.Contains(id)) {
                Bootstrapper.Logger.Error($"Extension {id} is not in whitelist");
                ext.Status = ExtensionStatus.Failed;
            }

            if (ver.Major != Bootstrapper.DESrvVersion.Major || ver.Minor > Bootstrapper.DESrvVersion.Minor) {
                Bootstrapper.Logger.Error($"Extension {id} ({ver}) is incompatable with current DESrv version ({Bootstrapper.DESrvVersion})");
                ext.Status = ExtensionStatus.Failed;
            }

            // TODO process references
            //extm.Metadata.RefersTo;
            //extm.Metadata.Dependencies;

            try {

                MethodInfo? methodEntrypoint = null;
                MethodInfo? methodOnload = null;

                foreach (var type in ext.Assembly.DefinedTypes) {
                    methodEntrypoint = type.DeclaredMethods.First(m => m.GetCustomAttributes(false).First(a => a is ExtensionEntrypointAttribute) is not null);
                    methodOnload = type.DeclaredMethods.First(m => m.GetCustomAttributes(false).First(a => a is ExtensionOnLoadAttribute) is not null);
                    
                }

                new Thread(() => { // TODO threader
                    while (ext.CancellationToken.Token.IsCancellationRequested) {
                        methodEntrypoint?.Invoke(ext.Instance, null);
                    }
                }).Start();

                new Thread(() => { // TODO threader
                        methodOnload?.Invoke(ext.Instance, new object[] { this, new ExtensionLoadedEventArgs(ext) });
                }).Start();

                ext.Status = ExtensionStatus.Loaded;

            } catch (Exception ex) {
                Bootstrapper.Logger.Error($"Something went wrong during {id} extension execution.", ex);
                ext.Status = ExtensionStatus.Failed;
            }
        } else if (ext.Status is ExtensionStatus.Suspended or ExtensionStatus.Found) {
            MethodInfo? methodOnload = null;

            foreach (var type in ext.Assembly.DefinedTypes) {
                methodOnload = type.DeclaredMethods.First(m => m.GetCustomAttributes(false).First(a => a is ExtensionOnLoadAttribute) is not null);
            }
            new Thread(() => { // TODO threader
                methodOnload?.Invoke(ext.Instance, new object[] { this, new ExtensionLoadedEventArgs(ext) });
            }).Start();

            ext.Status = ExtensionStatus.Loaded;
        }
        return ext;
    }

    /// <summary>
    /// Unoad extension by ID
    /// </summary>
    /// <param name="id"></param>
    public void UnloadExtension(string id) { /*ExtensionStatus.Loaded or ExtensionStatus.Shared or ExtensionStatus.LoadedAsChildren*/ }

    /// <summary>
    /// Suspend extension by ID
    /// </summary>
    /// <param name="id"></param>
    public void SuspendExtension(string id) { }

    /// <summary>
    /// Remove extension by ID
    /// </summary>
    /// <param name="id"></param>
    public void RemoveExtension(string id) { }

    //public static Assembly ResolveAssembly(object? sender, ResolveEventArgs args) {
    //    return Assembly.Load(args.Name);
    //}
}
