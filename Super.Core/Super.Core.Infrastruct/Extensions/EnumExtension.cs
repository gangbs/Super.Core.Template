using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Super.Core.Infrastruct.Extensions
{
    public static class EnumExtension
    {
        /// <summary>
        /// 读取枚举类型描述信息
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="item">枚举对象</param>
        /// <returns></returns>
        public static string GetDescribeInfo<T>(this T item) 
        {
            var type = item.GetType();
            FieldInfo field = type.GetField(item.ToString(), BindingFlags.Public | BindingFlags.Static);
            var query = from attribute in field.GetCustomAttributes(typeof(DescriptionAttribute), false) select ((DescriptionAttribute)attribute);
            return query.FirstOrDefault()?.Description;
        }

        /// <summary>
        /// 将枚举转换成字典
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <returns></returns>
        public static Dictionary<int, string> ToDictionary<T>()
        {
            var a = typeof(T);
            var dic = new Dictionary<int, string>();
            if (a.IsEnum)
            {
                foreach (var b in Enum.GetValues(a))
                {

                    var d = EnumExtension.GetDescribeInfo((T)Enum.Parse(a, b.ToString()));
                    dic.Add((int)b, d);
                }
            }
            return dic;
        }
    }
}
