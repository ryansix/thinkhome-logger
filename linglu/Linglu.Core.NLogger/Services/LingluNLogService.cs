
using Linglu.Core.Abstractions;
using Microsoft.Extensions.Options;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Linglu.Core
{
   public class LingluNLogService:ILingluNLogService
    {
        private readonly ILogger _logger;
        private readonly LingluLoggerOptions lingluLogger;
        public LingluNLogService()
        {
            _logger = LogManager.GetCurrentClassLogger(); //LogManager.GetLogger("LingluLogger", typeof(LingluNLogService)); //LogManager.GetCurrentClassLogger();// LogManager.GetLogger(name,type);
        } 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        public LingluNLogService(string name, Type type):this()
        { 
            _logger = LogManager.GetLogger(name, type); //LogManager.GetCurrentClassLogger();// LogManager.GetLogger(name,type);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="loggerOptions"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        public void Log(LogLevel logLevel, LingluLoggerOptions loggerOptions, string subject, string message)
        {
            var logEventInfo = CreateLogEventInfo(loggerOptions, logLevel, subject, message);
            _logger.Log(logEventInfo);
        }

      

        public void LogAudit(LogLevel logLevel, LingluAuditLoggerData data)
        { 
            var logEventInfo = LogEventInfo.Create(logLevel, string.Empty, string.Empty);
            logEventInfo.Properties["context"] = JsonSerializer.Serialize(data, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
            logEventInfo.Message = JsonSerializer.Serialize(data, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
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
        internal LogEventInfo CreateLogEventInfo(LingluLoggerOptions loggerOptions, NLog.LogLevel logLevel, string subject, string message)
        {
            var logEventInfo = LogEventInfo.Create(logLevel, subject, string.Empty);
            var msg = new LingluLoggerData(loggerOptions, logLevel)
            {
                Subject = subject,
                Content = message,
                Source = loggerOptions.Source
            };
            
            logEventInfo.Properties["context"] = JsonSerializer.Serialize(msg, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
            logEventInfo.Message = JsonSerializer.Serialize(msg, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
            return logEventInfo;
        }

    }
}
