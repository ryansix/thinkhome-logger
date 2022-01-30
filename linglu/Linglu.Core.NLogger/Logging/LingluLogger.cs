using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using Linglu.Core.Abstractions;

namespace Linglu.Core.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public class LingluLogger : ILogger
    {
        private readonly string _name;
        private readonly LingluLoggerOptions _loggerOptions;
        private readonly ILingluNLogService _lingluNLogService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="loggerOptions"></param>
        public LingluLogger(string name, LingluLoggerOptions loggerOptions)
        {
            _name = name;
            _loggerOptions = loggerOptions; 
            _lingluNLogService = new LingluNLogService(name, typeof(LingluLogger));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="logLevel"></param>
        /// <param name="eventId"></param>
        /// <param name="state"></param>
        /// <param name="exception"></param>
        /// <param name="formatter"></param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            this.Log(_name, logLevel, eventId, state, exception, formatter);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="subject"></param>
        /// <param name="logLevel"></param>
        /// <param name="eventId"></param>
        /// <param name="state"></param>
        /// <param name="exception"></param>
        /// <param name="formatter"></param>
        protected void Log<TState>(string subject, LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var nlogLogLevel = NLog.LogLevel.Trace;
            switch (logLevel)
            {
                case Microsoft.Extensions.Logging.LogLevel.Trace:
                    nlogLogLevel = NLog.LogLevel.Trace;
                    break;

                case Microsoft.Extensions.Logging.LogLevel.Debug:
                    nlogLogLevel = NLog.LogLevel.Debug;
                    break;

                case Microsoft.Extensions.Logging.LogLevel.Information:
                    nlogLogLevel = NLog.LogLevel.Info;
                    break;

                case Microsoft.Extensions.Logging.LogLevel.Warning:
                    nlogLogLevel = NLog.LogLevel.Warn;
                    break;

                case Microsoft.Extensions.Logging.LogLevel.Error:
                    nlogLogLevel = NLog.LogLevel.Error;
                    break;

                case Microsoft.Extensions.Logging.LogLevel.Critical:
                    nlogLogLevel = NLog.LogLevel.Fatal;
                    break;
            }
            string message = formatter(state, exception);
            if (string.IsNullOrWhiteSpace(message)) return;
            _lingluNLogService.Log(nlogLogLevel, _loggerOptions, subject, message);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state"></param>
        /// <returns></returns>
        public System.IDisposable BeginScope<TState>(TState state) => NullScope.Instance;
    }
}
