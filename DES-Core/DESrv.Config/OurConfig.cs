using DESCEnd.Config;
#pragma warning disable IDE1006
namespace DESrv.Config {
    public class OurConfig : ConfigurationModel {
        public string? ipAdress { get; set; }
        public int? logLevel { get; set; }
        public string[]? extsToLoad { get; set; }
        public string? prefferedRandom { get; set; }
    }
}
#pragma warning restore IDE1006