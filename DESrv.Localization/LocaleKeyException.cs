using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blusutils.DESrv.Localization {
    /// <summary>
    /// Exception what raises when key not found during getting localization string
    /// </summary>
    public class LocaleKeyException : Exception {
        /// <summary>
        /// Create new <see cref="LocaleKeyException"/>
        /// </summary>
        /// <param name="key">Key what is unable to find</param>
        /// <param name="isFromListing">Is this exception raises from translation string list (dict)?</param>
        /// <param name="fromExc">Exception what caused this exception</param>
        public LocaleKeyException(string key, bool isFromListing = true, Exception? fromExc = null)
            : base($"Key `{key}` not found in {(isFromListing ? "localization keys" : "locale list")}", fromExc) { }
    }
}
