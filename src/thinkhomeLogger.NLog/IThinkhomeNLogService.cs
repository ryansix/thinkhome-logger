using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkhomeLogger.Abstractions;

namespace ThinkhomeLogger
{
    public interface IThinkhomeNLogService
    {
        public void Log(LogLevel logLevel, ThinkhomeLoggerOptions loggerOptions, string subject, string message); 
    }
}
