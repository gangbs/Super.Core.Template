using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Super.Core.Infrastruct.Extensions
{
    public static class XmlExtensions
    {
        /// <summary>
        /// 对象Xml序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjectToXml<T>(this T obj) where T : class
        {
            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(stream, obj);
                stream.Position = 0;
                using (StreamReader sr = new StreamReader(stream))
                {
                    string xml = sr.ReadToEnd();
                    return xml;
                }
            }
        }

        /// <summary>
        /// xml反序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T XmlToObject<T>(this string xml) where T : class
        {
            using (StringReader sr = new StringReader(xml))
            {
                XmlSerializer s = new XmlSerializer(typeof(T));
                return s.Deserialize(sr) as T;
            }
        }
    }
}
