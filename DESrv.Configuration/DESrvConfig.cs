using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Blusutils.DESrv.Configuration;
using Blusutils.DESrv.Logging;

namespace Blusutils.DESrv.Configuration; 

/// <summary>
/// DESrv config
/// </summary>
public class DESrvConfig : ConfigurationModel {

    /// <summary>
    /// Config instance
    /// </summary>
    public static DESrvConfig? Instance { get; set; } = null;

    /// <summary>
    /// Is project built with development configuration
    /// </summary>
    public bool isDevelopment { get; set; } =
        #if DEBUG
                    true;
        #else
                    false;
        #endif

    /// <summary>
    /// Global locale code used in translations
    /// </summary>
    public string locale { get; set; } = "en";

    /// <summary>
    /// Is Updater module will check updates on every server startup
    /// </summary>
    public bool autoCheckUpdates { get; set; } = true;

    /// <summary>
    /// Is Updater module will do updates on every server startup
    /// </summary>
    public bool autoUpdate { get; set; } = false;

    /// <summary>
    /// Default host IP address
    /// </summary>
    public IPAddress? ipAddress { get; set; } = new(new byte[] { 127, 0, 0, 1 });

    /// <summary>
    /// Logging level
    /// </summary>
    public LogLevel logLevel { get; set; } = LogLevel.Debug;

    /// <summary>
    /// Logging level project built with development configuration
    /// </summary>
    public LogLevel logLevelDevelopment { get; set; } = LogLevel.Debug;

    /// <summary>
    /// Is logging to console enabled
    /// </summary>
    public bool useConsoleLogging { get; set; } = true;

    /// <summary>
    /// Is logging to files enabled
    /// </summary>
    public bool useFileLogging { get; set; } = true;

    /// <summary>
    /// Is secure versions of protocols preferred
    /// </summary>
    public bool? preferSecure { get; set; } = false;

    /// <summary>
    /// Directory to load extensions froms
    /// </summary>
    public string? extensionsDir { get; set; } = "./extensions";

    /// <summary>
    /// How deep do need to look for extensions
    /// </summary>
    public int? extensionsFindDepth { get; set; } = 2;

    /// <summary>
    /// Extensions allowed to load
    /// </summary>
    public string[]? extensionsWhitelist { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Extesnion that will capture console input stream after Logman startup
    /// </summary>
    public string? mainExtension { get; set; } = "DESrvInternal";

    /// <summary>
    /// Directory that contains shared libs
    /// </summary>
    public string? sharedLibsDir { get; set; } = "./libs-shared";

    /// <summary>
    /// Directory that contains static content
    /// </summary>
    public string? staticResourcesDir { get; set; } = "./static-rsrc";
}
