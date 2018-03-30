using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cogslite
{
    public static class PageHelper
    {
        public static string WhenContains(this IDictionary<string, object> dictionary, string key, string positiveResult, string negativeResult = "")
        {
            if (dictionary.ContainsKey(key) && dictionary[key] != null)
                return positiveResult;
            return negativeResult;
        }

    }
}
