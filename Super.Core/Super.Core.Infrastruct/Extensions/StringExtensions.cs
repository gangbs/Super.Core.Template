using System;
using System.Collections.Generic;
using System.Text;

namespace Super.Core.Infrastruct.Extensions
{
    public static class StringExtensions
    {
        public static string Byte2String(this byte[] array)
        {
            return Encoding.UTF8.GetString(array);
        }

        public static byte[] String2Byte(this string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }


        public static string ToBase64(this string content)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(content);
            return Convert.ToBase64String(bytes);
        }

        public static string FromBase64(this string base64)
        {
            byte[] bytes = Convert.FromBase64String(base64);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
