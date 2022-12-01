namespace DESrv.Utils {
    /// <summary>
    /// Commandline arguments parser
    /// </summary>
    static class ArgParser {
        /// <summary>
        /// Parse commandline arguments
        /// </summary>
        /// <param name="source">Array of commandline args</param>
        /// <returns><see cref="Dictionary{string, object}"/> (string, string) with parsed arguments</returns>
        public static Dictionary<string, object> Parse(string[] source) {
            Dictionary<string, object> result = new Dictionary<string, object> {
                { "nocategory", "" }
            };
            string? last = null;
            foreach (var elem in source) {
                if (elem.StartsWith('-')) {
                    last = elem.Substring(1);
                    result.Add(last, "");
                } else {
                    result.TryGetValue(last ?? "nocategory", out object val);
                    result[last ?? "nocategory"] = val.ToString() + " " + elem;
                }
            }
            return result;
        }
    }
}
