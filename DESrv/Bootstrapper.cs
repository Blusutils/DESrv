﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blusutils.DESrv.Configuration;
using Blusutils.DESrv.Localization;
using Blusutils.DESrv.Logging;

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
    /// <summary>
    /// Command processor from <see cref="SimultaneousConsole"/>
    /// </summary>
    public ICommandInputProcessor? CommandInputProcessor { get; set; }
    public void Start(CancellationToken cancellationToken) {
        if (Threader == null)
            throw new NullReferenceException("Threader is null, set it in bootstrapper initializer");
        if (Logger == null)
            throw new NullReferenceException("Logger is null, set it in bootstrapper initializer");
        if (Localization == null)
            throw new NullReferenceException("Localizer is null, set it in bootstrapper initializer");
        if (DESrvVersion == null)
            throw new NullReferenceException("DESrv version is null, set it in bootstrapper initializer");
        if (CommandInputProcessor == null)
            CommandInputProcessor = new CommandInputProcessor();

        Logger.Info("DESrv starting...");

        //ThreadManager.QueueSingletonThread(() => SimultaneousConsole.StartRead());
        if (ConsoleService.Console is SimultaneousConsole simc) // shitty dependency injection to prevent bugs with docker; TODO remote command sending API
            new Thread(() => {
                while (cancellationToken.IsCancellationRequested) {
                    CommandInputProcessor.Process(simc.ReadLine());
                }
            }).Start();
    }
}
