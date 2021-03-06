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
    /// <summary>
    /// 
    /// </summary>
    public class ThinkhomeLoggerProvider : ILoggerProvider
    {
        private readonly ThinkhomeLoggerOptions _loggerOptions;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggerOptions"></param>
        public ThinkhomeLoggerProvider(ThinkhomeLoggerOptions loggerOptions)
        {
            _loggerOptions = loggerOptions;
        }
        private readonly ConcurrentDictionary<string, ThinkhomeLogger> _loggers = new ConcurrentDictionary<string, ThinkhomeLogger>();
        /// <summary>
        /// 创建ILogger对象
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public ILogger CreateLogger(string categoryName) => _loggers.GetOrAdd(categoryName, name => new ThinkhomeLogger(categoryName, _loggerOptions));
        /// <summary>
        /// 
        /// </summary>
        public void Dispose() => this._loggers.Clear();
    }
}
