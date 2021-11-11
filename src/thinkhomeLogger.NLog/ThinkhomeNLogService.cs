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
        public ThinkhomeNLogService(string name,Type type) {
            _logger = LogManager.GetCurrentClassLogger();// LogManager.GetLogger(name,type);
            //_name = name;
        }
        public void Log(LogLevel logLevel, ThinkhomeLoggerOptions loggerOptions, string subject, string message)
        {
              var logEventInfo = CreateLogEventInfo(loggerOptions,logLevel,subject,message);
            _logger.Log(logEventInfo);
        }

        internal LogEventInfo CreateLogEventInfo(ThinkhomeLoggerOptions loggerOptions,NLog.LogLevel logLevel, string subject, string message)
        {
            var logEventInfo = LogEventInfo.Create(logLevel, subject, string.Empty);
            var msg = new ThinkhomeLoggerData(loggerOptions, logLevel)
            {
                Subject = subject,
                Content = message,
                Source = loggerOptions.Source
            };
            logEventInfo.Properties["context"] = JsonConvert.SerializeObject(msg);
            return logEventInfo;
        }
    }
}
