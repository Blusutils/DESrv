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
public static class Bootstrapper {

    //static Bootstrapper () {
    //    // Setting AppDomain references resolver
    //    var domain = AppDomain.CurrentDomain;
    //    domain.AssemblyResolve += PdkLoader.ResolveAssembly;
    //}

    /// <summary>
    /// Thread manager
    /// </summary>
    public static ThreadManager? Threader { get; set; } = new();
    /// <summary>
    /// Version of DESrv
    /// </summary>
    public static Version DESrvVersion { get; } = Versions.Versions.DESrvVersion;
    /// <summary>
    /// Localization manager
    /// </summary>
    public static LocalizationProvider? Localization { get; set; } = null;
    /// <summary>
    /// Logger
    /// </summary>
    public static Logger Logger { get; set; } = new();

    /// <summary>
    /// Plugin loader
    /// </summary>
    public static PdkLoader PdkLoader { get; set; } = new();

    /// <summary>
    /// Start DESrv
    /// </summary>
    /// <param name="cancellationToken"></param>
    public static void Start(CancellationTokenSource cancellationToken) {

        var sw = new Stopwatch();

        Logger.Info("DESrv starting...");

        if (DESrvConfig.Instance.autoCheckUpdates && Updater.Updater.CheckVersion(DESrvVersion) is Version nver) {
            Logger.Warn($"A new version v{nver} is available! {(!DESrvConfig.Instance.autoUpdate ? "Download it on https://github.com/Blusutils/DESrv/releases/latest" : "")}", source: "DESrv.Updater");
            if (DESrvConfig.Instance.autoUpdate) {
                Logger.Notice("Update starting...", source: "DESrv.Updater");
                Updater.Updater.Update();
                Logger.Success("Update downloaded, restarting DESrv!", source: "DESrv.Updater");

                var fileName = AppDomain.CurrentDomain.FriendlyName;
                var arguments = Environment.GetCommandLineArgs();
                arguments[0] = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DESrv.exe");

                var processInfo = new ProcessStartInfo {
                    FileName = fileName,
                    Arguments = string.Join(" ", arguments),
                    UseShellExecute = true
                };
                Process.Start(processInfo);
                Environment.Exit(0);
            }
        }

        #region PDK Loader
        Logger.Debug($"Trying to load extensions from {DESrvConfig.Instance?.extensionsDir}.", source: "DESrv.PDK.Extensions");

        if (DESrvConfig.Instance?.extensionsDir is null) {
            Logger.Fatal($"Extensions directory is not set, exiting.", source: "DESrv.PDK.Extensions.Add");
            Environment.Exit(1);
        }

        if (!Directory.Exists(DESrvConfig.Instance.extensionsDir))
            Directory.CreateDirectory(DESrvConfig.Instance.extensionsDir);

        PdkLoader.LoadFrom = DESrvConfig.Instance.extensionsDir;
        sw.Start();

        try {
            PdkLoader.AddExtensionsFromDirectory();
        } catch (Exception ex) {
            Logger.Fatal("Something went wrong when adding extensions.", ex, source: "DESrv.PDK.Extensions.Add");
            Environment.Exit(1);
        }

        foreach (var ext in PdkLoader.Extensions) {
            Logger.Debug($"Trying to load extension {ext.Key}.", source: "DESrv.PDK.Extensions.Load");
            var extm = PdkLoader.LoadExtension(ext.Key);
            if (extm is null) {
                Logger.Error($"Extension {ext.Key} is null.", new NullReferenceException(ext.Key), source: "DESrv.PDK.Extensions.Load");
                continue;
            }
        }

        sw.Stop();
        Logger.Debug($"Loading extensions took {sw.ElapsedMilliseconds}ms.", source: "DESrv.PDK.Extensions");
        Logger.Success($"Loaded {PdkLoader.Extensions.Count(kv => kv.Value.Status is ExtensionStatus.Loaded or ExtensionStatus.LoadedAsChildren)} extensions.", source: "DESrv.PDK.Extensions");
        #endregion


        //ThreadManager.QueueSingletonThread(() => SimultaneousConsole.StartRead());
        //new Thread(() => {
        //    while (cancellationToken.IsCancellationRequested) {
        //        // And so, what we will do here?
        //    }
        //}).Start();
        while (true) {}
        Logger.Info("DESrv stopping...");
        Console.WriteLine("Press any key to continue.");
        Console.Read();
    }
}