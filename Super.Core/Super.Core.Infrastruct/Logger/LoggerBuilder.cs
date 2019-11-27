using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Super.Core.Infrastruct.Logger
{
    public class LoggerBuilder
    {
        private static LoggerBuilder _instance = new LoggerBuilder();
        private LoggerBuilder() { }
        public static LoggerBuilder Instance
        {
            get
            {
                return _instance;
            }
        }

        private ILoggerFactory _loggerFactory;
        public void Register(ILoggerFactory loggerFactory)
        {
            this._loggerFactory = loggerFactory;
        }

        public ILogger Build<T>()
        {
            var logger = this._loggerFactory.CreateLogger<T>();
            return logger;
        }

        public ILogger Build(string category)
        {
            var logger = this._loggerFactory.CreateLogger(category);
            return logger;
        }

    }
}
