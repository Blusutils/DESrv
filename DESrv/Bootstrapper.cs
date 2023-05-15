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
using Blusutils.DESrv.Updater;

namespace Blusutils.DESrv;

/// <summary>
/// DESrv server Bootstrapper
/// </summary>
public sealed class Bootstrapper {

    //static Bootstrapper () {
    //    // Setting AppDomain references resolver
    //    var domain = AppDomain.CurrentDomain;
    //    domain.AssemblyResolve += PdkLoader.ResolveAssembly;
    //}

    /// <summary>
    /// Thread manager
    /// </summary>
    public ThreadManager? Threader { get; set; } = null;
    /// <summary>
    /// Version of DESrv
    /// </summary>
    public static Version DESrvVersion { get; set; } = new(1,0,0);
    /// <summary>
    /// Localization manager
    /// </summary>
    public LocalizationProvider? Localization { get; set; } = null;
    /// <summary>
    /// Logger
    /// </summary>
    public static Logger Logger { get; set; }

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

        if (DESrvConfig.Instance.autoCheckUpdates && Updater.Updater.CheckVersion(DESrvVersion) is Version nver) {
            Logger.Warn($"A new version v{nver} is available! {(!DESrvConfig.Instance.autoUpdate ? "Download it on https://github.com/Blusutils/DESrv/releases/latest" : "")}");
            if (DESrvConfig.Instance.autoUpdate) {
                Logger.Notice("Update starting...");
                Updater.Updater.Update();
            }
        }

        #region PDK Loader
        Logger.Debug($"Trying to load extensions from {DESrvConfig.Instance?.extensionsDir}");

        if (DESrvConfig.Instance?.extensionsDir is null) {
            Logger.Fatal($"Extensions directory is not set, exiting");
            Environment.Exit(1);
        }

        if (!Directory.Exists(DESrvConfig.Instance.extensionsDir))
            Directory.CreateDirectory(DESrvConfig.Instance.extensionsDir);

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
            } else if (extm.Status == ExtensionStatus.Failed) {
                Logger.Error($"Extension {ext.Key} failed");
                continue;
            }
            
        }

        sw.Stop();
        Logger.Debug($"Adding extensions took {sw.ElapsedMilliseconds}ms");
        Logger.Success($"Added {PdkLoader.Extensions.Count(kv => kv.Value.Status is ExtensionStatus.Loaded or ExtensionStatus.Shared or ExtensionStatus.LoadedAsChildren)} extensions");
        #endregion


        //ThreadManager.QueueSingletonThread(() => SimultaneousConsole.StartRead());
        //new Thread(() => {
        //    while (cancellationToken.IsCancellationRequested) {
        //        // And so, what we will do here?
        //    }
        //}).Start();
    }
}