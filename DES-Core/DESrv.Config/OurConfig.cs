﻿using DESCEnd.Config;
#pragma warning disable IDE1006
namespace DESrv.Config {
    public class OurConfig : ConfigurationModel {
        public string? ipAdress { get; set; }
        public int? logLevel { get; set; }
        public string[]? extensionsToLoad { get; set; }
        public string? randomMode { get; set; }
        public string? mainExtension { get; set; }
    }
}
#pragma warning restore IDE1006