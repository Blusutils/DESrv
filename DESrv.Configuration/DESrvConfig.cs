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
        public bool isDevelopment { get; set; } =
            #if DEBUG
                        true;
            #else
                        false;
#endif
        public string locale { get; set; } = "en-US";
        public bool autoCheckUpdates { get; set; } = true;
        public bool autoUpdate { get; set; } = false;
        public IPAddress? ipAddress { get; set; } = new(new byte[] { 127, 0, 0, 1 });
        public LogLevel logLevel { get; set; } = LogLevel.Debug;
        public LogLevel logLevelDevelopment { get; set; } = LogLevel.Debug;
        public bool useConsoleLogging { get; set; } = true;
        public bool useFileLogging { get; set; } = true;
        public ConsoleControlSequence? simControlKeys { get; set; } = new();
        public bool? preferSecure { get; set; } = false;
        public string? extensionsDir { get; set; } = "./extensions";
        public int? extensionsFindDepth { get; set; } = 2;
        public string[]? extensionsWhitelist { get; set; } = Array.Empty<string>();
        public string? mainExtension { get; set; } = "DESrvInternal";
        public string? sharedLibsDir { get; set; } = "./libs-shared";
        public int? sharedLibsFindDepth { get; set; } = 2;
        public string? staticResourcesDir { get; set; } = "./static-rsrc";
    }
}
