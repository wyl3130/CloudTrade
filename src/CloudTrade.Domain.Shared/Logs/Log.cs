using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Targets;
namespace CloudTrade.Domain.Shared.Logs
{
    public static  class Log
    {
        public static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        static Log()
        {
            
        }

        public static void LogTrace(string message)
        {
            logger.Trace(message);
        }
        public static void LogWarning(string message)
        {
            logger.Warn(message);
        }
        public static void LogError(string message)
        {
            logger.Error(message);
        }
        public static void LogFatal(string message)
        {
            logger.Fatal(message);
        }
        public static void LogInfo(string message)
        {
            logger.Info(message);
        }
        public static void LogError(Exception ex)
        {
            logger.Error(ex.Message + ex.StackTrace);
        }
    }
}
