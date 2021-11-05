using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkhomeLogger.Abstractions;

namespace ThinkhomeLogger.Logging
{
    public class ThinkhomeLoggerProvider : ILoggerProvider
    {
        private readonly ThinkhomeLoggerOptions _loggerOptions;
        public ThinkhomeLoggerProvider(ThinkhomeLoggerOptions loggerOptions)
        {
            _loggerOptions = loggerOptions;
        }
        private readonly ConcurrentDictionary<string, ThinkhomeLogger> _loggers = new ConcurrentDictionary<string, ThinkhomeLogger>();
        
        public ILogger CreateLogger(string categoryName) => _loggers.GetOrAdd(categoryName, name => new ThinkhomeLogger(categoryName, _loggerOptions));
        public void Dispose() => this._loggers.Clear();
    }
}
