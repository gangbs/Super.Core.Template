using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Core.Mvc
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

        public ILogger GetLogger<T>()
        {
            var logger = this._loggerFactory.CreateLogger<T>();
            return logger;
        }

        public ILogger GetLogger(string category)
        {
            var logger = this._loggerFactory.CreateLogger(category);
            return logger;
        }

    }
}
