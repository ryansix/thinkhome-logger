 
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkhomeLogger.Abstractions;

namespace ThinkhomeLogger
{
    /// <summary>
    /// 
    /// </summary>
    public class ThinkhomeNLogService : IThinkhomeNLogService
    { 
        private readonly ILogger _logger; 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        public ThinkhomeNLogService(string name,Type type) {
            _logger = LogManager.GetCurrentClassLogger();// LogManager.GetLogger(name,type);
            //_name = name;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="loggerOptions"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        public void Log(LogLevel logLevel, ThinkhomeLoggerOptions loggerOptions, string subject, string message)
        {
              var logEventInfo = CreateLogEventInfo(loggerOptions,logLevel,subject,message);
            _logger.Log(logEventInfo);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggerOptions"></param>
        /// <param name="logLevel"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        internal LogEventInfo CreateLogEventInfo(ThinkhomeLoggerOptions loggerOptions,NLog.LogLevel logLevel, string subject, string message)
        {
            var logEventInfo = LogEventInfo.Create(logLevel, subject, string.Empty);
            var msg = new ThinkhomeLoggerData(loggerOptions, logLevel)
            {
                Subject = subject,
                Content = message,
                Source = loggerOptions.Source
            };
            logEventInfo.Properties["context"] = System.Text.Json.JsonSerializer.Serialize(msg);
            return logEventInfo;
        }
    }
}
