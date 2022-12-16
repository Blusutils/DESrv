namespace DESCEnd.TextUtils {
    /// <summary>
    /// Advanced (and better) keyword-based format (with extension methods)
    /// </summary>
    public static class AdvFormat {
        /// <summary>
        /// Format string from dict with keywords
        /// </summary>
        /// <param name="srcStr">Source string (string to format)</param>
        /// <param name="format">Dictionary, where keys is keyword for format, and values is values!</param>
        /// <param name="strict">Need to throw exception if formattion failed?</param>
        /// <returns>Formatted string</returns>
        /// <exception cref="FormatException">When formattion fails</exception>
        public static string Format(this string srcStr, Dictionary<string, object> format, bool strict = false) {
            if (srcStr == null || srcStr == "") throw new FormatException("unable to format empty string");
            foreach (var k in format.Keys)
                try {
                    srcStr = srcStr.Replace($@"{{{k}}}", format[k].ToString());
                } catch (ArgumentNullException e) {
                    if (strict) throw new FormatException($@"unable to format string with null key", e);
                } catch (ArgumentException e) {
                    if (strict) throw new FormatException($@"unable to format string with {{{k}}} key", e);
                }
            return srcStr;
        }
        /// <summary>
        /// Classic <see cref="string.Format(string, object?[])"/>
        /// </summary>
        /// <param name="srcStr">Source string (string to format)</param>
        /// <param name="formats">An object array that contains zero or more objects to format</param>
        /// <returns>Formatted string</returns>
        /// <exception cref="FormatException">When formattion fails</exception>
        /// <exception cref="ArgumentNullException">Source string or args is null</exception>
        public static string Format(this string srcStr, params object[] formats) {
            return string.Format(srcStr, formats);
        }
    }
}