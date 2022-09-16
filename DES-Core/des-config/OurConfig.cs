using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DESCEnd.Config;

namespace DESCore {
    public class OurConfig : ConfigurationModel {
        public string ipAdress { get; set; }
        public int logLevel { get; set; }
        public string[] extsToLoad { get; set; }
    }
}
