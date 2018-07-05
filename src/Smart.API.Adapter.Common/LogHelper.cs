﻿using log4net;
using System;
using System.IO;

namespace Smart.API.Adapter.Common
{
    public class LogHelper
    {
        private static ILog _InfoLog;

        static LogHelper()
        {
            //_ErrorLog = log4net.LogManager.GetLogger("logerror");
            _InfoLog = log4net.LogManager.GetLogger("loginfo");
        }

        public static void RegisterLog4Config(string filePath)
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo(filePath));
        }

        public static void Error(string message, Exception exception = null)
        {
            if (exception == null)
                _InfoLog.Error(message);
            else
                _InfoLog.Error(message, exception);
        }

        public static void Warn(string message, Exception exception = null)
        {

            if (exception == null)
                _InfoLog.Warn(message);
            else
                _InfoLog.Warn(message, exception);
        }

        public static void Info(string message, Exception exception = null)
        {
            if (exception == null)
                _InfoLog.Info(message);
            else
                _InfoLog.Info(message, exception);
        }

        public static void Debug(string message, Exception exception = null)
        {

            if (exception == null)
                _InfoLog.Debug(message);
            else
                _InfoLog.Debug(message, exception);
        }

        public static void Fatal(string message, Exception exception = null)
        {

            if (exception == null)
                _InfoLog.Fatal(message);
            else
                _InfoLog.Fatal(message, exception);
        }
    }
}
