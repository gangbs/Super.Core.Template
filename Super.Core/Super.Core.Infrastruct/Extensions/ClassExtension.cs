using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Super.Core.Infrastruct.Extensions
{
    public static class ClassExtension
    {
        public static TTarget MapTo<TSource, TTarget>(this TSource obj, bool caseSensitive = true) where TTarget : class, new() where TSource : class
        {
            if (obj == null) return default(TTarget);

            var sPros = typeof(TSource).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var tPros = typeof(TTarget).GetProperties(BindingFlags.Instance | BindingFlags.Public);

            Dictionary<string, object> dic;

            if (caseSensitive)
            {
                dic = (from p in sPros
                       select new KeyValuePair<string, object>(p.Name, p.GetValue(obj))).ToDictionary(x => x.Key, y => y.Value);
            }
            else
            {
                dic = (from p in sPros
                       select new KeyValuePair<string, object>(p.Name.ToUpper(), p.GetValue(obj))).ToDictionary(x => x.Key, y => y.Value);
            }


            var target = (TTarget)Activator.CreateInstance(typeof(TTarget));
            foreach (var p in tPros)
            {

                if (caseSensitive && dic.ContainsKey(p.Name))
                {
                    p.SetValue(target, dic[p.Name], null);
                }
                else if (!caseSensitive && dic.ContainsKey(p.Name.ToUpper()))
                {
                    p.SetValue(target, dic[p.Name.ToUpper()], null);
                }
            }
            return target;
        }

        public static IEnumerable<TTarget> MapTo<TSource, TTarget>(this IEnumerable<TSource> lstSource, bool caseSensitive = true)
            where TTarget : class, new() where TSource : class
        {
            if (lstSource == null) return null;

            var lstTarget = from item in lstSource
                            select item.MapTo<TSource, TTarget>(caseSensitive);
            return lstTarget;
        }

        public static TTarget MapTo<TSource, TTarget>(this TSource obj, Action<TTarget> action, bool caseSensitive = true) where TTarget : class, new() where TSource : class
        {
            var toObj = obj.MapTo<TSource, TTarget>(caseSensitive);
            action(toObj);
            return toObj;
        }

        public static IEnumerable<TTarget> MapTo<TSource, TTarget>(this IEnumerable<TSource> lstSource, Action<TTarget> action, bool caseSensitive = true)
            where TTarget : class, new() where TSource : class
        {
            if (lstSource == null) return null;

            var lstTarget = from item in lstSource
                            select item.MapTo<TSource, TTarget>(action, caseSensitive);
            return lstTarget;
        }

        public static TSelf MapFrom<TSelf, TFrom>(this TSelf selfObj, TFrom fromObj, bool ignoreNull = true, bool caseSensitive = true)
            where TSelf : class where TFrom : class
        {
            if (fromObj == null) return selfObj;
            var fPros = typeof(TFrom).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var sPros = typeof(TSelf).GetProperties(BindingFlags.Instance | BindingFlags.Public);

            Dictionary<string, object> dic;

            if (caseSensitive)
            {
                dic = (from p in fPros
                       select new KeyValuePair<string, object>(p.Name, p.GetValue(fromObj))).ToDictionary(x => x.Key, y => y.Value);
            }
            else
            {
                dic = (from p in fPros
                       select new KeyValuePair<string, object>(p.Name.ToUpper(), p.GetValue(fromObj))).ToDictionary(x => x.Key, y => y.Value);
            }

            foreach (var fp in dic)
            {

                if (ignoreNull && fp.Value == null) continue;

                Func<PropertyInfo, bool> filter;

                if (caseSensitive)
                {
                    filter = x => x.Name == fp.Key;
                }
                else
                {
                    filter = x => x.Name.ToUpper() == fp.Key;
                }

                PropertyInfo sp = sPros.FirstOrDefault(filter);

                if (sp == null) continue;

                sp.SetValue(selfObj, fp.Value, null);
            }

            return selfObj;
        }

    }
}
