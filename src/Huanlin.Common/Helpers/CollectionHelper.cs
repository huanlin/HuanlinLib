using System;
using System.Collections.Generic;
using System.Text;

namespace Huanlin.Common.Helpers
{
    public static class CollectionHelper
    {
        public static string DictionaryToString(IDictionary<string, string> items, string keyValueDelimiter, string keyValuePairDelimiter)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string key in items.Keys)
            {
                sb.Append(key);
                sb.Append(keyValueDelimiter);
                sb.Append(items[key]);
                sb.Append(keyValuePairDelimiter);
            }
            return sb.ToString();
        }
    }
}