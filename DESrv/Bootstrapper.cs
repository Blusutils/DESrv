using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blusutils.DESrv.Localization;
using Blusutils.DESrv.Logging;

namespace Blusutils.DESrv {
    public class Bootstrapper {
        public Version? DESrvVersion { get; set; } = null;
        public Localizer? Localization { get; set; } = null;
        public Logger? Logger { get; set; } = null;
        public void Start() {
            if (Logger == null) { throw new NullReferenceException("Logger is null, set it in bootstrapper initializer"); }
            if (Localization == null) { throw new NullReferenceException("Localizer is null, set it in bootstrapper initializer"); }
            if (DESrvVersion == null) { throw new NullReferenceException("DESrv version is null, set it in bootstrapper initializer"); }
            
        }
    }
}
