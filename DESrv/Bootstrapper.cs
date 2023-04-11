using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blusutils.DESrv.Configuration;
using Blusutils.DESrv.Localization;
using Blusutils.DESrv.Logging;

namespace Blusutils.DESrv {
    /// <summary>
    /// DESrv server Bootstrapper
    /// </summary>
    public sealed class Bootstrapper {
        /// <summary>
        /// Thread manager
        /// </summary>
        public Threader? Threader { get; set; } = null;
        /// <summary>
        /// Version of DESrv
        /// </summary>
        public Version? DESrvVersion { get; set; } = null;
        /// <summary>
        /// Localization manager
        /// </summary>
        public Localizer? Localization { get; set; } = null;
        /// <summary>
        /// Logger
        /// </summary>
        public Logger? Logger { get; set; } = null;
        /// <summary>
        /// Command processor from <see cref="SimultaneousConsole"/>
        /// </summary>
        public ICommandInputProcessor? CommandInputProcessor { get; set; }
        public void Start() {
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

            //Threader.QueueSingletonThread(() => SimultaneousConsole.StartRead());
            new Thread(() => {
                while (true) {
                    CommandInputProcessor.Process(SimultaneousConsole.ReadLine());
                }
            }).Start();
        }
    }
}
