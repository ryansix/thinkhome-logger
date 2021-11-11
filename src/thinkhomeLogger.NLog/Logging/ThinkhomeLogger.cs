using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkhomeLogger.Abstractions;

namespace ThinkhomeLogger.Logging
{
    public class ThinkhomeLogger : ILogger
    {
        private readonly string _name;
        private readonly ThinkhomeLoggerOptions _loggerOptions;
        private readonly IThinkhomeNLogService _thinkhomeNLogService;

        public ThinkhomeLogger(string name, ThinkhomeLoggerOptions loggerOptions)
        {
            _name = name;
            _loggerOptions = loggerOptions;
            _thinkhomeNLogService = new ThinkhomeNLogService(name,typeof(ThinkhomeLogger));
        }
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            this.Log<TState>(_name, logLevel, eventId, state, exception, formatter);
        }

        protected void Log<TState>(string subject,LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var logContext = new ThinkhomeLoggerData(_loggerOptions);
            NLog.LogLevel nlogLogLevel = NLog.LogLevel.Trace;
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
            _thinkhomeNLogService.Log(nlogLogLevel, _loggerOptions, subject, message);
        }
        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

        public System.IDisposable BeginScope<TState>(TState state) => NullScope.Instance;
    }
}
