using System;
using System.Collections.Generic;
using System.Text;

namespace Super.Core.Infrastruct.Configuration
{
    public class AppConfiguration : JsonConfiguration
    {
        private readonly static object _locker = new object();
        private static AppConfiguration _instance;
        public static AppConfiguration Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null)
                            _instance = new AppConfiguration();
                    }
                }
                return _instance;
            }
        }

        private AppConfiguration() : base("Config/appsettings.json")
        {
        }
    }
}
