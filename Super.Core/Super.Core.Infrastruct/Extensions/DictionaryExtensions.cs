using System;
using System.Collections.Generic;
using System.Text;

namespace Super.Core.Infrastruct.Extensions
{
    public static class DictionaryExtensions
    {
        public static bool TryGetValue<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, TValue defaultVal, out TValue value)
        {
            value = defaultVal;
            if (dic == null) return false;

            if (dic.TryGetValue(key, out value))
            {
                return true;
            }
            else
            {
                value = defaultVal;
                return false;
            }
        }
    }
}
