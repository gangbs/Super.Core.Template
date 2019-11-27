using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Super.Core.Infrastruct.Configuration
{
    public class JsonConfiguration
    {
        readonly protected IConfiguration _cfg;

        public JsonConfiguration(IConfiguration cfg)
        {
            this._cfg = cfg;
        }

        public JsonConfiguration(string path)
        {
            this._cfg = CreateJsonConfig(path);
        }

        public static IConfiguration CreateJsonConfig(string path)
        {
            var builder = new ConfigurationBuilder().AddJsonFile(path, optional: true, reloadOnChange: true);//能否不指定
            var configuration = builder.Build();
            return configuration;
        }

        #region 工具方法

        public IConfiguration Cfg
        {
            get
            {
                return this._cfg;
            }
        }

        public string GetValue(string key)
        {
            return this._cfg[key];
        }

        public T GetValue<T>(string key)
        {
            return this._cfg.GetValue<T>(key);
        }

        public T GetValue<T>(string key, T def)
        {
            return this._cfg.GetValue<T>(key, def);
        }

        public T GetModel<T>(string key) where T : class, new()
        {
            Type tp = typeof(T);
            var obj = (T)tp.Assembly.CreateInstance(tp.FullName);
            this._cfg.GetSection(key).Bind(obj);
            return obj;
        }

        #endregion
    }
}
