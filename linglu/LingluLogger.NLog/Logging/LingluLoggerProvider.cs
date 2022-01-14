using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LingluLogger.Abstractions;

namespace LingluLogger.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public class LingluLoggerProvider : ILoggerProvider
    {
        private readonly LingluLoggerOptions _loggerOptions;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggerOptions"></param>
        public LingluLoggerProvider(LingluLoggerOptions loggerOptions)
        {
            _loggerOptions = loggerOptions;
        }
        private readonly ConcurrentDictionary<string, LingluLogger> _loggers = new ConcurrentDictionary<string, LingluLogger>();
        /// <summary>
        /// 创建ILogger对象
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public ILogger CreateLogger(string categoryName)
        {
            if (categoryName.Contains("Linglu"))
            {

            }
            return _loggers.GetOrAdd(categoryName, name => new LingluLogger(categoryName, _loggerOptions));
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose() => this._loggers.Clear();
    }
}
