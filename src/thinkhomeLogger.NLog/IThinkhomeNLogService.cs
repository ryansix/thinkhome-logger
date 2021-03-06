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
    public interface IThinkhomeNLogService
    {
        /// <summary>
        ///  写入日志
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="loggerOptions"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        public void Log(LogLevel logLevel, ThinkhomeLoggerOptions loggerOptions, string subject, string message); 
    }
}
