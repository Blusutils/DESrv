using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DESCore.Utils
{
    static class ArgParser
    {
        public static Dictionary<string, string> Parse(string[] source)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("nocategory", "");
            string? last = null;
            foreach (var elem in source)
            {
                if (elem.StartsWith('-'))
                {
                    last = elem.Substring(1);
                    result.Add(last, "");
                } else
                {
                    result.TryGetValue(last??"nocategory", out string val);
                    result[last??"nocategory"] = val.ToString() + " " + elem;
                }
            }
            return result;
        }
    }
}
