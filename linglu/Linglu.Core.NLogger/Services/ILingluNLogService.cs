
using Linglu.Core.Abstractions;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linglu.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILingluNLogService
    {
        /// <summary>
        ///  写入日志
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="loggerOptions"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        void Log(LogLevel logLevel, LingluLoggerOptions loggerOptions, string subject, string message);

        void LogAudit(LogLevel logLevel, LingluAuditLoggerData data);
    }
}
