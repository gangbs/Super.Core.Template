using log4net;
using log4net.Config;
using log4net.Core;
using log4net.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Super.Core.Infrastruct.Logger
{
    public class Log4NetConfig
    {
        public const string RepositoryName = "SuperLogRepository";
        private static ILoggerRepository _repository;
        private static Log4NetConfig _instance = new Log4NetConfig();
        public static Log4NetConfig Instance
        {
            get
            {
                return _instance;
            }
        }
        private Log4NetConfig() { }

        public void ConfigureAndWatch(string cfgFile)
        {
            if (Log4NetConfig._repository == null) CreateRepository(RepositoryName);
            XmlConfigurator.ConfigureAndWatch(Log4NetConfig._repository, new FileInfo(cfgFile));
        }

        private void CreateRepository(string name)
        {
            _repository = LogManager.CreateRepository(name);
        }
    }


    public class Log4NetProxy : ILog
    {
        private ILog _logger;

        public Log4NetProxy(Type tp)
        {
            this._logger = LogManager.GetLogger(Log4NetConfig.RepositoryName, tp);
        }

        public Log4NetProxy(string loggerName)
        {
            this._logger = LogManager.GetLogger(Log4NetConfig.RepositoryName, loggerName);
        }

        public ILogger Logger => this._logger.Logger;

        public bool IsDebugEnabled => this._logger.IsDebugEnabled;

        public bool IsInfoEnabled => this._logger.IsInfoEnabled;

        public bool IsWarnEnabled => this._logger.IsWarnEnabled;

        public bool IsErrorEnabled => this._logger.IsErrorEnabled;

        public bool IsFatalEnabled => this._logger.IsFatalEnabled;

        public void Debug(object message)
        {
            this._logger.Debug(message);
        }

        public void Debug(object message, Exception exception)
        {
            this._logger.Debug(message, exception);
        }

        public void DebugFormat(string format, params object[] args)
        {
            this._logger.DebugFormat(format, args);
        }

        public void DebugFormat(string format, object arg0)
        {
            this._logger.DebugFormat(format, arg0);
        }

        public void DebugFormat(string format, object arg0, object arg1)
        {
            this._logger.DebugFormat(format, arg0, arg1);
        }

        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            this._logger.DebugFormat(format, arg0, arg1, arg2);
        }

        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            this._logger.DebugFormat(provider, format, args);
        }

        public void Error(object message)
        {
            this._logger.Error(message);
        }

        public void Error(object message, Exception exception)
        {
            this._logger.Error(message, exception);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            this._logger.ErrorFormat(format, args);
        }

        public void ErrorFormat(string format, object arg0)
        {
            this._logger.ErrorFormat(format, arg0);
        }

        public void ErrorFormat(string format, object arg0, object arg1)
        {
            this._logger.ErrorFormat(format, arg0, arg1);
        }

        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            this._logger.ErrorFormat(format, arg0, arg1, arg2);
        }

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            this._logger.ErrorFormat(provider, format, args);
        }

        public void Fatal(object message)
        {
            this._logger.Fatal(message);
        }

        public void Fatal(object message, Exception exception)
        {
            this._logger.Fatal(message, exception);
        }

        public void FatalFormat(string format, params object[] args)
        {
            this._logger.FatalFormat(format, args);
        }

        public void FatalFormat(string format, object arg0)
        {
            this._logger.FatalFormat(format, arg0);
        }

        public void FatalFormat(string format, object arg0, object arg1)
        {
            this._logger.FatalFormat(format, arg0, arg1);
        }

        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            this._logger.FatalFormat(format, arg0, arg1, arg2);
        }

        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            this._logger.FatalFormat(provider, format, args);
        }

        public void Info(object message)
        {
            this._logger.Info(message);
        }

        public void Info(object message, Exception exception)
        {
            this._logger.Info(message, exception);
        }

        public void InfoFormat(string format, params object[] args)
        {
            this._logger.InfoFormat(format, args);
        }

        public void InfoFormat(string format, object arg0)
        {
            this._logger.InfoFormat(format, arg0);
        }

        public void InfoFormat(string format, object arg0, object arg1)
        {
            this._logger.InfoFormat(format, arg0, arg1);
        }

        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            this._logger.InfoFormat(format, arg0, arg1, arg2);
        }

        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            this._logger.InfoFormat(provider, format, args);
        }

        public void Warn(object message)
        {
            this.Warn(message);
        }

        public void Warn(object message, Exception exception)
        {
            this.Warn(message, exception);
        }

        public void WarnFormat(string format, params object[] args)
        {
            this.WarnFormat(format, args);
        }

        public void WarnFormat(string format, object arg0)
        {
            this.WarnFormat(format, arg0);
        }

        public void WarnFormat(string format, object arg0, object arg1)
        {
            this.WarnFormat(format, arg0, arg1);
        }

        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            this.WarnFormat(format, arg0, arg1, arg2);
        }

        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            this.WarnFormat(provider, format, args);
        }
    }
}
