using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Blusutils.DESrv.Configuration;
using Blusutils.DESrv.Localization;
using Blusutils.DESrv.Logging;
using Blusutils.DESrv.PDK;
using Blusutils.DESrv.Threader;

namespace Blusutils.DESrv;

/// <summary>
/// DESrv server Bootstrapper
/// </summary>
public sealed class Bootstrapper {
    /// <summary>
    /// Thread manager
    /// </summary>
    public ThreadManager? Threader { get; set; } = null;
    /// <summary>
    /// Version of DESrv
    /// </summary>
    public Version? DESrvVersion { get; set; } = null;
    /// <summary>
    /// Localization manager
    /// </summary>
    public LocalizationProvider? Localization { get; set; } = null;
    /// <summary>
    /// Logger
    /// </summary>
    public Logger? Logger { get; set; } = null;

    public PdkLoader PdkLoader { get; set; } = new PdkLoader();

    public void Start(CancellationToken cancellationToken) {
        if (Threader == null)
            throw new NullReferenceException("Threader is null, set it in bootstrapper initializer");
        if (Logger == null)
            throw new NullReferenceException("Logger is null, set it in bootstrapper initializer");
        if (Localization == null)
            throw new NullReferenceException("Localizer is null, set it in bootstrapper initializer");
        if (DESrvVersion == null)
            throw new NullReferenceException("DESrv version is null, set it in bootstrapper initializer");

        var sw = new Stopwatch();

        Logger.Info("DESrv starting...");

        Logger.Debug($"Trying to load extensions from {DESrvConfig.Instance?.extensionsDir}");

        if (DESrvConfig.Instance?.extensionsDir is null) {
            Logger.Fatal($"Extensions directory is not set, exiting");
            Environment.Exit(1);
        }

        PdkLoader.LoadFrom = DESrvConfig.Instance.extensionsDir;
        sw.Start();

        try {
            PdkLoader.AddExtensionsFromDirectory();
        } catch (Exception ex) {
            Logger.Fatal($"Something went wrong when adding extensions.", ex);
            Environment.Exit(1);
        }

        foreach (var ext in PdkLoader.Extensions) {

            var extm = PdkLoader.LoadExtension(ext.Key);
            if (extm is null) {
                Logger.Error($"Extension {ext.Key} is null");
                continue;
            }
            var ver = extm.Metadata.TargetDESrvVersion;
            var id = extm.Metadata.ID;

            if (DESrvConfig.Instance.extensionsWhitelist is not null && !DESrvConfig.Instance.extensionsWhitelist.Contains(id)) {
                Logger.Error($"Extension {id} is not in whitelist");
                extm.Status = ExtensionStatus.Failed;
                continue;
            }

            if (ver.Major != DESrvVersion.Major || ver.Minor > DESrvVersion.Minor) {
                Logger.Error($"Extension {id} ({ver}) is incompatable with current DESrv version ({DESrvVersion})");
                extm.Status = ExtensionStatus.Failed;
                continue;
            }

            // TODO process references
            //extm.Metadata.RefersTo;
            //extm.Metadata.Dependencies;
            extm.Status = ExtensionStatus.Loaded;
            try {

                MethodInfo method = null;
                foreach (var type in extm.Assembly.DefinedTypes) {
                    method = type.DeclaredMethods.First(m => m.GetCustomAttributes(false).First(a => a is ExtensionEntrypointAttribute) is not null);
                }

                new Thread(() => { // TODO threader
                    while (extm.CancellationToken.IsCancellationRequested) {
                        method?.Invoke(extm.Instance, null);
                    }
                }).Start();

            } catch (Exception ex) {
                Logger.Error($"Something went wrong during {ext.Key} extension execution.", ex);
            }
        }

        sw.Stop();
        Logger.Debug($"Adding extensions took 10 seconds {sw.ElapsedMilliseconds}");
        Logger.Success($"Added {PdkLoader.Extensions.Count(kv => kv.Value.Status is ExtensionStatus.Loaded or ExtensionStatus.Shared or ExtensionStatus.LoadedAsChildren)} extensions");

        //ThreadManager.QueueSingletonThread(() => SimultaneousConsole.StartRead());
        //new Thread(() => {
        //    while (cancellationToken.IsCancellationRequested) {
        //        // And so, what we will do here?
        //    }
        //}).Start();
    }
}