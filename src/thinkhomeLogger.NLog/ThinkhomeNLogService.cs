using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkhomeLogger.Abstractions;

namespace ThinkhomeLogger
{
    public class ThinkhomeNLogService : IThinkhomeNLogService
    { 
        private readonly ILogger _logger;
        public ThinkhomeNLogService() {
            _logger = LogManager.GetCurrentClassLogger();
        }
        public void Log(LogLevel logLevel, ThinkhomeLoggerOptions loggerOptions, string subject, string message)
        {
              var logEventInfo = CreateLogEventInfo(loggerOptions,logLevel,subject,message);
            _logger.Log(logEventInfo);
        }

        internal LogEventInfo CreateLogEventInfo(ThinkhomeLoggerOptions loggerOptions,NLog.LogLevel logLevel, string subject, string message)
        {
            var logEventInfo = LogEventInfo.Create(logLevel, loggerOptions.AppId, string.Empty);
            var msg = new ThinkhomeLoggerData(loggerOptions, logLevel)
            {
                Subject = subject,
                Content = message
            };
            logEventInfo.Properties["context"] = JsonConvert.SerializeObject(msg);
            return logEventInfo;
        }
    }
}
