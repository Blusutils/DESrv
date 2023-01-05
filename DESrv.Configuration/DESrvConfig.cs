using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Blusutils.DESrv.Configuration;
using Blusutils.DESrv.Logging;

namespace Blusutils.DESrv.Configuration {
    public class DESrvConfig : ConfigurationModel {
        public static DESrvConfig? Instance { get; set; } = null;
        public bool IsDevelopment { get; set; } =
            #if DEBUG
                        true;
            #else
                        false;
            #endif
        public bool AutoCheckUpdates { get; set; } = false;
        public bool AutoUpdate { get; set; } = false;
        public string Locale { get; set; } = "en-US";
        public LogLevel LogLevel { get; set; } = LogLevel.Debug;
        public LogLevel LogLevelDevelopment { get; set; } = LogLevel.Debug;
        public bool UseConsoleLogging { get; set; } = true;
        public bool UseFileLogging { get; set; } = true;
        public IPAddress? HostIP { get; set; } = new(new byte [] { 127, 0, 0, 1 });
        public bool? PreferSecure { get; set; } = false;
        public string[]? ExtensionsList { get; set; } = null;
        public string? MainExtension { get; set; } = null;
        public bool? AllowStdioListeners { get; set; } = false;
    }
}
